using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToAudio : MonoBehaviour
{
    // Start is called before the first frame update
    public PauseAction _PA;
    private void Start()
    {
        _PA = GameObject.Find("PauseControl").GetComponent<PauseAction>();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "CarryCheck" && gameObject.name == "Audio")
        {
            _PA.ToAudioSeting();

        }
    }
}
