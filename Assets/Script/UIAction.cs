using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIAction : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] _hpGameObjectArray = new GameObject[6];
    private int _hpCount = 6;

    [SerializeField] SpriteRenderer[] _gunAmmoSpriteRenderer = new SpriteRenderer[1];
    [SerializeField] Sprite[] _numberSprite = new Sprite[9];

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            GunAmmoUI(10);
        }

    }

    public void GunAmmoUI(int ammo)
    {
        int ammo10 = ammo / 10;
        int ammo1 = ammo % 10;

        _gunAmmoSpriteRenderer[0].sprite = _numberSprite[ammo10];
        _gunAmmoSpriteRenderer[1].sprite = _numberSprite[ammo1];

    }

    public void DamageUI()
    {
        if (_hpCount >= 0)
        {
            _hpGameObjectArray[_hpCount].SetActive(false);
            _hpCount--;
            if (_hpCount < 0)
            {
                print("die!!!!!!!!!!");
            }
        }
    }

}
