using UnityEngine;
using System.Collections;
using AssemblyCSharp.NeuronalNetwork;

namespace AssemblyCSharp
{
	public class Controller : MonoBehaviour
	{
		CellHandler ch;
		Brain brain;

		// Use this for initialization
		void Start ()
		{
			ch = GetComponent<CellHandler> ();
			brain = GetComponent<Brain> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (ch.Mass < Shrinker.StaticMinMass / Thruster.StaticThrustToMassRatio)
				enabled = false;

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
		}
	}
}