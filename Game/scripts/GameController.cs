using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public AudioSource switch_sound = null;

	//Magnet Objects
	public GameObject green;
	public GameObject purple;
	public GameObject red;
	public GameObject blue;

	//Variables for the in game texts
    public TextMesh redPlaceText;
    public TextMesh purplePlaceText;
    public TextMesh greenPlaceText;
    public TextMesh bluePlaceText;
	public TextMesh wayToGoText;

    //Variables for win checks
    private bool flagRed;
	private bool flagBlue;
	private bool flagGreen;
	private bool flagPurple;

	//Variable for counting number of players across finish line
	private int count;	

	// List the winners in order 
	// rankings[0] should be first place
	// rankings[3] should be last place
	private IList<GameObject> rankings;
	private IList<string> rankStrings = new List<string> { "First Place - ", "Second Place - ", "Third Place - ", "Fourth Place - " };

    void Start()
    {
		Time.timeScale = 1.5f;
        count = 0;
		rankings = new List<GameObject> ();
    }

    
    void Update()
    {
		/*
		 * * Counting position of each racer in the end
		 */
		if ((red.GetComponent<Magnet>().done == true && !flagRed)|| (count == 3 && !flagRed))
		{
			Debug.Log("managing laps" + count);

			printSuccess (red);
			rankings.Add (red);
            count += 1;
			flagRed = true;
        }
		if ((purple.GetComponent<Magnet>().done == true && !flagPurple)|| (count == 3 && !flagPurple))
        {
			printSuccess (purple);
            Debug.Log("managing laps" + count);
			rankings.Add (purple);
            count += 1;
			flagPurple = true;
        }
		if ((green.GetComponent<Magnet>().done == true && !flagGreen) || (count == 3 && !flagGreen))
        {
			printSuccess (green);
			rankings.Add (green);

            count += 1;
			flagGreen = true;
        }
		if ((blue.GetComponent<Magnet>().done == true && !flagBlue) || (count == 3 && !flagBlue))
        {
			printSuccess (blue);
			rankings.Add (blue);

            count += 1;
			flagBlue = true;
        }

		/*
		 * Printing the results on screen when race is finished
		 */
        if (count == 4)
        {
			for(int i = 0; i < 4; i++) {
				GameObject item = rankings [i];
				string message = rankStrings [i] + item.name;
				switch (item.name) {
					case "BLUE":
						bluePlaceText.text = message;
						break;
					case "RED":
						redPlaceText.text = message;
						break;
					case "GREEN":
						greenPlaceText.text = message;
						break;
					default:
						purplePlaceText.text = message;
						break;	
				}
			}
				
            Time.timeScale = 0;
        }
    }

	void printSuccess(GameObject color) {
        wayToGoText.text = string.Format("Way to go " + color.name);

		switch (color.name) {
		case "BLUE":
			wayToGoText.color = Color.blue;
			break;
		case "RED":
			wayToGoText.color = Color.red;
			break;
		case "GREEN":
			wayToGoText.color = Color.green;
			break;
		default:
			wayToGoText.color = Color.magenta;
			break;
		}
	}
}

