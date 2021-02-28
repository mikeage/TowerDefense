using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FlappyFirstPersonControl : MonoBehaviour {

	public float flapForce = 10f;

	private bool hitFlap = false;

	public float strafeForce = 3f;
	public float forwardForce = 3f;
	public float turnForce = 3f;


	private Rigidbody rb;

	private Camera myCam;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Player2Flap")) {
			hitFlap = true;
		}
	}

	void FixedUpdate() {
		if(hitFlap) {
			rb.AddForce(Vector3.up * flapForce, ForceMode.Impulse);
			hitFlap = false;
		}
		rb.AddRelativeForce(new Vector3(Input.GetAxis("Player2LeftRight") * strafeForce, 0f, Input.GetAxis("Player2ForwardBack") * forwardForce));
		rb.AddTorque(Vector3.up * Input.GetAxis("Player2Turn") *  turnForce * Time.fixedDeltaTime);
	}

}  
