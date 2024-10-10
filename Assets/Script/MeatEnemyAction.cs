using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeatEnemyAction : MonoBehaviour
{
    private Rigidbody2D _meatEnemyRigidbody;
    private SpriteRenderer _meatRenderer;
    private Collider2D _meatCollider;
    [SerializeField] private float _damageRate = 0.1f;
    private float _nextDamageTime = 0.0f;
    // [SerializeField] private float _damageForce = 1.0f;
    private int _coldHP = 100;
    private int _donenessHP = 100;
    [SerializeField] private Vector3 _playLastPosition = Vector3.zero;
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private float _meatMoveSpeed = 5f;
    private bool _touchPlayer = false;
    private float _nextTrackTime = 1.0f;

    void Start()
    {
        _meatEnemyRigidbody = GetComponent<Rigidbody2D>();
        _meatRenderer = GetComponent<SpriteRenderer>();
        _meatRenderer.color = new Color32(108, 222, 243, 255);
        _meatCollider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (_coldHP > 0) MeatMove();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("hit!");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time >= _nextDamageTime && other.gameObject.tag == "FireDamage")
        {
            _nextDamageTime = Time.time + _damageRate;
            print("fire!!!!!!!!!!!!" + _nextDamageTime);
            HPControl();
        }
    }

    private void HPControl()
    {
        if (_coldHP > 0) //冷凍肉
        {
            print("解凍中");
            _coldHP -= 5;
        }
        else if (_coldHP <= 0 && _donenessHP <= 100 && _donenessHP > 30) //生肉
        {
            gameObject.tag = "MeatRaw";
            print("烤肉中");
            _donenessHP -= 5;
            _meatRenderer.color = new Color32(241, 124, 129, 255);

            _meatCollider.isTrigger = true;
            print(gameObject.transform.localRotation.eulerAngles.z);
            if (gameObject.transform.localRotation.eulerAngles.z == 0)
            {
                gameObject.transform.Rotate(0, 0, 90);
            }
        }
        else if (_coldHP <= 0 && _donenessHP <= 30 && _donenessHP > 0)//熟肉
        {
            gameObject.tag = "MeatFood";
            print("熟的剛好");
            _donenessHP -= 5;
            _meatRenderer.color = new Color32(150, 99, 99, 255);
        }
        else if (_coldHP <= 0 && _donenessHP <= 0)//烤焦
        {
            gameObject.tag = "MeatBurnt";
            print("全熟有點焦");
            _meatRenderer.color = new Color32(56, 23, 23, 255);
        }
        else
        {
            print("例外");
        }
    }

    private void MeatMove()
    {
        if (_playLastPosition != _playerGameObject.transform.position)
        {
            _playLastPosition = _playerGameObject.transform.position;
        }
        _meatEnemyRigidbody.transform.position = Vector3.MoveTowards(transform.position, _playLastPosition, _meatMoveSpeed * Time.deltaTime);

        if (_touchPlayer == true)
        {
            _nextTrackTime -= Time.deltaTime;
            if (_nextTrackTime <= 0.0f)
            {
                _meatEnemyRigidbody.simulated = true;
                _touchPlayer = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && gameObject.tag == "MeatEnemy")
        {
            _meatEnemyRigidbody.simulated = false;
            _touchPlayer = true;
            _nextTrackTime = 1.0f;
        }
    }
}
