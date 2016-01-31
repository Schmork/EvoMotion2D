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
            return CellFactory.Cells.Sum(cell => cell.GetComponent<Rigidbody2D>().mass);
		}
	}
}