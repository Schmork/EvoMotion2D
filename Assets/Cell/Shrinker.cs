using UnityEngine;

namespace EvoMotion2D.Cell
{
	public class Shrinker : MonoBehaviour
	{
		[Range(0.001f, 1)]
		public float MinMass;
		public static float StaticMinMass;
		CellHandler ch;
		Rigidbody2D rb;
		public float ColorShrinkAmount;

		// Use this for initialization
		void Start ()
		{
			StaticMinMass = MinMass;
			ch = GetComponent<CellHandler> ();
			rb = GetComponent<Rigidbody2D> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
			extremizeColor ();

            var fattieBonus = 0f;
            if (ch.Mass > 1) fattieBonus = Mathf.Sqrt(ch.Mass / 10f);

            rb.drag = rb.velocity.magnitude - fattieBonus;

            var minDrag = 0.04f;
            if (rb.drag < minDrag) rb.drag = minDrag;
            
			if (ch.Mass > 1)
            {
                ch.Mass = Mathf.Pow(ch.Mass, 1 - ch.Mass / 99999f);    
            }
            else
            {
                ch.Mass *= 0.99999f;
            }
             
			ch.Mass -= .000003f;
            if (ch.Mass < MinMass)
            {
                CellFactory.Cells.Remove(gameObject);
                GameObject.Destroy(gameObject);
            }
		}

		void extremizeColor ()
		{
            var color = ch.Color;
		
			float[] values = { color.r, color.g, color.b };
		
			int minIndex = 0;
			int maxIndex = 0;
			float min = float.MaxValue;
			float max = float.MinValue;
			for (int i = 0; i < values.Length; i++) {
				if (values [i] > max) {
					max = values [i];
					maxIndex = i;
				}
				if (values [i] < min) {
					min = values [i];
					minIndex = i;
				}
			}

			if (min != float.MinValue)
				values [minIndex] -= ColorShrinkAmount;
			if (max != float.MaxValue)
				values [maxIndex] += ColorShrinkAmount;
		
			for (int i = 0; i < values.Length; i++) {
				values [i] = Mathf.Clamp (values [i], 0, 1);
			}

            ch.Color = new Color (values [0], values [1], values [2]);
		}
	}
}
