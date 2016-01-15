using UnityEngine;
using EvoMotion2D.Cell;
using EvoMotion2D.Modules;

namespace EvoMotion2D
{
	public class CellFactory : MonoBehaviour
	{
		public static GameObject Spawn (GameObject cell, GameObject spawnArea, float mass) {			
			var spawn = GameObject.Instantiate (cell);
			
			var box = spawnArea.GetComponent<BoxCollider2D> ();
			var x = Random.value * box.size.x * 2 - box.size.x;
			var y = Random.value * box.size.y * 2 - box.size.y;

			spawn.transform.position = new Vector2 (x, y);
			spawn.GetComponent<Rigidbody2D> ().mass = mass;			
			spawn.name = Util.CreatePassword (5);
			spawn.transform.parent = spawnArea.transform;
			
			spawn.GetComponent<SpriteRenderer> ().color = new Color (Random.Range(0.3f, 1),
			                                                        Random.Range(0.3f, 1),
			                                                        Random.Range(0.3f, 1));

			return spawn;
		}

		public static GameObject Thrust (GameObject cell, float mass, Vector2 dir)
		{
			var ch = cell.GetComponent<CellHandler> ();

			var thrust = GameObject.Instantiate (cell);
			var tCh = thrust.GetComponent<CellHandler> ();
		
			tCh.Mass = mass;
            ch.Mass -= mass;

			tCh.Vel = ch.Vel;
			tCh.Pos = ch.Pos + dir * (ch.Radius + tCh.Radius + 0.2f) * 0.2f;
		
			dir *= -2000;
			var factor = 13f;
		
			cell.GetComponent<Rigidbody2D> ().AddForce (dir);
			thrust.GetComponent<Rigidbody2D> ().AddForce (-dir * mass / ch.Mass * factor);
		
			thrust.transform.parent = cell.transform.parent;
			thrust.name = cell.name + Util.CreatePassword (1);
		
			var parentColor = cell.GetComponent<SpriteRenderer> ().color;
			var drift = Random.insideUnitSphere * 0.03f;
		
			var childColor = new Color (parentColor.r + drift.x,
		                            parentColor.g + drift.y,
		                            parentColor.b + drift.z);
		
			thrust.GetComponent<SpriteRenderer> ().color = childColor;

            var tSensor = thrust.GetComponentInChildren<Sensor>();
            var cSensor = cell.GetComponentInChildren<Sensor>();

            tSensor.MaxFleeDistance = cSensor.MaxFleeDistance;
            tSensor.MaxFleeDistance.Mutate();

            tSensor.PreyFactor = cSensor.PreyFactor;
            tSensor.PreyFactor.Mutate();

            tSensor.ScanChance = cSensor.ScanChance;
            tSensor.ScanChance.Mutate();

            tSensor.ScanMaxRange = cSensor.ScanMaxRange;
            tSensor.ScanMaxRange.Mutate();

            var tCont = thrust.GetComponent<Controller>();
            var cCont = cell.GetComponent<Controller>();

            tCont.Cooldown = cCont.Cooldown;
            tCont.Cooldown.Mutate();

            var tThrust = thrust.GetComponent<Thruster>();
            var cThrust = cell.GetComponent<Thruster>();

            tThrust.ThrustToMassRatio = cThrust.ThrustToMassRatio;
            tThrust.ThrustToMassRatio.Mutate();

            return thrust;
		}
	}
}