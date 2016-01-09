using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class Spawner : MonoBehaviour
	{

		public float Cooldown;
		public GameObject SpawnedItem;
		public float MinMass, MaxMass;
		public float TotalMassLimit;

		// Use this for initialization
		void Start ()
		{
			InvokeRepeating ("spawn", .1f, Cooldown);
		}

		void spawn ()
		{
			//Debug.Log ("total mass: " + totalMass ());
			if (TotalMassLimit - totalMass () < MinMass)
				return;

			var mass = Random.value * (MaxMass - MinMass) + MinMass;

			CellFactory.Spawn (SpawnedItem, gameObject, mass);
		}

		float totalMass ()
		{		
			var cells = GameObject.FindGameObjectsWithTag ("Cell");
			float mass = 0;
			foreach (var cell in cells) {
				var rb = cell.GetComponent<Rigidbody2D> ();
				if (rb)
					mass += rb.mass;
			}
			return mass;
		}
	}
}