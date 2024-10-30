using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerActionController : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D _playerRigidbody;
    private Collider2D _playerCollider;
    [SerializeField] private Vector2 _moveVector = Vector2.zero;
    [SerializeField] private float _moveSpeed = 10f;
    private Vector2 _smoothMove;
    private Vector2 _smoothVector = Vector2.zero;
    [SerializeField] private float _smoothTime = 0.05f;
    private Vector2 _directionVector = new Vector2(1, 0);
    private float _fireVectorX = 0.0f;

    [SerializeField] private int _playerHp = 6;
    [SerializeField] private float _damageForce = 100f;
    [SerializeField] private float _damageRate = 0.5f;
    private float _nextDamageTime = 0.0f;
    private bool _isDamageState = false;
    private Vector2 _contactVector = Vector2.zero;
    private bool _touchWall = false;

    private bool _isCarryMeat = false;
    private bool _isPunch = false;
    private bool _isFire = false;
    private bool _isReload = false;
    private bool _isDown = false;
    private bool _isSlow = false;
    private bool _isPressButton = false;

    [SerializeField] private int _gunAmmo = 50;
    [SerializeField] private float _ammoRate = 0.25f;
    private float _nextAmmoTime = 0.0f;

    [SerializeField] private GameObject _flameShot;
    [SerializeField] private GameObject _punchBackObject;
    [SerializeField] private GameObject _carryItem;


    [SerializeField] private GameObject _meatFoodCarry = null;
    [SerializeField] private UIAction _UI;

    private SpriteRenderer _playerSpriteRenderer;
    private SpriteRenderer _punchSpriteRenderer;
    private SpriteRenderer _fireSpriteRenderer;


    private Animator _playerAnimator;

    private PlayerInput _playInputMap;
    public AudioAction _AA;

    void Start()
    {
        _AA = GameObject.Find("AudioControl").GetComponent<AudioAction>();
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _flameShot.SetActive(false);
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<Animator>();
        _playInputMap = GetComponent<PlayerInput>();
        _playerCollider = GetComponent<Collider2D>();
        _UI = GameObject.Find("UI").GetComponent<UIAction>();
        _playInputMap.SwitchCurrentActionMap("PlayerNormal");
        _punchSpriteRenderer = _punchBackObject.GetComponent<SpriteRenderer>();
        _fireSpriteRenderer = _flameShot.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.timeScale == 0.0f && _playInputMap.currentActionMap.name == "PlayerNormal") _playInputMap.SwitchCurrentActionMap("PlayerPause");
        else if (Time.timeScale == 1.0f && _playInputMap.currentActionMap.name == "PlayerPause") _playInputMap.SwitchCurrentActionMap("PlayerNormal");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // print(_playerRigidbody.velocity);
        if (!_isDamageState && !_isDown)
        {
            PlayerMove();
            FireControl();
            CarryMeatMove();
        }
        else if (_isDamageState)
        {
            KnockBackEnd();
        }
    }


    private void PlayerMove()
    {

        if (_isSlow) _moveSpeed = 5f; else if (!_isSlow) _moveSpeed = 10f;

        _smoothMove = Vector2.SmoothDamp(_smoothMove, _moveVector, ref _smoothVector, _smoothTime);
        _playerRigidbody.velocity = _smoothMove * _moveSpeed;

        if ((_directionVector.x > 0 && !_isFire) || (_fireVectorX > 0 && _isFire))
        {
            _playerSpriteRenderer.flipX = false;
        }
        else if ((_directionVector.x < 0 && !_isFire) || (_fireVectorX < 0 && _isFire))
        {
            _playerSpriteRenderer.flipX = true;
        }

        if (_playerSpriteRenderer.flipX == true) _playerCollider.offset = new Vector2(0.17f, 0);
        else if (_playerSpriteRenderer.flipX == false) _playerCollider.offset = new Vector2(-0.17f, 0);
        // print(_smoothMove);
        //利用SmoothDamp會逐漸達到目標值(_moveVector)的功能來實現平滑移動,而非直接的移動
    }

    public void Move(InputAction.CallbackContext callBackContext)
    {
        _moveVector = callBackContext.ReadValue<Vector2>();
        //取得按鍵相應Vector2向量

        if (_moveVector.x != 0)
        {
            _directionVector = _moveVector;
        }
        //確定最後面對方向 控制開火方向



        if (_moveVector != Vector2.zero)
        {
            _playerAnimator.SetBool("isWalk", true);
        }
        else
        {
            _playerAnimator.SetBool("isWalk", false);
        }

        if (_isFire && (_moveVector.x + _fireVectorX) == 0)
        {
            _playerAnimator.SetBool("isBack", true);
        }
        else
        {
            _playerAnimator.SetBool("isBack", false);
        }

    }

    public void Fire(InputAction.CallbackContext callbackContext)
    {
        if (!_isPunch && !_isCarryMeat && !_isReload && !_isDown)
        {
            if (callbackContext.started)
            {
                _isPressButton = true;
                _isFire = true;
                _fireVectorX = _directionVector.x;
                if (_directionVector.x > 0)
                {
                    _flameShot.transform.localPosition = new Vector2(1.63f, _flameShot.transform.localPosition.y);
                    _fireSpriteRenderer.flipX = false;
                }
                else if (_directionVector.x < 0)
                {
                    _flameShot.transform.localPosition = new Vector2(-1.63f, _flameShot.transform.localPosition.y);
                    _fireSpriteRenderer.flipX = true;

                }
                _AA.PlayClip(1, "FireKeepSE", true);
            }

            if (callbackContext.canceled)
            {
                _isFire = false;
            }
        }
    }

    public void Reload(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && !_isDown) GunReload();
    }

    private void GunReload()
    {
        if (!_isReload)
        {
            _isReload = true;
            _isFire = false;
            _playerAnimator.SetTrigger("isReload");
        }
        else if (_isReload)
        {
            _gunAmmo = 50;
            _isReload = false;
            _UI.GunAmmoUI(_gunAmmo);
        }
    }

    private void FireControl()
    {
        if (_isFire && !_isPunch && !_isReload)
        {

            if (_gunAmmo > 0)
            {
                _flameShot.SetActive(true);
                _playerAnimator.SetBool("isFire", true);
            }

            if (Time.time >= _nextAmmoTime && _gunAmmo > 0)
            {
                _UI.GunAmmoUI(_gunAmmo);
                _gunAmmo--;
                _nextAmmoTime = Time.time + _ammoRate;
            }
            else if (_gunAmmo <= 0)
            {
                _UI.GunAmmoUI(_gunAmmo);
                GunReload();
            }
        }
        else if (!_isFire || _isPunch || _isReload)
        {

            _playerAnimator.SetFloat("PlayerMove", Math.Abs(_moveVector.x) + Math.Abs(_moveVector.y));
            _flameShot.SetActive(false);
            _playerAnimator.SetBool("isFire", false);
            _playerAnimator.SetBool("isBack", false);
        }
        if (!_isFire && _isPressButton)
        {
            _AA.StopClip(1, "FireKeepSE");
            _isPressButton = false;
        }
    }

    public void Punch(InputAction.CallbackContext callbackContext)
    {

        if ((_directionVector.x > 0 && !_isFire) || (_fireVectorX > 0 && _isFire))
        {
            _punchBackObject.transform.localPosition = new Vector2(0.825f, _punchBackObject.transform.localPosition.y);
            _punchSpriteRenderer.flipX = false;
        }
        else if ((_directionVector.x < 0 && !_isFire) || (_fireVectorX < 0 && _isFire))
        {
            _punchBackObject.transform.localPosition = new Vector2(-0.825f, _punchBackObject.transform.localPosition.y);
            _punchSpriteRenderer.flipX = true;
        }

        if (callbackContext.started && !_isDamageState && !_isPunch && !_isDown)
        {
            _flameShot.SetActive(false);
            _playerAnimator.SetTrigger("isPunch");
            _AA.PlayClip(2, "PunchSE", false);
            _playerAnimator.SetBool("isFire", false);
            _isCarryMeat = false;
            _isReload = false;
        }

    }

    //動畫事件
    private void AnimatorPunch()
    {
        if (!_isPunch)
        {
            _punchBackObject.SetActive(true);
            _isPunch = true;
        }
        else if (_isPunch)
        {
            _punchBackObject.SetActive(false);
            _isPunch = false;
        }
    }

    public void Carry(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && !_isFire)
        {
            _isCarryMeat = true;
        }
        if (callbackContext.started && _isFire)
        {
            _isSlow = true;
        }
        else if (callbackContext.canceled)
        {
            _isCarryMeat = false;
            _isSlow = false;

        }
    }

    private void CarryMeatMove()
    {
        if (_isCarryMeat)
        {
            _playerAnimator.SetBool("isCarry", true);
            _carryItem.SetActive(true);

            if (_meatFoodCarry != null)
            {
                _meatFoodCarry.transform.position = gameObject.transform.position + new Vector3(0, 0.65f);
            }
        }
        else
        {
            _playerAnimator.SetBool("isCarry", false);
            _carryItem.SetActive(false);

        }
    }

    private void PlayerDown()
    {
        _playerAnimator.SetTrigger("isDown");
        _isDown = true;
        _isDamageState = false;
        _isFire = false;
        _AA.PlayClip(2, "DieSE", false);
        UIAction._playerTime = Time.timeSinceLevelLoad;
        SceneManager.LoadScene(sceneName: "StartKitchen");

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //受傷
        if (Time.time >= _nextDamageTime && (other.gameObject.tag == "MeatEnemy" || other.gameObject.tag == "MeatDark"))
        {
            if (_playerHp > 0)
            {
                _isDamageState = true;
                _nextDamageTime = Time.time + _damageRate;
                Vector2 _vectorDifference = (transform.position - other.transform.position).normalized;
                Vector2 _force = _vectorDifference * _damageForce;
                _contactVector = transform.position;
                _playerRigidbody.velocity = _force;
                _playerAnimator.SetTrigger("isDamage");
                _UI.DamageUI();
                _AA.PlayClip(2, "PlayerDagameSE", false);
                _playerHp--;
            }
            else if (_playerHp <= 0)
            {
                PlayerDown();
                _UI.DamageUI();
            }

        }

        if (other.gameObject.tag == "Wall")
        {
            _touchWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            _touchWall = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "MeatFood" || other.gameObject.tag == "MeatBurnt") && _meatFoodCarry == null)
        {
            _meatFoodCarry = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.gameObject.tag == "MeatFood" || other.gameObject.tag == "MeatBurnt") && !_isCarryMeat)
        {
            _meatFoodCarry = null;
        }
    }

    private void KnockBackEnd()
    {

        if (!_touchWall)
        {
            double a2 = Math.Pow(transform.position.x - _contactVector.x, 2) + Math.Pow(transform.position.y - _contactVector.y, 2);
            if (Math.Sqrt(a2) >= 2f)
            {
                _playerRigidbody.velocity = Vector2.zero;
                _isDamageState = false;
            }
        }
        else if (_touchWall)
        {
            _playerRigidbody.velocity = Vector2.zero;
            _isDamageState = false;
        }

    }



}
