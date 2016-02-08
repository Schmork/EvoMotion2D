using UnityEngine;

namespace EvoMotion2D.Cell
{
	public class CellHandler : MonoBehaviour
	{
		Thruster thruster;
		[Range(0.01f, 50f)]
        Rigidbody2D rb2d;
        SpriteRenderer spriteRenderer;
        public float CollectedMass;
		public float Age;
		public int MaxAge;		// not really a max - but from this point on, cells start to shrink drastically
		public bool HasStrayedOff;

        public GameObject toolTip;
		
		void Awake ()
		{
			thruster = GetComponent<Thruster> ();
            rb2d = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
			Age = 0f;
            toolTip = Instantiate(toolTip);// GetComponentInChildren<TextMesh>();
		}

		void Update() {
			Age += Mathf.Sqrt(Mass);

            var textMesh = toolTip.GetComponentInChildren<TextMesh>();

            textMesh.color = changeAlpha(-0.01f);

            var format = "{0:0.#}";

            var text = name + "\n";
            text += "mass: " + string.Format(format, Mass) + "\n";
            text += "age : " + string.Format(format, Age) + "\n";
            text += "coll: " + string.Format(format, CollectedMass) + "\n";
            textMesh.text = text;

            var bounds = textMesh.GetComponent<Renderer>().bounds;
            var offset = new Vector3(0, bounds.extents.y * 1.1f + 1, -1);
            
            toolTip.transform.position = transform.position + getRadius() * Vector3.up * 2f + offset;
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
			return getRadius (rb2d.mass);
		}

		public static float getRadius(float mass) {
			return Mathf.Sqrt(mass);
		}

        void OnMouseOver()
        {
            var textMesh = toolTip.GetComponentInChildren<TextMesh>();
            var color = textMesh.color;
            var factor = 1.7f;
            color = new Color(Color.r * factor,
                              Color.g * factor,
                              Color.b * factor,
                              1f);  // make fully visible
            textMesh.text = name;
            //textMesh.fontSize = (int)(Mass * 50);
            textMesh.color = color;
        }

        Color changeAlpha(float change)
        {
            var color = toolTip.GetComponentInChildren<TextMesh>().color;
            return new Color(color.r,
                             color.g,
                             color.b,
                             color.a + change);
        }
    }
}