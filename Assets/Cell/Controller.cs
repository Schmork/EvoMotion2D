using UnityEngine;
using EvoMotion2D.Modules;
using EvoMotion2D.Parameters;

namespace EvoMotion2D.Cell
{
	public class Controller : MonoBehaviour
	{
		CellHandler ch;
        SensorHandler sh;
        public UnsignedMutateableParameter Cooldown;
        float LastEjection;

		// Use this for initialization
		void Start ()
		{
            Cooldown = new UnsignedMutateableParameter();
			ch = GetComponent<CellHandler> ();
            sh = GetComponent<SensorHandler>();
            LastEjection = Time.time;

            while (Cooldown.Value < 0.05f) Cooldown.Value += 0.002f;
            while (Cooldown.Value > 2.5f) Cooldown.Value *= 0.99f;
		}
	
        public void Activate()
        {
            notifyAll(true);
        }

        public void Deactivate()
        {
            notifyAll(false);
        }

        void notifyAll(bool status)
        {
            enabled = status;
            foreach (var sensor in sh.GetComponentsInChildren<Sensor>())
            {
                sensor.enabled = status;
            }
        }

		// Update is called once per frame
		void Update ()
		{
            if (!GetComponent<Thruster>().canThrust()) Deactivate();

            var sensors = sh.getSensors();
            var maxScore = -1f;
            var importantSensorIndex = -1;

            for (int i = 0; i < sensors.Length; i++)
            {
                if (sensors[i].target == null) continue;
                var sensor = sensors[i];

                var yourMass = sensor.target.GetComponent<CellHandler>().Mass;
                var myMass = GetComponent<CellHandler>().Mass;
                
                if (sensor.WhatToWatch == Sensor.WatchType.PREY && yourMass > myMass)
                {
                    sensor.target = null;
                    continue;
                }
                if (sensor.WhatToWatch == Sensor.WatchType.PREDATOR && yourMass < myMass)
                {
                    sensor.target = null;
                    continue;
                }

                var score = sensor.getScore() * sensor.ImportanceFactor.Value;

                if (score > maxScore)
                {
                    maxScore = score;
                    importantSensorIndex = i;
                }
            }

            if (importantSensorIndex != -1 && Time.time > LastEjection + Cooldown.Value)
            {
                var sensor = sh.getSensors()[importantSensorIndex];

                LastEjection = Time.time;

                float moveDir = 0f;
                if (sensor.GetComponent<Sensor>().WhatToWatch == Sensor.WatchType.PREDATOR)
                {
                    moveDir = 1;
                }

                if (sensor.GetComponent<Sensor>().WhatToWatch == Sensor.WatchType.PREY)
                {
                    moveDir = -1;
                }
                Debug.Assert(Mathf.Abs(moveDir) == 1);
                
                ch.Move(moveDir * (sensor.target.transform.position - transform.position));
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