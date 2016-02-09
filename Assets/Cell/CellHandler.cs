// this class serves as a central node in the network of classes. It provides access to some of the cell's attributes, like mass for example.


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
        public int Generation;
        public int Children;

        public GameObject toolTip;
		
		void Awake ()
		{
			thruster = GetComponent<Thruster> ();
            rb2d = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
			Age = 0f;
            toolTip = Instantiate(toolTip);// GetComponentInChildren<TextMesh>();
            toolTip.GetComponent<ToolTipHandler>().cellHandler = this;
		}

		void Update() {
			Age += Mass * Time.deltaTime;

            var textMesh = toolTip.GetComponentInChildren<TextMesh>();
            var bounds = toolTip.GetComponentInChildren<SpriteRenderer>().bounds;
            var offset = new Vector3(0, bounds.extents.y + 2, -1);
            
            toolTip.transform.position = transform.position + 1.1f * getRadius() * Vector3.up + offset;
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
            var textMesh = toolTip.GetComponentInChildren<TextMesh>();              // tooltip text
            var spriteRenderer = toolTip.GetComponentInChildren<SpriteRenderer>();  // tooltip background

            var factor = 1.7f;
            
            var color = textMesh.color;
            color = new Color(Color.r * factor,
                              Color.g * factor,
                              Color.b * factor,
                              1f);                                          // make fully visible
            textMesh.color = color;

            color = spriteRenderer.color;
            color = new Color(Color.r * factor,
                              Color.g * factor,
                              Color.b * factor,
                              ToolTipHandler.TextToBackgroundAlphaRatio);   // make less visible
            spriteRenderer.color = color;
        }
    }
}