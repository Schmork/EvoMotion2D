// this is where the cell decides if it should turn or move or to which of it's sensors it should listen.


using UnityEngine;
using EvoMotion2D.Modules;
using EvoMotion2D.Parameters;

namespace EvoMotion2D.Cell
{
    public class Controller : MonoBehaviour
    {
        CellHandler cellHandler;
        SensorHandler sensorHandler;
        SpriteRenderer sprite;
        Thruster thruster;
        Rigidbody2D rb2d;
		public UnsignedMutateableParameter Cooldown, MinTurnForce, MaxTurnForce;
		public ClampedMutateableParameter MaxPreyAngle, MaxPredatorAngle;
        float LastEjection;

        GameObject primaryTarget;

        // Use this for initialization
        void Start()
        {
            Cooldown = new UnsignedMutateableParameter();
			MinTurnForce = new UnsignedMutateableParameter(0.01f);
			MaxTurnForce = new UnsignedMutateableParameter(1f);
			MaxPreyAngle = new ClampedMutateableParameter (1f, 180f);
			MaxPredatorAngle = new ClampedMutateableParameter (1f, 180f);
			//MaxPredatorAngle.Value = 45f;
            cellHandler = GetComponent<CellHandler>();
            sensorHandler = GetComponent<SensorHandler>();
            sprite = GetComponent<SpriteRenderer>();
            thruster = GetComponent<Thruster>();
            rb2d = GetComponent<Rigidbody2D>();
            LastEjection = Time.time;
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
            foreach (var sensor in sensorHandler.GetComponentsInChildren<Sensor>())
            {
                sensor.enabled = status;
            }
        }
        
        // Update is called once per frame
        void Update()
        {
            if (!thruster.canThrust()) Deactivate();

            var sensors = sensorHandler.getSensors();
            var maxScore = float.MinValue;
            Sensor importantSensor = null;

            for (int i = 0; i < sensors.Length; i++)
            {
                sensors[i].Recheck();
                if (sensors[i].target == null) continue;
                var sensor = sensors[i];

                var yourMass = sensor.target.GetComponent<CellHandler>().Mass;
                var myMass = cellHandler.Mass;

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
                    importantSensor = sensor;
                }
            }

            if (importantSensor != null && Time.time > LastEjection + Cooldown.Value)
            {
                LastEjection = Time.time;
                var sensor = importantSensor;
                primaryTarget = sensor.target;

                // where is the target?
                Vector2 targetDirection = primaryTarget.transform.position - transform.position;
                // where are we looking?
                Vector2 lookDirection = rb2d.transform.right;

                // to indicate the sign of the (otherwise absolute) angle
                Vector3 cross = Vector3.Cross(targetDirection, lookDirection);
                // actually get the sign (either 1 or -1)
                float sign = Mathf.Sign(cross.z);

                // the angle, ranging from 0 to 180 degrees
                float angle = Vector2.Angle(targetDirection, lookDirection);
                bool preyTurn = angle > MaxPreyAngle.Value;
                bool predatorTurn = angle < MaxPredatorAngle.Value;
                
                if (sensor.GetComponent<Sensor>().WhatToWatch == Sensor.WatchType.PREDATOR)
                {
                    if (predatorTurn)
                    {
						var force = Mathf.Sqrt((180 - angle) / MaxPredatorAngle.Value) * (MaxTurnForce.Value - MinTurnForce.Value) + MinTurnForce.Value;
						force *= sign * Time.deltaTime;
                        rb2d.AddTorque(force);
                    }
                    else cellHandler.Move();
                }

                if (sensor.GetComponent<Sensor>().WhatToWatch == Sensor.WatchType.PREY)
                {
                    if (preyTurn)
                    {
						var force = Mathf.Sqrt(angle / MaxPreyAngle.Value) * (MaxTurnForce.Value - MinTurnForce.Value) + MinTurnForce.Value;
						force *= sign * Time.deltaTime;
                        rb2d.AddTorque(-force);
                    }
                    else cellHandler.Move();
                }
            }
        }
    }
}