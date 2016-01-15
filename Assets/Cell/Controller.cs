using UnityEngine;
using EvoMotion2D.Modules;

namespace EvoMotion2D.Cell
{
	public class Controller : MonoBehaviour
	{
		CellHandler ch;
        SensorHandler sh;
        public UnsignedMutateableFloat Cooldown;
        float LastEjection;

		// Use this for initialization
		void Start ()
		{
			ch = GetComponent<CellHandler> ();
            sh = GetComponent<SensorHandler>();
            LastEjection = Time.time;

            while (Cooldown < 0.05f) Cooldown += 0.002f;
            while (Cooldown > 2.5f) Cooldown *= 0.99f;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (ch.Mass < Shrinker.StaticMinMass / GetComponent<Thruster>().ThrustToMassRatio)
				enabled = false;
            
            var sensor = sh.getSensors()[0];

            if (sensor.target != null && Time.time > LastEjection + Cooldown)
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