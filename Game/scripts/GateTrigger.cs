using UnityEngine;
using System.Collections;

public class GateTrigger : MonoBehaviour
{

    // next trigger in the lap
    public GateTrigger next;

    // when an object enters this trigger
    void OnTriggerEnter2D(Collider2D other)
    {
      
        LapIterator carLapCounter = other.gameObject.GetComponent<LapIterator>();
        
        if (carLapCounter)
        {
            // Debug.Log("lap trigger " + gameObject.name);
            carLapCounter.OnLapTrigger(this);
        }
    }
}