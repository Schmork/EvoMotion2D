using UnityEngine;
using EvoMotion2D.Cell;
using System.Collections.Generic;

namespace EvoMotion2D
{
	public class CellFactory : MonoBehaviour
	{
        public GameObject Cube;
        static GameObject cube;

        public static List<GameObject> Cells = new List<GameObject>();

        void Start()
        {
            cube = Cube;
        }

		public static GameObject Spawn (GameObject cell, GameObject spawnArea, float mass) {			
			var spawn = Instantiate(cell);
            Cells.Add(spawn);

			var box = spawnArea.GetComponent<BoxCollider2D> ();
            var x = Random.Range(0, box.size.x) - box.size.x / 2;
            var y = Random.Range(0, box.size.y) - box.size.y / 2;

			spawn.transform.position = new Vector2 (x, y);
			spawn.GetComponent<CellHandler> ().Mass = mass;			
			spawn.name = Util.CreatePassword (5);
			spawn.transform.parent = spawnArea.transform;
			
			spawn.GetComponent<SpriteRenderer> ().color = new Color (Random.Range(0.3f, 1),
			                                                        Random.Range(0.3f, 1),
			                                                        Random.Range(0.3f, 1));
            
            spawn.transform.rotation = new Quaternion(0, 0, Random.value, Random.value);
            return spawn;
		}

        public static GameObject Thrust (GameObject cell, float mass)
		{
			var ch = cell.GetComponent<CellHandler> ();

			var thrust = Instantiate(cell);
            Cells.Add(thrust);

			var tCh = thrust.GetComponent<CellHandler> ();
		
			tCh.Mass = mass;
            ch.Mass -= mass * 0.75f;

            var extraSpace = 1.01f;
            Vector2 thrustDirection = - cell.GetComponent<Rigidbody2D>().transform.right;
            var distance = thrustDirection * (ch.Radius + tCh.Radius) * extraSpace;

            tCh.Pos = ch.Pos + distance;
            tCh.Vel = ch.Vel;

            var rotationRange = 20;     // thrust ojects divert by +/- this value from their parent's rotation
            var rotationVariance = Random.Range(-rotationRange, rotationRange);
            thrust.transform.Rotate(new Vector3(0, 0, 180 - rotationVariance));

            thrustDirection *= 7 * ch.Mass;  // adjust force
            var factorThrustFasterThanCell = 3f;// 1/5f;
		
			cell.GetComponent<Rigidbody2D> ().AddForce (-thrustDirection);
			thrust.GetComponent<Rigidbody2D> ().AddForce (thrustDirection * mass / ch.Mass * factorThrustFasterThanCell);
		
			thrust.transform.parent = cell.transform.parent;
			thrust.name = cell.name + Util.CreatePassword (1);

            var parentColor = ch.Color;
			var drift = Random.insideUnitSphere * 0.07f;    // slight change in color for children
		
			var childColor = new Color (parentColor.r + drift.x,
		                            parentColor.g + drift.y,
		                            parentColor.b + drift.z);
		
			tCh.Color = childColor;

            mutateParameters(cell, thrust);

            return thrust;
		}

        static void mutateParameters(GameObject parent, GameObject child)
        {
            var cSensorHandler = child.GetComponentInChildren<SensorHandler>();
            var pSensorHandler = parent.GetComponentInChildren<SensorHandler>();

            for (int i = 0; i < pSensorHandler.NumberOfSensors; i++)
            {
                var tSensor = cSensorHandler.getSensors()[i];
                var cSensor = pSensorHandler.getSensors()[i];

                tSensor.MaxFleeDistance = cSensor.MaxFleeDistance.Mutate();
                tSensor.PreyFactor = cSensor.PreyFactor.Mutate();
                tSensor.ScanChance = cSensor.ScanChance.Mutate();
                tSensor.ScanMaxRange = cSensor.ScanMaxRange.Mutate();
                tSensor.ImportanceFactor = cSensor.ImportanceFactor.Mutate();

                tSensor.WhatToWatch = cSensor.WhatToWatch;
            }

            var tController = child.GetComponent<Controller>();
            var cController = parent.GetComponent<Controller>();

            tController.Cooldown = cController.Cooldown.Mutate();
            tController.MaxAngleForThrusting = cController.MaxAngleForThrusting.Mutate();
            child.GetComponent<Thruster>().ThrustToMassRatio = parent.GetComponent<Thruster>().ThrustToMassRatio.Mutate();
        }
	}
}