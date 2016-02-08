using UnityEngine;
using EvoMotion2D.Cell;
using System.Collections.Generic;

namespace EvoMotion2D
{
	public class CellFactory : MonoBehaviour
	{
		public float ColorDrift;
		public static float colorDrift;

		public float SpawnRadius;
		public static float spawnRadius;

        public static List<GameObject> Cells = new List<GameObject>();

		public List<Sprite> Images;
		public static List<Sprite> images;

        void Start()
        {
			colorDrift = ColorDrift;
			spawnRadius = SpawnRadius;
			images = Images;
        }

		public static GameObject Spawn (GameObject cell, GameObject spawnArea, float mass) {			

			var point = Random.insideUnitCircle * spawnRadius;

			var obstacle = Physics2D.OverlapCircle (point, CellHandler.getRadius(mass * 3));
			if (obstacle) return null;

			var spawn = Instantiate(cell);

			spawn.transform.position = point;
            spawn.transform.rotation = new Quaternion(0, 0, Random.value, Random.value);
			spawn.GetComponent<CellHandler> ().Mass = mass;			
			spawn.name = Util.CreatePassword (5);
			spawn.transform.parent = spawnArea.transform;

			var spriteRenderer = spawn.GetComponent<SpriteRenderer> ();
			
			spriteRenderer.color = new Color (Random.Range(0.3f, 1),
			                                  Random.Range(0.3f, 1),
			                                  Random.Range(0.3f, 1));
			//spriteRenderer.sprite = images [(int)(Random.value * images.Count)];

			Cells.Add(spawn);
			
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

			thrustDirection *= 7 * ch.Mass * Time.deltaTime;  // adjust force
            var factorThrustFasterThanCell = 3f;// 1/5f;
		
			cell.GetComponent<Rigidbody2D> ().AddForce (-thrustDirection);
			thrust.GetComponent<Rigidbody2D> ().AddForce (thrustDirection * mass / ch.Mass * factorThrustFasterThanCell);
		
			thrust.transform.parent = cell.transform.parent;
			thrust.name = cell.name + Util.CreatePassword (1);

            var parentColor = ch.Color;
			var drift = Random.insideUnitSphere * colorDrift;    // slight change in color for children
		
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
			tController.MaxPreyAngle = cController.MaxPreyAngle.Mutate ();
			tController.MaxPredatorAngle = cController.MaxPredatorAngle.Mutate ();
			tController.MinTurnForce = cController.MinTurnForce.Mutate ();
			tController.MaxTurnForce = cController.MaxTurnForce.Mutate ();
            child.GetComponent<Thruster>().ThrustToMassRatio = parent.GetComponent<Thruster>().ThrustToMassRatio.Mutate();
        }
	}
}