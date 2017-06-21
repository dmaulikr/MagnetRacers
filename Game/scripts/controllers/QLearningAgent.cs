using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QLearningAgent : MonoBehaviour {
	// Variables for learning 
	private const float LEARNING_RATE = 0.05f;
	private const float LEARNING_RATE_SLOW = 0.03f;
	private const float DISCOUNT_FACTOR = 0.2f;
	private const int LEARNING_MATURITY_CAP = 2;

	// Reward types
	public enum RewardType {GATE, MOTION, FORCE};
	public RewardType rewardType;

	// Location of q map text file
	public string fileNameBase;
	private const string SAVE_FOLDER = "/Users/Ben/Desktop/Spring 2017/Game Artificial Intelligence/Project/Magnet_Racer_AI_Agent/Magnet_Racer_Unity/Assets/Game/data/";
	public string qMapFileLocation;

	// Game objects to use
	private GameObject magnet;
	private GameObject[] opponents;

	// Parent's magnet script
	private Magnet magnetScript;

	// QMap for stored q values
	private QMap qMap;

	private QState currentState;
	// Values to save for calculations
	private Vector2 lastPosition;
	private QState lastState;
	private bool lastAction;

	void Start() {
		// Objects
		magnet = gameObject;
		magnetScript = magnet.GetComponent<Magnet> ();
		opponents = magnetScript.getOpponents ();

		// Memory
		currentState = null;
		lastState = null;
		lastAction = false;

		// Save Data
		if (qMapFileLocation == null || qMapFileLocation == "") {
			qMapFileLocation = SAVE_FOLDER + fileNameBase + System.DateTime.Now.GetHashCode() + ".txt";
		}
		Debug.Log("Q Data Location: " + qMapFileLocation);
			
		// Q Map
		qMap = new QMap (qMapFileLocation);
	}

	// If deciding to toggle, call magnet scripts toggle functionality.
	void FixedUpdate () {
		// Get current state
		currentState = QState.getQState (magnet, opponents);

		// Don't continue after magnet is done.
		if (magnetScript.getCharge () != 0) {
			// Update Q value
			if (lastState != null) {
				updateQValue();
			}

			// Choose an action
			bool action = choseAction();

			// Toggle if needed
			if (action) {
				magnetScript.makeMove ();
			}

			// Update memory
			lastState = currentState;
			lastAction = action;
			lastPosition = magnet.GetComponent<Magnet>().getPosition();
		}
	}

	// Need to increase the maturity for the magnet to start using what it has learned.
	public void increaseMaturity() {
		qMap.increaseMaturity ();
	}

	/*
	 * LEARNING OPERATIONS vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
	 */

	// Based on last state and action
	//  - Find reward
	//  - Calculate a new q value
	// Update the q values for last state with the new value replacing its old value
	private void updateQValue() {
		int reward = calculateReward_Forces ();
		int maxFuture = bestCurrentQValue ();
		int[] oldValues = qMap.getValues (lastState);
		int[] newValues = new int[2];

		if (lastAction) {
			newValues [0] = calculateQValue (reward, maxFuture);
			newValues [1] = oldValues [1];
		} else {
			newValues [1] = calculateQValue (reward, maxFuture);
			newValues [0] = oldValues [0];
		}

		//Debug.Log("Old Values: " + oldValues[0] + "," + oldValues[1] + 
		//	"\nReward: " + reward +
		//	"\nMax Future: " + maxFuture +
		//	"\nNew Values Q: " + newValues[0] + "," + newValues[1]);

		qMap.updateQValues(lastState, newValues);
	}

	// Based on the action taken from the last state and the current state that resulted
	// a reward will be given
	// Use position changes (reward passing through quadrant gates)
	//
	/// <summary>
	/// Conclusion:
	/// Not effective because a magnet could go through gates easily with the wrong charge do to momentum
	/// </summary>
	/// <returns>The reward gates.</returns>
	private int calculateReward_Gates() {
		if (lastState.getQuadrant () < currentState.getQuadrant () ||
			(lastState.getQuadrant() == 3 && currentState.getQuadrant() == 0)) {
			// Debug.Log ("reward: 25");
			return 90;
		}

		return 0;
	}

	// Based on the action taken from the last state and the current state that resulted
	// a reward will be given
	// Use position changes (reward counter clockwise)
	/// <summary>
	/// Conclusion: 
	/// Not effective for the same reason the gates do not work. The magnet could easily go the right 
	/// direction with the wrong charge just because of their momentum
	/// </summary>
	/// <returns>The reward motion.</returns>
	private int calculateReward_Motion() {
		Vector2 position = magnet.GetComponent<Magnet>().getPosition();

		if (lastState.getQuadrant () < currentState.getQuadrant () ||
		    (lastState.getQuadrant () == 3 && currentState.getQuadrant () == 0)) 
		{
			// Debug.Log ("reward: 25");
			return 100;
		} 
		else if (lastState.getQuadrant() == currentState.getQuadrant()) 
		{
			switch (currentState.getQuadrant ()) {
			case 0:
				if (lastPosition.x > position.x)
					return 50;
				break;
			case 1:
				if (lastPosition.y > position.y)
					return 50;
				break;
			case 2:
				if (lastPosition.x < position.x)
					return 50;
				break;
			case 3: 
				if (lastPosition.y < position.y)
					return 50;
				break;
			default:
				break;
			}
		}

		return 0;
	}

	// Calculate a reward based on the new force on the magnet
	// This way the momentum shouldn't matter
	private int calculateReward_Forces() {
		if (lastState.getQuadrant() == currentState.getQuadrant()) 
		{
			Vector4 force = magnetScript.calculateForce ();

			switch (currentState.getQuadrant ()) {
			case 0:
				if (force.x < 0)
					return 50;
				break;
			case 1:
				if (force.y < 0)
					return 50;
				break;
			case 2:
				if (force.x > 0)
					return 50;
				break;
			case 3: 
				if (force.y > 0)
					return 50;
				break;
			default:
				break;
			}
		}

		return 0;
	}

	// Calculate a new Q Value
	// Using q learning equation
	// Q(s,a) = Q(s,a) + learning_rate(r + discout_factor * Max(Q(s’,a)) - Q(s,a))
	private int calculateQValue(int reward, int maxCurrenValue) {
		// TODO: implement
		int[] oldOptions = qMap.getValues(lastState);
		int oldValue = lastAction ? oldOptions [0] : oldOptions [1];
		float learningRate = qMap.getMaturity () > LEARNING_MATURITY_CAP ? LEARNING_RATE_SLOW : LEARNING_RATE;


		float weight = (reward + DISCOUNT_FACTOR * maxCurrenValue - oldValue);
		int newValue = (int) (oldValue + learningRate * weight);

		if (reward > 0) {
			// Debug.Log("State: " + lastState.ToString() + "\nAction: " + lastAction + "\nOld Q: " + oldValue + "\nNew Q: " + newValue); 
		}

		return newValue;
	}

	// Find the max q value available at the current state
	private int bestCurrentQValue() {
		int[] values = qMap.getValues (currentState);
		int max = int.MinValue;
		foreach (int i in values) {
			if (i > max) {
				max = i;
			}
		}
		return max;
	}

	// Choose an action to take
	// In early stages, it should try random moves
	// As it has learned more it is considered mature and will try other options less often.
	private bool choseAction() {
		float randomNumber = UnityEngine.Random.value;

		if (qMap.getMaturity () < LEARNING_MATURITY_CAP) {
			// Explore (act as random agent)
			return randomNumber < 0.02f;
		} else {
			// Probability to explore is lower, eventually 0
			float probability = 0.02f - 0.002f * (qMap.getMaturity() / LEARNING_MATURITY_CAP); 
			if (probability < 0.005f)
				probability = 0.005f;
			if (randomNumber < probability) {
				float randomAction = UnityEngine.Random.value;
				return randomAction < 0.5f; // Don't toggle to often or it will be stagnant
			} else {
				int[] values = qMap.getValues (currentState);
				// If the action for toggle (position 0) is greater than the action for hold
				// then return true for toggle.

				return values [0] > values [1];
			}
		}
	}
}
