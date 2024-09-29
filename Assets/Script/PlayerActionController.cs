using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private GameObject _flameShot;


    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _flameShot.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMove();
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
    }

    public void Fire(InputAction.CallbackContext callbackContext)
    {
        print(callbackContext);
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

}