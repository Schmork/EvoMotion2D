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
        public UnsignedMutateableParameter Cooldown;
        public UnsignedMutateableParameter MaxAngleForThrusting;
        float LastEjection;

        GameObject primaryTarget;

        // Use this for initialization
        void Start()
        {
            Cooldown = new UnsignedMutateableParameter();
            MaxAngleForThrusting = new UnsignedMutateableParameter();
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

                // rotate if angle is bigger than that to decrease it
                float maxPreyAngle = 9f;
                float maxPredatorAngle = 30f;

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
                bool preyTurn = angle > maxPreyAngle;
                bool predatorTurn = angle > maxPredatorAngle;

                var min = 0.8f;
                var max = 12f;

                var turnTowardsForce = Mathf.Clamp(angle / 4f, min, max) * sign;
                var turnAwayForce = Mathf.Clamp((180 - angle) / 3f, min, max) * sign;
                
                if (sensor.GetComponent<Sensor>().WhatToWatch == Sensor.WatchType.PREDATOR)
                {
                    if (predatorTurn)
                    {
                        rb2d.AddTorque(turnAwayForce);
                    }
                    else cellHandler.Move();
                }

                if (sensor.GetComponent<Sensor>().WhatToWatch == Sensor.WatchType.PREY)
                {
                    if (preyTurn)
                    {
                        rb2d.AddTorque(-turnTowardsForce);
                    }
                    else cellHandler.Move();
                }
            }
        }
    }
}