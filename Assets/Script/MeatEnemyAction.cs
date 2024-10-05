using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeatEnemyAction : MonoBehaviour
{
    private Rigidbody2D _meatEnemyRigidbody;
    private SpriteRenderer _meatRenderer;
    [SerializeField] private float _damageRate = 0.5f;
    private float _nextDamageTime = 0.0f;
    // [SerializeField] private float _damageForce = 1.0f;
    private int _coldHP = 10;
    private int _donenessHP = 10;
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
        if (_coldHP > 0)
        {
            print("解凍中");
            _coldHP--;
        }
        else if (_coldHP <= 0 && _donenessHP > 0)
        {
            gameObject.tag = "MeatFood";
            print("烤肉中");
            _donenessHP--;
            _meatRenderer.color = new Color32(241, 124, 129, 255);
        }
        else if (_coldHP <= 0 && _donenessHP <= 0)
        {
            print("全熟有點焦");
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
        if (other.gameObject.tag == "Player")
        {
            _meatEnemyRigidbody.simulated=false;
            _touchPlayer = true;
            _nextTrackTime = 1.0f;
        }
    }
}
