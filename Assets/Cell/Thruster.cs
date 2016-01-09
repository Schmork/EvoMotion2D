using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class Thruster : MonoBehaviour
	{
		[Range(0, 1)]
		public float ThrustToMassRatio;
		public static float StaticThrustToMassRatio;

		public GameObject ThrustObject;
		CellHandler ch;

		// Use this for initialization
		void Awake ()
		{
			StaticThrustToMassRatio = ThrustToMassRatio;
			ch = GetComponent<CellHandler> ();
		}

		public void Thrust (Vector2 dir)
		{
			dir = dir.normalized;

			var mass = ch.Mass * ThrustToMassRatio;
			if (mass < Shrinker.StaticMinMass)
				return;

			CellFactory.Thrust (gameObject, mass, dir);
		}
	}
}