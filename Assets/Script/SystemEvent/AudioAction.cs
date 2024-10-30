using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioAction : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioClip _bgm;
    public AudioClip _punch;
    public AudioClip _punchHit;
    public AudioClip _playerDamage;
    public AudioClip _cashOut;
    public AudioClip _fireKeep;
    public AudioClip _die;
    List<AudioSource> _audioSourceList = new List<AudioSource>();

    private float _bgmVolume;
    private float _seVolume;
    private static AudioAction instance = null;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            _audioSourceList.Add(audio);
        }
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
        PlayClip(0, "BGM", true);
        _audioSourceList[0].volume = 0.5f;
        for (int i = 1; i < 4; i++)
        {
            _audioSourceList[i].volume = 0.5f;
        }


    }

    // Update is called once per frame
    public void PlayClip(int index, string audioName, bool isLoop)
    {
        AudioClip clip = GetAudioClip(audioName);
        if (clip != null)
        {
            AudioSource audio = _audioSourceList[index];
            audio.clip = clip;
            audio.loop = isLoop;
            audio.Play();
        }
    }
    public void StopClip(int index, string audioName)
    {
        AudioClip clip = GetAudioClip(audioName);
        if (clip != null)
        {
            AudioSource audio = _audioSourceList[index];
            if (audio.clip.name == clip.name)
                audio.Stop();
        }
    }

    public AudioClip GetAudioClip(string AudioName)
    {
        switch (AudioName)
        {
            case "BGM":
                return _bgm;
            case "PunchSE":
                return _punch;
            case "PunchHitSE":
                return _punchHit;
            case "PlayerDagameSE":
                return _playerDamage;
            case "CashOutSE":
                return _cashOut;
            case "FireKeepSE":
                return _fireKeep;
            case "DieSE":
                return _die;

        }
        return null;
    }

    public void ChangeVolume(int volume, string typevolume)
    {
        if (typevolume == "BGM")
        {
            _bgmVolume = (float)volume / 100;
            _audioSourceList[0].volume = _bgmVolume;
        }
        if (typevolume == "SE")
        {
            _seVolume = (float)volume / 100;
            for (int i = 1; i < 4; i++)
            {
                _audioSourceList[i].volume = _seVolume;
            }
        }

    }
}
