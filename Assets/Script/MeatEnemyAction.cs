using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatEnemyAction : MonoBehaviour
{
    private Rigidbody2D _meatEnemyRigidbody;
    [SerializeField] private float _damageRate = 0.5f;
    private float _nextDamageTime = 0.0f;
    // [SerializeField] private float _damageForce = 1.0f;
    private int _coldHP = 10;
    private int _donenessHP = 10;

    void Start()
    {
        _meatEnemyRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // if (Time.time >= _nextDamageTime)
        // {
        //     _nextDamageTime = Time.time + _damageRate;
        //     print("fire!!!!!!!!!!!!"+_nextDamageTime);
        // }
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
            // _meatEnemyRigidbody.AddForce(transform.right*_damageForce);
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
        else if(_coldHP<=0&&_donenessHP>0){
            print("烤肉中");
            _donenessHP--;
        }else if(_coldHP<=0&&_donenessHP<=0){
            print("全熟有點焦");
        }else{
            print("例外");
        }
    }
}
