using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    public UIAction _UI;
    private void Start()
    {
        _UI = GameObject.Find("UI").GetComponent<UIAction>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "CarryCheck" && gameObject.name == "Start")
        {
            UIAction._playerSroce = 0;
            UIAction._playerTime = 0.0f;
            SceneManager.LoadScene(sceneName: "MainKitchen");

        }

    }
}
