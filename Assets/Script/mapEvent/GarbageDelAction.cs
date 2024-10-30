using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageDelAction : MonoBehaviour
{
    // Start is called before the first frame update

    private int _meatBurnt = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MeatBurnt")
        {
            Destroy(other.gameObject);
            _meatBurnt++;
        }

    }

}
