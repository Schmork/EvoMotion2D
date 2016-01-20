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

        public ClampedMutateableParameter ThrustToMassRatio;

        void Awake ()
		{
            ThrustToMassRatio = new ClampedMutateableParameter(0.1f, 0.3f);
            ch = GetComponent<CellHandler> ();
        }

		public void Thrust (Vector2 dir)
		{
            var mass = getThrustMass();
			if (!canThrust()) return;

            ch.Mass -= UsageFee;
            dir = dir.normalized;
            CellFactory.Thrust (gameObject, mass, dir);
		}

        public float getThrustMass()
        {
            return ch.Mass * ThrustToMassRatio.Value;
        }

        public bool canThrust()
        {
            return getThrustMass() > Shrinker.StaticMinMass;
        }
	}
}