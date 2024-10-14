using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackAction : MonoBehaviour
{

    GameObject _playerParentObject;
    // [SerializeField] float _knockBackForce = 80f;
    float _knockEndRate = 3f;
    float _knockEndTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        _playerParentObject = gameObject.transform.parent.gameObject;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameObject.activeSelf)
        {
            _knockEndTime = Time.time + _knockEndRate;
        }
        else if (gameObject.activeSelf && Time.time >= _knockEndTime)
        {
            gameObject.SetActive(false);
        }
    }


}
