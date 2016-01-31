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

		public void Thrust ()
		{
            if (!canThrust()) return;

            var mass = getThrustMass();

            //if (ch.Mass < Shrinker.StaticMinMass * 5) UsageFee = -getThrustMass() * 0.7f;     // reward and encouragement to move. Not given to biggies.
            ch.Mass -= UsageFee;            
            CellFactory.Thrust (gameObject, mass);
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