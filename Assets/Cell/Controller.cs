using UnityEngine;
using AssemblyCSharp.Modules;

namespace AssemblyCSharp
{
	public class Controller : MonoBehaviour
	{
		CellHandler ch;
        SensorHandler sh;
        public Parameter Cooldown = new Parameter(true);
        float LastEjection;

		// Use this for initialization
		void Start ()
		{
			ch = GetComponent<CellHandler> ();
            sh = GetComponent<SensorHandler>();
            LastEjection = Time.time;

            while (Cooldown.Value < 0.05f) Cooldown.Value += 0.002f;
            while (Cooldown.Value > 2.5f) Cooldown.Value *= 0.99f;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (ch.Mass < Shrinker.StaticMinMass / GetComponent<Thruster>().ThrustToMassRatio.Value)
				enabled = false;
            
            var sensor = sh.getSensors()[0];

            if (sensor.target != null && Time.time > LastEjection + Cooldown.Value)
            {
                LastEjection = Time.time;
                ch.Move(sensor.target.transform.position - transform.position);
            }

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