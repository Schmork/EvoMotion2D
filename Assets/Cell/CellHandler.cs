using UnityEngine;

namespace EvoMotion2D.Cell
{
	public class CellHandler : MonoBehaviour
	{
		Thruster thruster;
		[Range(0.01f, 50f)]
		public float
			Scale;

		void Awake ()
		{
			thruster = GetComponent<Thruster> ();
		}

		public float Mass {
			get { return GetComponent<Rigidbody2D> ().mass; }
			set {
				GetComponent<Rigidbody2D> ().mass = value;
				var radius = getRadius ();
				GetComponent<Transform> ().localScale = new Vector2 (radius * 2, radius * 2);
			}
		}

		public float Radius {
			get { return getRadius (); }
		}

		public Vector2 Pos {
			get { return transform.position; }
			set { transform.position = value; }
		}

		public Vector2 Vel {
			get { return GetComponent<Rigidbody2D> ().velocity; }
			set { GetComponent<Rigidbody2D> ().velocity = value; }
		}

		public void Move (Vector2 dir)
		{
			thruster.Thrust (dir);
		}

		float getRadius ()
		{
			return Mathf.Sqrt (GetComponent<Rigidbody2D> ().mass * Scale);
		}
	}
}