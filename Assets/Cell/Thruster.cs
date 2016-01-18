using UnityEngine;
using EvoMotion2D.Modules;
using EvoMotion2D.Parameters;

namespace EvoMotion2D.Cell
{
	public class Thruster : MonoBehaviour
	{
        public float UsageFee;
		public GameObject ThrustObject;
		CellHandler ch;

        public UnsignedMutateableParameter ThrustToMassRatio = UnsignedMutateableParameter.Random();

        // Use this for initialization
        void Awake ()
		{
			ch = GetComponent<CellHandler> ();
            while (ThrustToMassRatio.Value > 0.1) ThrustToMassRatio.Value *= .999f;
        }

		public void Thrust (Vector2 dir)
		{
			var mass = ch.Mass * ThrustToMassRatio.Value;
			if (mass < Shrinker.StaticMinMass)
				return;

            ch.Mass -= UsageFee;
            dir = dir.normalized;
            CellFactory.Thrust (gameObject, mass, dir);
		}
	}
}