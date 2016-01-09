using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class CellFactory : MonoBehaviour
	{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public static GameObject Spawn (GameObject cell, GameObject spawnArea, float mass) {			
			var spawn = GameObject.Instantiate (cell);
			
			var box = spawnArea.GetComponent<BoxCollider2D> ();
			var x = Random.value * box.size.x * 2 - box.size.x;
			var y = Random.value * box.size.y * 2 - box.size.y;

			spawn.transform.position = new Vector2 (x, y);
			spawn.GetComponent<Rigidbody2D> ().mass = mass;			
			spawn.name = AssemblyCSharp.NeuronalNetwork.Util.CreatePassword (5);
			spawn.transform.parent = spawnArea.transform;
			
			spawn.GetComponent<SpriteRenderer> ().color = new Color (Random.Range(0.3f, 1),
			                                                        Random.Range(0.3f, 1),
			                                                        Random.Range(0.3f, 1));

			spawn.AddComponent<AssemblyCSharp.NeuronalNetwork.Brain>();

			return spawn;
		}

		public static GameObject Thrust (GameObject cell, float mass, Vector2 dir)
		{
			var ch = cell.GetComponent<CellHandler> ();

			var thrust = GameObject.Instantiate (cell);
			var tCh = thrust.GetComponent<CellHandler> ();
		
			tCh.Mass = mass;
			tCh.Vel = ch.Vel;
			tCh.Pos = ch.Pos + dir * (ch.Radius + tCh.Radius + 0.2f) * 0.2f;
		
			dir *= -2000;
			var factor = 13f;
		
			cell.GetComponent<Rigidbody2D> ().AddForce (dir);
			thrust.GetComponent<Rigidbody2D> ().AddForce (-dir * mass / ch.Mass * factor);
		
			thrust.transform.parent = cell.transform.parent;
			thrust.name = cell.name + AssemblyCSharp.NeuronalNetwork.Util.CreatePassword (1);
		
			var parentColor = cell.GetComponent<SpriteRenderer> ().color;
			var drift = Random.insideUnitSphere * 0.03f;
		
			var childColor = new Color (parentColor.r + drift.x,
		                            parentColor.g + drift.y,
		                            parentColor.b + drift.z);
		
			thrust.GetComponent<SpriteRenderer> ().color = childColor;

			thrust.GetComponent<NeuronalNetwork.Brain> ().parent = cell.GetComponent<NeuronalNetwork.Brain> ();

			return thrust;
		}
	}
}