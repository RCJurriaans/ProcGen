using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {

	public float force;

	public Transform rotatePoint;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rb.AddRelativeForce (Vector3.up * (rb.mass * Mathf.Abs (Physics.gravity.y)));

		rb.AddForce(0, Input.GetAxis("Vertical") * 10, 0);
		Vector3 horizontalForce = transform.right*Input.GetAxis ("Horizontal");
		rb.AddForceAtPosition(horizontalForce, rotatePoint.position);

		if (Input.GetKey ("space")) {
			Vector3 forwardForce = transform.forward*10;
			rb.AddForce(forwardForce);
		}

	}
}
