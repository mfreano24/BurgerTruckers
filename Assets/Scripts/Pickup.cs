using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    float yPos;
    int rando;
    void Start()
    {
        rando = Random.Range(1, 11);
        yPos = transform.position.y;
        //transform.Rotate(30, 0, 0);
    }

    void Update()
    {
        if (this.gameObject.name != "knife")
        {
            transform.Rotate(0.0f, 0.5f, 0.0f);
        }
        
        yPos = 0.001f *  Mathf.Sin(2.0f*Time.time + rando);
        transform.position += new Vector3(0, yPos, 0);
    }
}
