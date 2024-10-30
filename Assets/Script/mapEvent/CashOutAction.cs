using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashOutAction : MonoBehaviour
{

    [SerializeField] public UIAction _UI;
    public AudioAction _AA;

    private void Start()
    {
        _AA = GameObject.Find("AudioControl").GetComponent<AudioAction>();

    }
    // Start is called before the first frame update


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "MeatFood")
        {
            Destroy(other.gameObject);
            _AA.PlayClip(3, "CashOutSE", false);
            _UI.SrocePlus();

        }

    }

}
