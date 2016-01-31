using UnityEngine;

namespace EvoMotion2D.Cell
{
	public class CellHandler : MonoBehaviour
	{
		Thruster thruster;
		[Range(0.01f, 50f)]
		public float Scale;
        Rigidbody2D rb2d;
        SpriteRenderer spriteRenderer;
        public float CollectedMass;

		void Awake ()
		{
			thruster = GetComponent<Thruster> ();
            rb2d = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
		}
        
		public float Mass {
			get { return GetComponent<Rigidbody2D> ().mass; }
			set {
                rb2d.mass = value;
				var radius = getRadius ();
                transform.localScale = new Vector2(radius, radius);
			}
		}

        public Color Color 
        {
            get { return spriteRenderer.color; }
            set { spriteRenderer.color = value; }
        }


		public float Radius {
			get { return getRadius (); }
		}

		public Vector2 Pos {
			get { return transform.position; }
			set { transform.position = value; }
		}

		public Vector2 Vel {
			get { return rb2d.velocity; }
			set { rb2d.velocity = value; }
		}

		public void Move ()
		{
			thruster.Thrust ();
		}

		float getRadius ()
		{
            return Mathf.Sqrt(rb2d.mass * Scale);
		}
	}
}