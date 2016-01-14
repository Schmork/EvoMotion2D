using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class Controller : MonoBehaviour
	{
		CellHandler ch;

		// Use this for initialization
		void Start ()
		{
			ch = GetComponent<CellHandler> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (ch.Mass < Shrinker.StaticMinMass / Thruster.StaticThrustToMassRatio)
				enabled = false;

			/*
			// TODO: replace
			var decision = brain.GetOutput ();

			if (decision.Move) {
				ch.Move(new Vector2(decision.DirX, decision.DirY));
			}

			if (decision.Switch == null)
				return;

			for (int i = 0; i < decision.Switch.Length; i++) {
				if (decision.Switch[i])
					GetComponentsInChildren<Sensor>()[i].SwitchToCandidate();
			}
			*/
		}
	}
}