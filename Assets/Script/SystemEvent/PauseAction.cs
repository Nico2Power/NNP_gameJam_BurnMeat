using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseAction : MonoBehaviour
{
    public GameObject _pauseCanvasObject;
    public GameObject _pauseObject;
    public GameObject _audioObject;

    private bool _isPause = false;
    private bool _isAudio = false;

    public GameObject[] _pauseArray = new GameObject[3];
    private int _pauseIndex = 0;
    private int _lastIndex = 0;

    public GameObject[] _audioArray = new GameObject[3];
    private int _audioIndex = 0;
    private int _audiolastIndex = 0;

    public TextMeshProUGUI _bgmText;
    public TextMeshProUGUI _seText;

    public static int _bgmCount;
    public static int _seCount;

    private Canvas _pauseCanvas;

    public AudioAction _AA;

    private static PauseAction instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }

    void Start()
    {
        _AA = GameObject.Find("AudioControl").GetComponent<AudioAction>();
        _pauseCanvas = _pauseCanvasObject.GetComponent<Canvas>();
        _bgmCount = 50;
        _seCount = 50;
        ChangeText();
        

        _lastIndex = _pauseIndex;
        _audiolastIndex = _audioIndex;
    }


    // Update is called once per frame
    void Update()
    {
        PauseButton();
        if (_isPause && !_isAudio) PauseListChoose();
        if (_isPause && _isAudio) AudioListChoose();
        // print(_pauseIndex + "   " + _isPause);
        if (_pauseCanvas.worldCamera == null) _pauseCanvas.worldCamera = Camera.main;

    }

    private void PauseButton()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame||Keyboard.current[Key.P].wasPressedThisFrame)
        {
            if (!_isPause)
            {
                _pauseIndex = 0;
                _audioIndex = 0;
                Time.timeScale = 0.0f;
                _pauseCanvasObject.SetActive(true);
                _pauseObject.SetActive(true);
                _audioObject.SetActive(false);
                _isPause = true;

            }
            else if (_isPause)
            {

                Time.timeScale = 1.0f;
                _pauseCanvasObject.SetActive(false);
                _isPause = false;
                _isAudio = false;

            }
        }
    }

    private void PauseListChoose()
    {
        if (_lastIndex != _pauseIndex)
        {
            _pauseArray[_lastIndex].SetActive(false);
            _pauseArray[_pauseIndex].SetActive(true);
            _lastIndex = _pauseIndex;
        }

        if (Keyboard.current[Key.UpArrow].wasPressedThisFrame && _pauseIndex > 0) _pauseIndex--;
        else if (Keyboard.current[Key.DownArrow].wasPressedThisFrame && _pauseIndex < 3) _pauseIndex++;

        if (Keyboard.current[Key.Z].wasPressedThisFrame)
        {
            switch (_pauseIndex)
            {
                case 0:
                    if (_isPause)
                    {
                        _isPause = false;
                        Time.timeScale = 1.0f;
                        _pauseCanvasObject.SetActive(_isPause);
                    }
                    return;
                case 1:
                    _pauseObject.SetActive(false);
                    _audioObject.SetActive(true);
                    _isAudio = true;
                    return;
                case 2:
                    UIAction._playerTime = Time.timeSinceLevelLoad;
                    Time.timeScale = 1.0f;
                    _isPause = false;
                    _isAudio = false;
                    _pauseCanvasObject.SetActive(false);
                    SceneManager.LoadScene(sceneName: "StartKitchen");
                    return;
            }
            if (Keyboard.current[Key.X].wasPressedThisFrame)
            {
                _isPause = false;
                Time.timeScale = 1.0f;
                _pauseCanvasObject.SetActive(false);

            }
        }
    }

    private void AudioListChoose()
    {
        if (_audiolastIndex != _audioIndex)
        {
            _audioArray[_audiolastIndex].SetActive(false);
            _audioArray[_audioIndex].SetActive(true);
            _audiolastIndex = _audioIndex;
        }
        ChangeText();


        if (Keyboard.current[Key.UpArrow].wasPressedThisFrame && _audioIndex > 0) _audioIndex--;
        else if (Keyboard.current[Key.DownArrow].wasPressedThisFrame && _audioIndex < 3) _audioIndex++;

        // print(_audioIndex);

        if (Keyboard.current[Key.Z].wasPressedThisFrame)
        {
            switch (_audioIndex)
            {
                case 0:

                    return;
                case 1:

                    return;
                case 2:

                    _pauseObject.SetActive(true);
                    _audioObject.SetActive(false);
                    _audioIndex = 0;
                    _isAudio = false;

                    return;
            }
        }

        if (Keyboard.current[Key.LeftArrow].wasPressedThisFrame)
        {
            switch (_audioIndex)
            {
                case 0:
                    if (_bgmCount > 0) _bgmCount -= 5;
                    _AA.ChangeVolume(_bgmCount, "BGM");
                    return;
                case 1:
                    if (_seCount > 0) _seCount -= 5;
                    _AA.ChangeVolume(_seCount, "SE");

                    return;
            }
        }
        if (Keyboard.current[Key.RightArrow].wasPressedThisFrame)
        {
            switch (_audioIndex)
            {
                case 0:
                    if (_bgmCount < 100) _bgmCount += 5;
                    _AA.ChangeVolume(_bgmCount, "BGM");
                    return;
                case 1:
                    if (_seCount < 100) _seCount += 5;
                    _AA.ChangeVolume(_seCount, "SE");

                    return;
            }
        }

        if (Keyboard.current[Key.X].wasPressedThisFrame)
        {
            _isAudio = false;
            _pauseObject.SetActive(true);
            _audioObject.SetActive(false);

        }
    }

    void ChangeText()
    {
        _bgmText.text = "BGM " + _bgmCount + "%";
        _seText.text = "SE " + _seCount + "%";
    }

    public void ToAudioSeting()
    {

        _pauseCanvasObject.SetActive(true);
        _pauseObject.SetActive(false);
        _audioObject.SetActive(true);
        Time.timeScale = 0.0f;
        _isAudio = true;
        _isPause = true;
        _pauseIndex = 0;
        _audioIndex = 0;
    }
}
