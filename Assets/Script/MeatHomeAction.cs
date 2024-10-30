using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatHomeAction : MonoBehaviour
{
    public GameObject _newMeat;
    public UIAction _UI;

    private int _newMeatNumber;
    private int _newMeatWave;
    void Start()
    {
        _newMeatNumber = 1;
        _newMeatWave = 0;
        CreateNewMeat();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetTimerToNewMeat();
    }

    public void CreateNewMeat()
    {
        for (int i = _newMeatNumber; i > 0; i--)
        {
            Vector3 newMeatPosition = GetRandomPostion();
            Instantiate(_newMeat, newMeatPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPostion()
    {
        //  Vector3(11.5,5.4) Vector3(11.5,-5.4)
        // Vector3(20.58,-5.4,0) Vector3(20.58,5.4,0)
        int randomRL = UnityEngine.Random.Range(0, 2);
        float randomX = UnityEngine.Random.Range(11.5f, 20.58f);
        float randomY = UnityEngine.Random.Range(-5.4f, 5.4f);

        if (randomRL == 1) randomX = -randomX;

        Vector3 newMeatPosition = new Vector3(randomX, randomY, 0);
        return newMeatPosition;
    }

    private void GetTimerToNewMeat()
    {
        int minuts = (int)Time.timeSinceLevelLoad / 60;
        int second = (int)Time.timeSinceLevelLoad;

        if (minuts + 1 > _newMeatNumber) { _newMeatNumber++; }
        if (second / 10 > _newMeatWave)
        {
            _newMeatWave++;
            CreateNewMeat();
        }

    }
}
