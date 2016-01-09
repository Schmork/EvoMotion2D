using UnityEngine;
using System.Collections;

public class StayInBounds : MonoBehaviour {

	[Range(0, 500)]
	public float Width, Height;
		
	// Update is called once per frame
	void Update () {

		float dist = Vector2.Distance (transform.position, Vector2.zero);
		if (dist > 30) {
			Destroy(gameObject);
			/*var dir = transform.position.normalized;
			var mag = transform.position.magnitude;
			GetComponent<Rigidbody2D> ().AddForce (-dir * mag);*/
		}
	}
}
