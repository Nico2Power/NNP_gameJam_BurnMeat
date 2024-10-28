using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class MeatEnemyAction : MonoBehaviour
{
    private Rigidbody2D _meatEnemyRigidbody;
    private SpriteRenderer _meatRenderer;
    private Collider2D _meatCollider;
    [SerializeField] private float _damageRate = 0.1f;
    private float _nextDamageTime = 0.0f;

    [SerializeField] private int _coldHP = 100;
    [SerializeField] private int _donenessHP = 100;
    private int _darknessPoint = 0;
    private int _darknessPointMax = 100;
    private float _nextDarknessTime = 0.0f;

    private bool _isToDark = false;

    [SerializeField] private Vector3 _playLastPosition = Vector3.zero;
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private float _meatMoveSpeed = 5f;
    private bool _touchPlayer = false;
    private float _nextTrackTime = 1.0f;

    private Collider2D _wallColllider2D;

    [SerializeField] float _knockBackForce = 100f;
    private bool _isHit = false;

    [SerializeField] GameObject[] _hpGameObject = new GameObject[3];

    private bool _isEnterGround = false;
    void Start()
    {
        _meatEnemyRigidbody = GetComponent<Rigidbody2D>();
        _meatRenderer = GetComponent<SpriteRenderer>();
        _meatRenderer.color = new Color32(108, 222, 243, 255);
        _meatCollider = GetComponent<Collider2D>();

    }

    private void FixedUpdate()
    {
        if ((gameObject.tag == "MeatEnemy" || gameObject.tag == "MeatDark") && !_isHit)
        {
            MeatMove();
        }
        else if (_isHit)
        {
            OnHitEnd();
        }

        if (_touchPlayer)
        {
            AttackPlayer();
        }

        Reborn();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PunchBack")
        {
            _isHit = true;
            DoKnockBack();
        }

        if (other.gameObject.tag == "Wall" && (gameObject.tag != "MeatEnemy" || gameObject.tag != "MeatDark"))
        {
            OverWallPoistion(other);
        }

        if (other.gameObject.tag == "Gate")
        {
            _isEnterGround = true;
            Physics2D.IgnoreCollision(_wallColllider2D, _meatCollider, false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time >= _nextDamageTime && other.gameObject.tag == "FireDamage")
        {
            DamageControl();
            HPControl();
            HpBarControl();
            _nextDamageTime = Time.time + _damageRate;
            print("fire!!!!!!!!!!!!" + _nextDamageTime);
        }

        if (other.gameObject.tag == "Wall" && gameObject.tag != "MeatEnemy")
        {
            OverWallPoistion(other);
        }
    }

    private void DamageControl()
    {
        if (gameObject.tag != "MeatDark")
        {
            if (_coldHP > 0) _coldHP -= 5;
            else _donenessHP -= 5;

            _darknessPoint = 0;
        }
        else if (gameObject.tag == "MeatDark")
        {
            _darknessPoint -= 10;
        }

    }

    private void OnHitEnd()
    {
        if (_meatEnemyRigidbody.velocity == Vector2.zero)
        {
            _isHit = false;
        }
    }

    private void OverWallPoistion(Collider2D wall)
    {
        _meatEnemyRigidbody.velocity = Vector2.zero;
        _meatEnemyRigidbody.angularVelocity = 0;
        print("wall");

        switch (wall.gameObject.name)
        {
            case "wall1":
                _meatEnemyRigidbody.transform.position = new Vector2(-10.19f, _meatEnemyRigidbody.transform.position.y);
                break;
            case "wall2":
                _meatEnemyRigidbody.transform.position = new Vector2(10.17f, _meatEnemyRigidbody.transform.position.y);
                break;
            case "wall3":
                _meatEnemyRigidbody.transform.position = new Vector2(_meatEnemyRigidbody.transform.position.x, 5.78f);
                break;
            case "wall4":
                _meatEnemyRigidbody.transform.position = new Vector2(_meatEnemyRigidbody.transform.position.x, -5.78f);
                break;
        }
    }

    private void HPControl()
    {
        if (gameObject.tag != "MeatDark")
        {
            if (_coldHP > 0) //冷凍肉
            {
                print("解凍中");
            }

            if (_coldHP <= 0 && _donenessHP <= 100 && _donenessHP > 30) //生肉
            {
                gameObject.tag = "MeatRaw";
                print("烤肉中");
                _meatRenderer.color = new Color32(241, 124, 129, 255);
                _meatCollider.isTrigger = true;
                print(gameObject.transform.localRotation.eulerAngles.z);
                if (gameObject.transform.localRotation.eulerAngles.z == 0)
                {
                    gameObject.transform.Rotate(0, 0, 90);
                }
            }

            if (_coldHP <= 0 && _donenessHP <= 30 && _donenessHP > 0)//熟肉
            {
                gameObject.tag = "MeatFood";
                print("熟的剛好");
                _meatRenderer.color = new Color32(150, 99, 99, 255);
            }

            if (_coldHP <= 0 && _donenessHP <= 0)//烤焦
            {
                gameObject.tag = "MeatBurnt";
                print("全熟有點焦");
                _meatRenderer.color = new Color32(56, 23, 23, 255);
            }
        }
        else if (gameObject.tag == "MeatDark")
        {
            if (_darknessPoint <= 0)//烤焦
            {
                gameObject.tag = "MeatBurnt";
                print("全熟有點焦");
                _meatRenderer.color = new Color32(56, 23, 23, 255);
            }
        }

    }

    private void HpBarControl()
    {
        float coldScale = (float)_coldHP * 2 / 100;
        float donenessScale = (float)(_donenessHP - 30) * 2 / 70;
        float burntScale = (float)_donenessHP * 2 / 30;

        if (_coldHP >= 0)
        {
            _hpGameObject[0].transform.localScale = new Vector3(coldScale, _hpGameObject[0].transform.localScale.y, _hpGameObject[0].transform.localScale.z);
        }

        if (_donenessHP >= 0)
        {
            if (_donenessHP >= 30)
            {
                _hpGameObject[1].transform.localScale = new Vector3(donenessScale, _hpGameObject[1].transform.localScale.y, _hpGameObject[1].transform.localScale.z);
            }
            if (_donenessHP <= 30)
            {
                _hpGameObject[2].transform.localScale = new Vector3(burntScale, _hpGameObject[2].transform.localScale.y, _hpGameObject[2].transform.localScale.z);
            }
        }

        if (gameObject.tag == "MeatDark" && _darknessPoint >= 0)
        {
            float darknessScale = (float)_darknessPoint * 2 / 100;
            _hpGameObject[3].transform.localScale = new Vector3(darknessScale, _hpGameObject[3].transform.localScale.y, _hpGameObject[3].transform.localScale.z);
        }
    }
    Vector3 lastMeatPosition;
    private void Reborn()
    {
        if (gameObject.tag == "MeatFood" || gameObject.tag == "MeatBurnt" || gameObject.tag == "MeatRaw")
        {

            if (!_isToDark)
            {
                if (_darknessPoint < _darknessPointMax && Time.time >= _nextDarknessTime)
                {
                    _darknessPoint += 10;
                    _nextDarknessTime = Time.time + 1f;
                    lastMeatPosition = gameObject.transform.position;
                }

                if (gameObject.transform.position != lastMeatPosition) _darknessPoint = 0;


                float darknessScale = (float)_darknessPoint * 2 / 100;
                _hpGameObject[3].transform.localScale = new Vector3(darknessScale, _hpGameObject[3].transform.localScale.y, _hpGameObject[3].transform.localScale.z);

                if (_darknessPoint >= _darknessPointMax)
                {
                    gameObject.tag = "MeatDark";
                    _meatRenderer.color = new Color32(189, 0, 195, 255);
                    _meatCollider.isTrigger = false;
                    _meatMoveSpeed = 6f;
                    _donenessHP = 0;
                }
            }


        }

    }

    private void MeatMove()
    {
        if (_playLastPosition != _playerGameObject.transform.position)
        {
            _playLastPosition = _playerGameObject.transform.position;
        }
        _meatEnemyRigidbody.transform.position = Vector3.MoveTowards(transform.position, _playLastPosition, _meatMoveSpeed * Time.deltaTime);

    }

    private void AttackPlayer()
    {
        _nextTrackTime -= Time.deltaTime;
        if (_nextTrackTime <= 0.0f)
        {
            _meatEnemyRigidbody.simulated = true;
            _touchPlayer = false;
        }
    }

    private void DoKnockBack()
    {
        Vector2 _knockBackVector = (gameObject.transform.position - _playerGameObject.transform.position).normalized;
        _meatEnemyRigidbody.AddForce(_knockBackVector * _knockBackForce, ForceMode2D.Impulse);

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall" && !_isEnterGround)
        {
            _wallColllider2D = other.collider;
            Physics2D.IgnoreCollision(_wallColllider2D, _meatCollider, true);
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {

        if (other.gameObject.tag == "Player" && (gameObject.tag == "MeatEnemy" || gameObject.tag == "MeatDark"))
        {
            _meatEnemyRigidbody.simulated = false;
            _touchPlayer = true;
            _nextTrackTime = 1.0f;
        }
    }


}
