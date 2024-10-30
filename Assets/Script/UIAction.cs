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
    public static int _playerSroce = 0;
    public static float _playerTime = 0.0f;

    [SerializeField] SpriteRenderer[] _gunAmmoSpriteRenderer = new SpriteRenderer[1];
    [SerializeField] Sprite[] _numberSprite = new Sprite[9];

    [SerializeField] SpriteRenderer[] _timeSpriteRenderer = new SpriteRenderer[3];
    [SerializeField] SpriteRenderer[] _scoreSpriteRenderer = new SpriteRenderer[2];
    [SerializeField] GameObject[] _scoreGameObject = new GameObject[2];


    void Start()
    {
        _scoreGameObject[0].SetActive(true);
        _scoreGameObject[1].SetActive(false);
        _scoreGameObject[2].SetActive(false);
        SroceUI(_playerSroce);
        TimerUI();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_playerTime == 0.0f) TimerUI();
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
        if (_hpCount >= 0.0f)
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
        if (_playerTime > 0.0f)
        {
            minuts = (int)_playerTime / 60;
            second = (int)_playerTime % 60;
        }
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
        int minuts = (int)Time.timeSinceLevelLoad / 60;
        int second = (int)Time.timeSinceLevelLoad % 60;
        return (minuts, second);
    }

    public void SrocePlus()
    {
        _playerSroce++;
        SroceUI(_playerSroce);
    }

    public void SroceUI(int sroce)
    {
        int sroce100 = sroce / 100;
        int sroce10 = sroce % 100 / 10;
        int sroce1 = sroce % 10;

        _scoreSpriteRenderer[0].sprite = _numberSprite[sroce1];
        if (sroce10 > 0)
        {
            _scoreSpriteRenderer[1].sprite = _numberSprite[sroce10];
            _scoreGameObject[1].SetActive(true);

        }
        if (sroce100 > 0)
        {
            _scoreSpriteRenderer[2].sprite = _numberSprite[sroce100];
            _scoreGameObject[2].SetActive(true);

        }
    }


}
