using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour
{
    public GameObject pole1;
    public GameObject pole2;
    public GameObject pole3;
    public GameObject pole4;
    public GameObject opponent1;
    public GameObject opponent2;
    public GameObject opponent3;

    // local charges to hold value of each individual racer charge changes
    public int charge;

    public float repelling = 1000;
    public float repelling2 = 500;

    public bool done;

    public Sprite spritepositive;
    public Sprite spritenegative;
    SpriteRenderer spriterender;

    Rigidbody2D myrigidbody;

    void Start()
    {
        
        spriterender = GetComponent<SpriteRenderer>();
		updateSprite();

        myrigidbody = GetComponent<Rigidbody2D>();
    }

	public int getCharge() {
		return this.charge;
	}

	public void setDone() {
		this.done = true;
		this.charge = 0;
	}

	public void makeMove() {
		this.charge *= -1;
	}

	public GameObject[] getOpponents() {
		GameObject[] opponents = new GameObject[3];
		opponents [0] = opponent1;
		opponents [1] = opponent2;
		opponents [2] = opponent3;
		return opponents;
	}

	public Vector2 getPosition() {
		return myrigidbody.position;
	}

    private void updateSprite()
    {
        //updating sprite of this charge
		if (charge == 1)
            spriterender.sprite = spritepositive;
		else if (charge == -1)
            spriterender.sprite = spritenegative;
    }
   
	/**
	 * Physics for the magnet
	 */
    void FixedUpdate()
    {
		updateSprite();

		Vector3 totalForce = calculateForce ();
		myrigidbody.AddForce (totalForce);
    }

	public Vector3 calculateForce() {
		Vector3 forceSum = new Vector3 ();
		/*
		 * Permanent magnet 1
		 */
		Vector3 direction1 = (transform.position - pole1.transform.position).normalized;
		float traveldistance1 = Vector3.Distance(transform.position, pole1.transform.position);

		float force1 = repelling / traveldistance1;

		Vector3 newforce1 = direction1 * force1 * charge * -1 * Time.smoothDeltaTime;

		if (traveldistance1 <= 14)
			forceSum += newforce1;

		/*
		 * Permanent magnet 2
		 */
		Vector3 direction2 = (transform.position - pole2.transform.position).normalized;
		float traveldistance2 = Vector3.Distance(transform.position, pole2.transform.position);

		float force2 = repelling / traveldistance2;

		Vector3 newforce2 = direction2 * force2 * charge * 1 * Time.smoothDeltaTime;

		if (traveldistance2 <= 14)
			forceSum += newforce2;

		/*
		 * Permanent magnet 3
		 */
		Vector3 direction3 = (transform.position - pole3.transform.position).normalized;
		float traveldistance3 = Vector3.Distance(transform.position, pole3.transform.position);

		float force3 = repelling / traveldistance3;

		Vector3 newforce3 = direction3 * force3 * charge * -1 * Time.smoothDeltaTime;

		if (traveldistance3 <= 14)
			forceSum += newforce3;


		/*
		 * Permanent magnet 4
		 */
		Vector3 direction4 = (transform.position - pole4.transform.position).normalized;
		float traveldistance4 = Vector3.Distance(transform.position, pole4.transform.position);

		float force4 = repelling / traveldistance4;

		Vector3 newforce4 = direction4 * force4 * charge * 1 * Time.smoothDeltaTime;

		if (traveldistance4 <= 14)
			forceSum += newforce4;

		/*
		 * Racer1
		 */
		Vector3 direction5 = (transform.position - opponent1.transform.position).normalized;
		float traveldistance5 = Vector3.Distance(transform.position, opponent1.transform.position);

		float force5 = repelling2 / traveldistance5;

		Vector3 newforce5 = direction5 * force5 * charge * opponent1.GetComponent<Magnet>().getCharge() * Time.smoothDeltaTime;

		if (traveldistance5 <= 4)
			forceSum += newforce5;

		/*
		 * Racer2
		 */
		Vector3 direction6 = (transform.position - opponent2.transform.position).normalized;
		float traveldistance6 = Vector3.Distance(transform.position, opponent2.transform.position);

		float force6 = repelling2 / traveldistance6;

		Vector3 newforce6 = direction6 * force6 * charge * opponent2.GetComponent<Magnet>().getCharge() * Time.smoothDeltaTime;

		if (traveldistance6 <= 4)
			forceSum += newforce6;

		/*
		 * Racer3
		 */
		Vector3 direction7 = (transform.position - opponent3.transform.position).normalized;
		float traveldistance7 = Vector3.Distance(transform.position, opponent3.transform.position);

		float force7 = repelling2 / traveldistance7;

		Vector3 newforce7 = direction7 * force7 * charge * opponent3.GetComponent<Magnet>().getCharge() * Time.smoothDeltaTime;

		if (traveldistance7 <= 4)
			forceSum += newforce7;


		return forceSum;
	}
}


   

        