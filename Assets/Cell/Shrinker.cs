using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
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

			rb.drag = rb.velocity.magnitude / 4f;

			if (ch.Mass > 1)
            {
                ch.Mass = Mathf.Pow(ch.Mass, 0.9996f);    
            }
            else
            {
                ch.Mass *= 0.9999f;
            }
             
			ch.Mass -= .0001f;
			if (ch.Mass < MinMass)
				GameObject.Destroy (gameObject);
		}

		void extremizeColor ()
		{
			var color = GetComponent<SpriteRenderer> ().color;
		
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
		
			var newColor = new Color (values [0], values [1], values [2]);
			GetComponent<SpriteRenderer> ().color = newColor;
		}
	}
}
