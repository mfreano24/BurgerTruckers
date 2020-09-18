using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour
{
    /// <summary>
    /// This is just a general death class that can be put on any object which can trigger death, so both the truck and the player.s
    /// </summary>
    public GameManager gm;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Truck"))
        {
            StartCoroutine(gm.LoseGame());
        }
    }
}
