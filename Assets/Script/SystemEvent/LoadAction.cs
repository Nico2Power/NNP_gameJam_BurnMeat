using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(sceneName: "StartKitchen");

    }

    // Update is called once per frame
    void Update()
    {

    }
}
