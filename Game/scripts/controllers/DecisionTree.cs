using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour {

	private Magnet magnetScript;
    private int velocityFixCounter;


	private float y1_slope = 0.30f;
	private float y_const = 1.25f;
	private float y2_slope = -0.60f;

	void Start() {
		magnetScript = GetComponent<Magnet> ();
	}

    //Returns true if you want to toggle
    void FixedUpdate () {

		float x = magnetScript.gameObject.transform.position.x;
		float y = magnetScript.gameObject.transform.position.y;

                
        GameObject[] opponents = {magnetScript.opponent1, magnetScript.opponent2, magnetScript.opponent3 };

        //calculate closest opponent
        float distances, closest = float.MaxValue;
        int locked = 0;
        for (int i = 0; i < 3; i++)
        {
            distances = Vector2.Distance(magnetScript.gameObject.transform.position, opponents[i].transform.position);

            if (closest <= distances)
            {
                closest = distances;
                locked = i;
            }
        }


        //decision tree start
        //If above y1
        if (magnetScript.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1f)
        {
            //Debug.Log("flip next to magnet?: " + opponents[locked].name);
            if (closest < 1.0f)
            {
                //Debug.Log("flip next to magnet?: " + opponents[locked].name);
                if (opponents[locked].GetComponent<Magnet>().getCharge() == magnetScript.getCharge())
                    return;
                else
                    if (velocityFixCounter > 100)
                    {
                        magnetScript.makeMove();
                        //Debug.Log("flip next to magnet?: " +opponents[locked].name);
                        velocityFixCounter = 0;
                    }

            }
            else
            {
                if (velocityFixCounter > 100)
                {
                    //Debug.Log("velocity too low: " + magnetScript.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude);
                    magnetScript.makeMove();
                    velocityFixCounter = 0;
                }
            }

        }
        else
        if (y > (y1_slope*x)+y_const)
		{
			//If above y2
			if (y > (y2_slope*x)+y_const)
			{
				//In zone 1
				if (magnetScript.charge == 1) {
                    //do nothing
                    return;
                }
				else 
				{
					//Toggle to positive
					magnetScript.makeMove();
				}
			}
			//If below y2
			else
			{
				//In zone 4
				if (magnetScript.charge == 1) {
					//Toggle to positive
					magnetScript.makeMove();
				}
				else 
				{
                    //do nothing
                    return;  
                }
			}
		}
		//If below y1
		else
		{
			//If above y2
			if (y > (y2_slope*x)+y_const)
			{
				//In zone 2
				if (magnetScript.charge == 1) {
					//Toggle to positive
					magnetScript.makeMove();
				}
				else 
				{
                    // do nothing
                    return;
                }
			}
			//If below y2
			else
			{
				//In zone 3
				if (magnetScript.charge == 1) {
                    //do nothing
                    return;
                }
				else 
				{
					//Toggle to positive
					magnetScript.makeMove();
				}
			}
		}

        velocityFixCounter++;
       

    }
}
