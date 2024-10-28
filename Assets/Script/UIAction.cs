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

    [SerializeField] SpriteRenderer[] _timeSpriteRenderer = new SpriteRenderer[3];

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TimerUI();
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

    private void TimerUI()
    {
        (int minuts, int second) = GetTimer();
        int minuts10 = minuts / 10;
        int minuts1 = minuts % 10;
        int second10 = second / 10;
        int second1 = second % 10;
        _timeSpriteRenderer[0].sprite = _numberSprite[minuts10];
        _timeSpriteRenderer[1].sprite = _numberSprite[minuts1];
        _timeSpriteRenderer[2].sprite = _numberSprite[second10];
        _timeSpriteRenderer[3].sprite = _numberSprite[second1];
    }

    private (int, int) GetTimer()
    {
        int minuts = (int)Time.time / 60;
        int second = (int)Time.time % 60;
        return (minuts, second);
    }

}
