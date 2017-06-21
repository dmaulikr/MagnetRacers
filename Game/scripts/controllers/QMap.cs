using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class QMap {
	// Private Data
	private int learningLaps;
	private string fileName;
	private Dictionary<QState, int[]> qMap;

	// Number of values in a line
	private static int VALUES_PER_LINE = 7;

	// Use this for initialization
	public QMap(string file) {
		learningLaps = 0;
		qMap = new Dictionary<QState, int[]> ();
		this.fileName = file;
		readDataFromFile (fileName);
	}

	// Get the values associated with the actions taken from a state
	// int[0] will be the toggle action
	// int[1] will be the not toggle choice.
	public int[] getValues(QState state) {
		if (qMap.ContainsKey(state)) {
			return qMap [state];
		} else {
			return new int[2] {0, 0};
		}
	}

	// Allow to update q values for a state
	// Add a new key value pair if there is not one for that state.
	public void updateQValues(QState state, int[] values) {
		qMap [state] = values;
	}

	public int getMaturity() {
		return this.learningLaps;
	}

	public void increaseMaturity() {
		this.learningLaps++;
		Debug.Log ("Increase Maturity: " + learningLaps);
		writeDataToFile (fileName);
	}

	/*
	 * FILE OPERATIONS vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
	 */


	// read the stored q map.
	public void readDataFromFile(string fileName) {
		qMap.Clear ();
		string line;
		string[] values = new string[VALUES_PER_LINE];
		int[] intValues = new int[VALUES_PER_LINE];
		QState state;
	
		try {
			// Read the file and display it line by line.
			StreamReader file = new StreamReader(fileName);

			// Get first line to see number of laps trained for in file
			line = file.ReadLine();
			learningLaps = int.Parse (line);

			while((line = file.ReadLine()) != null)
			{
				Debug.Log("Read line: " + line);
				values = line.Split (';');
				for (int i = 0; i < values.Length; i++) {
					intValues[i] = int.Parse (values[i]);
				}
				state = new QState (intValues [0], intValues [1], intValues [2], intValues [3], intValues [4]);
				int[] qValues = new int[2];
				qValues [0] = intValues [5];
				qValues [1] = intValues [6];

				qMap.Add (state, qValues);
			}

			file.Close();
		} catch(Exception e) {
			Debug.Log("Q Data file does not exist yet. Will create when saving.");
		}
	}

	// Update the stored q map.
	public void writeDataToFile(string fileName) {
		int[] values = new int[2];
		string line;

		StreamWriter file = new StreamWriter (fileName, false);
		{
			file.WriteLine (learningLaps.ToString());
			foreach( KeyValuePair<QState, int[]> kvp in qMap )
			{
				values = kvp.Value;
				line = kvp.Key.ToString () + values [0].ToString () + ";" + values [1].ToString ();

				file.WriteLine(line);
			}
		}

		file.Close ();
	}
}
