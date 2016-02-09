using UnityEngine;
using System.Linq;

namespace EvoMotion2D
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
			if (TotalMassLimit - totalMass() < MinMass)
				return;

            var mass = Random.Range(MinMass, MaxMass);
			CellFactory.Spawn (SpawnedItem, gameObject, mass);
		}

		float totalMass ()
		{
            var sum = 0f;
            foreach (Transform t in transform)
            {
                sum += t.GetComponent<Rigidbody2D>().mass;
            }
            return sum;
		}
	}
}