using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour
{
	public GameObject target, candidate;
	public int Index;
	[Range(0, 0.5f)]
	public float
		ScanChance;
	[Range(0.1f, 500)]
	public float
		ScanMaxRange;
	[Range(0.1f, 10f)]
	public float
		PreyFactor;

	public void SwitchToCandidate ()
	{
		target = candidate;
		candidate = null;
	}

	void Update ()
	{
		var myMass = GetComponentInParent<Rigidbody2D> ().mass;

		if (Random.value < ScanChance) {

			var dir = Random.insideUnitCircle.normalized;
			var start = (Vector2)transform.position;

			RaycastHit2D hit = Physics2D.Raycast (start, dir, ScanMaxRange);
			if (hit.collider != null && hit.collider.tag == "Cell") {
				candidate = hit.collider.gameObject;

				/*if (name == candidate.name)
					return;
						
				var yourMass = candidate.GetComponent<Rigidbody2D> ().mass;
				if (yourMass < myMass && yourMass > myMass / PreyFactor)
					return;

				if (target == null) {
					target = candidate;
					return;
				} 

				if (getScore(candidate) > getScore(target))
					target = candidate;*/
			}
		}

		/*if (target != null) {
			if (name == target.name) {		// TODO: make this dirty fix obsolete
				SwitchToCandidate();
			} else if (myMass < target.GetComponent<Rigidbody2D> ().mass
				&& Vector2.Distance (transform.position, target.transform.position) > 0.9f) {
				SwitchToCandidate();
			} else if (myMass < target.GetComponent<Rigidbody2D> ().mass
				&& name.Contains(target.name))
				SwitchToCandidate();
		}

		if (object.Equals (target, null))
			SwitchToCandidate();*/
	}

	/*float getScore (GameObject cell)
	{
		
		var dist = Vector2.Distance (transform.position, cell.transform.position);		
		var score = cell.GetComponent<Rigidbody2D> ().mass / (dist * dist);
		return score;
	}*/
}
