using UnityEngine;
using EvoMotion2D.Modules;

namespace EvoMotion2D.Cell
{
	public class Thruster : MonoBehaviour
	{
        public float UsageFee;
		public GameObject ThrustObject;
		CellHandler ch;

        public UnsignedMutateableFloat ThrustToMassRatio;

        // Use this for initialization
        void Awake ()
		{
			ch = GetComponent<CellHandler> ();
            while (ThrustToMassRatio > 0.1) ThrustToMassRatio *= .999f;
        }

		public void Thrust (Vector2 dir)
		{
			var mass = ch.Mass * ThrustToMassRatio;
			if (mass < Shrinker.StaticMinMass)
				return;

            ch.Mass -= UsageFee;
            dir = dir.normalized;
            CellFactory.Thrust (gameObject, mass, dir);
		}
	}
}