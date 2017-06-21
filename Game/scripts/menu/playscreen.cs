using UnityEngine;
using System.Collections;

public class PlayScreen : MonoBehaviour {
    float delay = 3;
    public AudioSource start_sound;

    // Use this for initialization
    void Update () {
        delay -= Time.deltaTime;
        if (delay <= 0)
        {
            Application.LoadLevel(3);
            start_sound.Play();

        }
	}
	
	
}
