using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QState{
    // Use to determine Quadrant!
    private static float y1_slope = 0.35f;
	private static float y_const = 0.1f;
	private static float y2_slope = -0.65f;

	// -1 or 1
	private int thisCharge;

	// 0, 1, 2, 3
	private int thisQuadrant;

	// 1, 2 (Currently: 1 = near, 2 = far and fulcrum is far >= 3)
	private int nearestOpponentDistance;

	// Which direction would it push?
	private int nearestOpponentDirection;

	// -1 or 1
	private int nearestOpponentCharge;

	public QState() {
		thisCharge = 0;
		thisQuadrant = 0;
		nearestOpponentDistance = 0;
		nearestOpponentDirection = 0;
		nearestOpponentCharge = 0;
	}

	public QState(int charge, int quadrant, int distance, int direction, int otherCharge) {
		thisCharge = charge;
		thisQuadrant = quadrant;
		nearestOpponentDistance = distance;
		nearestOpponentDirection = direction;
		nearestOpponentCharge = otherCharge;
	}

	/*
	 * OVERRIDE OPERATIONS vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
	 */

	// Override equals for comparisons we want.
	public override bool Equals(object obj)
	{
		var item = obj as QState;

		if (item == null)
		{
			return false;
		}

		return this.GetHashCode() == item.GetHashCode();
	}

	// Override hash code for comparisons
	public override int GetHashCode()
	{
		return thisCharge * 10000 + thisQuadrant * 1000 + nearestOpponentDistance * 100 + 
			nearestOpponentDirection * 10 + nearestOpponentCharge;
	}

	// Need to override to string to make it easy to write to a file
	public override string ToString ()
	{
		//charge, quadrant, distance, direction, otherCharge -> this order is read from file
		return "" + thisCharge + ";" + 
			thisQuadrant + ";" + 
			nearestOpponentDistance + ";" +
			nearestOpponentDirection + ";" + 
			nearestOpponentCharge + ";";
	}

	/*
	 * GET OPERATIONS vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
	 */
	public int getQuadrant() {
		return this.thisQuadrant;
	}	

	/*
	 * STATIC OPERATIONS vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
	 */

	// Statically determine the game state given a magnet and its opponenets.
	public static QState getQState(GameObject magnet, GameObject[] opponents) {
		Vector2 position = magnet.GetComponent<Magnet>().getPosition();
		float x = position.x;
		float y = position.y;

		int charge = magnet.GetComponent<Magnet>().getCharge();

		int quadrant = calculateQuadrant(x,y);

		float distances;
		float closest = float.MaxValue;
		int locked = 0;

		for (int i = 0; i < 3; i++)
		{
			distances = Vector2.Distance(magnet.transform.position, opponents[i].transform.position);

			if (distances <= closest)
			{
				closest = distances;
				locked = i;
			}
		}

		int distance = (int) closest;
		if (distance >= 3) {
			distance = 2;
		} else {
			distance = 1;
		}
		// Debug.Log ("Distance to nearest opp: " + distance);

		// simplified directions based on which quadrant the closest opponent is present
		// Think: which way will it push: up, left, right, down?
		int direction = getOpponentDirection(x, y, opponents[locked]);

		int otherCharge = opponents[locked].GetComponent<Magnet>().getCharge();

		return new QState(charge, quadrant, distance, direction, otherCharge);
	}
    

	// Return 0 - 3 based on position in relation to quadrant lines y1 and y2.
	private static int calculateQuadrant(float x, float y) {       
        //If above y1
        if (y > (y1_slope * x) + y_const)
        {
            //If above y2
            if (y > (y2_slope * x) + y_const)
            {
                //In zone 0 gives 
                return 0;
            }
            //If below y2
            else
            {
                //In zone 1 gives
                return 1;
            }
        }
        //If below y1
        else
        {
            //If above y2
            if (y > (y2_slope * x) + y_const)
            {
                //In zone 3 gives
                return 3;

            }
            //If below y2
            else
            {
                //In zone 2
                return 2;
            }
        }
    }


	// Give a direction for where an opponent is. 
	// above = 0, left = 1, below = 2, right = 3
	// This way we have an idea of which way they will push us
	private static int getOpponentDirection(float x, float y, GameObject opponent) {
		float xOpponent = opponent.transform.position.x;
		float yOpponent = opponent.transform.position.y;
		float slope = (yOpponent - y) / (xOpponent - x);

		if (xOpponent > x) 
		{
			// above: slope made is steeper than 45 degree line
			if (slope > 0.5f)
				return 0;
			// right: slope made is between the -45 and 45 degree lines
			if (slope > -0.5f)
				return 3;
			// below: below -45 degree line
			return 2;
		} else {
			// above: slope is more negative than the -45 degree line
			if (slope < -0.5f)
				return 0;
			// left slope is more shallow than the 45 degree line and already
			// must be below the -45 degree line
			if (slope < 0.5f)
				return 1;
			// below
			return 2;
		}
	}
}
