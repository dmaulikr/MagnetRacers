using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour {

	private Magnet magnetScript;
	public KeyCode option;

    void Start()
    {
		magnetScript = GetComponent<Magnet>();
    }

    // Update is called once per frame
    void Update () {
		
		if(option != null && Input.GetKeyDown(option))
        {
            magnetScript.makeMove();
        }
	}
}
