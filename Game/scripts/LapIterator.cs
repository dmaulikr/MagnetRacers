using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class LapIterator : MonoBehaviour
{
	// Save data file location
	public string saveLapDataFileType;
	private const string SAVE_FOLDER = "/Users/Ben/Desktop/Spring 2017/Game Artificial Intelligence/Project/Magnet_Racer_AI_Agent/Magnet_Racer_Unity/Assets/Game/data/";
	private string fileName;

    public GateTrigger currentLapTrigger;
	public TextMesh lapCountText;
    public GameObject endPoint;
	public int winLapCount;

	GateTrigger nextLapTrigger;
	GateTrigger winning;

    int _lap;

	DateTime lastLapTime;
	// Parents magnet script
	private Magnet magnetScript;

    // Use this for initialization
    void Start()
    {
        _lap = 0;
		lastLapTime = System.DateTime.Now;
		magnetScript = GetComponent<Magnet> ();
		SetNextTrigger(currentLapTrigger);
        UpdateText();

		fileName = SAVE_FOLDER + saveLapDataFileType + lastLapTime.GetHashCode() + ".txt";
    }


    // Update lap counter text
    void UpdateText()
    {
		if (lapCountText)
        {
			lapCountText.text = string.Format("{0}", _lap);
        }
    }

	// Check for a winner
    public void winner()
    {
        if (_lap == winLapCount)
        {
			Rigidbody2D rigid = GetComponent<Rigidbody2D> ();
			rigid.transform.position = endPoint.transform.position;
			rigid.transform.rotation = Quaternion.identity;
			rigid.constraints = RigidbodyConstraints2D.FreezeAll;
			magnetScript.setDone ();
        }
    }

    // when lap trigger is entered
	public void OnLapTrigger(GateTrigger trigger)
    {
        if (trigger == nextLapTrigger)
        {
			if (currentLapTrigger == nextLapTrigger)
            {
                _lap++;
				saveLapTimeData ();
                UpdateText();
                winner();
            }
            SetNextTrigger(nextLapTrigger);
        }
    }

    //adding the next lap trigger counter into
	void SetNextTrigger(GateTrigger trigger)
    {
        nextLapTrigger = trigger.next;
     
        //Debug.Log("next=" + nextLapTrigger.gameObject.name);
    }


	/*
	 * Allow to analyze effectiveness by saving a magnets lap times to a text file.
	 * 
	 * Allow learning agents to increase their age.
	 */
	private void saveLapTimeData() {
		if (saveLapDataFileType != null) {
			// TODO: fix
			DateTime lapTime = System.DateTime.Now;
			TimeSpan lapse = lapTime - lastLapTime;
			lastLapTime = lapTime;
			// Use for time System.DateTime.Now;
			StreamWriter file = new StreamWriter (fileName, true);
			file.WriteLine ("Lap : " + _lap + " Time: " + lapse.TotalSeconds);
			file.Close ();
		}

		try{
			//TODO: Put any other learning agents aging function calls here in the future
			QLearningAgent agent = GetComponent<QLearningAgent>();
			if(agent != null) {
				agent.increaseMaturity ();
			}
		} catch(Exception e) {
			// Do nothing if no learning agent is present
		}
	}
}