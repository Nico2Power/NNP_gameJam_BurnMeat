using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class PlayerActionController : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D _playerRigidbody;
    [SerializeField] private Vector2 _moveVector = Vector2.zero;
    [SerializeField] private float _moveSpeed = 10f;
    private Vector2 _smoothMove;
    private Vector2 _smoothVector = Vector2.zero;
    [SerializeField] private float _smoothTime = 0.05f;
    private Vector2 _directionVector = new Vector2(1, 0);

    [SerializeField] private float _damageForce = 100f;
    [SerializeField] private float _damageRate = 0.5f;
    private float _nextDamageTime = 0.0f;
    private bool _isDamageState = false;
    private Vector2 _contactVector = Vector2.zero;
    private bool _touchWall = false;

    private bool _isCarryMeat = false;

    [SerializeField] private float _knockRate = 0.8f;
    private float _nextknockTime = 0.0f;
    private float _knockendTime = 0f;
    private bool _isKnock = false;

    [SerializeField] private GameObject _flameShot;
    [SerializeField] private GameObject _knockBackObject;

    [SerializeField] private GameObject _meatFoodCarry = null;

    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _flameShot.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // print(_playerRigidbody.velocity);
        if (!_isDamageState)
        {
            PlayerMove();
        }
        else if (_isDamageState)
        {
            KnockBackEnd();
        }

        if (_isKnock && Time.time >= _knockendTime)
        {
            _knockBackObject.SetActive(false);
            _isKnock = false;
        }

        if (_isCarryMeat)
        {
            CarryMeatMove();
        }

    }

    private void PlayerMove()
    {
        _smoothMove = Vector2.SmoothDamp(_smoothMove, _moveVector, ref _smoothVector, _smoothTime);
        _playerRigidbody.velocity = _smoothMove * _moveSpeed;
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

    }

    public void Fire(InputAction.CallbackContext callbackContext)
    {
        // print(callbackContext);

        if (_directionVector.x > 0)
        {
            _flameShot.transform.localPosition = new Vector2(4, _flameShot.transform.localPosition.y);
        }
        else if (_directionVector.x < 0)
        {
            _flameShot.transform.localPosition = new Vector2(-4, _flameShot.transform.localPosition.y);
        }

        if (callbackContext.started)
        {
            print("fire");
            _flameShot.SetActive(true);
        }
        if (callbackContext.canceled)
        {
            print("stop fire");
            _flameShot.SetActive(false);
        }
    }


    public void Knock(InputAction.CallbackContext callbackContext)
    {

        if (_directionVector.x > 0)
        {
            _knockBackObject.transform.localPosition = new Vector2(1.65f, _knockBackObject.transform.localPosition.y);
        }
        else if (_directionVector.x < 0)
        {
            _knockBackObject.transform.localPosition = new Vector2(-1.65f, _knockBackObject.transform.localPosition.y);
        }

        if (callbackContext.started && !_isDamageState)
        {
            _knockendTime = Time.time + 0.1f;
            _nextknockTime = Time.time + _knockRate;
            _knockBackObject.SetActive(true);
            _isKnock = true;
            // print("knock");
        }

    }

    public void Carry(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            _isCarryMeat = true;
        }
        else if (callbackContext.canceled)
        {
            _isCarryMeat = false;
        }
    }

    private void CarryMeatMove()
    {
        if (_meatFoodCarry != null)
        {
            _meatFoodCarry.transform.position = gameObject.transform.position + new Vector3(0, 0.65f);
            print("carry");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (Time.time >= _nextDamageTime && other.gameObject.tag == "MeatEnemy")
        {
            _isDamageState = true;
            _nextDamageTime = Time.time + _damageRate;
            Vector2 _vectorDifference = (transform.position - other.transform.position).normalized;
            Vector2 _force = _vectorDifference * _damageForce;
            _contactVector = transform.position;
            _playerRigidbody.velocity = _force;
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
            print("Enter " + _meatFoodCarry.name);
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
