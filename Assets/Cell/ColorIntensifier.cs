// this class intensifies a cell's color. 
// As they collide and mix with each other, their color tends towards an average, grayish color.
// this class counteracts this movement to have more colors than just gray in the long run.


using UnityEngine;

namespace EvoMotion2D.Cell
{
    class ColorIntensifier : MonoBehaviour
    {
		public float ColorShrinkAmount;

        CellHandler ch;

        void Start()
        {
            ch = GetComponent<CellHandler>();
        }

        void Update()
        {
            var color = ch.Color;

            float[] values = { color.r, color.g, color.b };

            int minIndex = 0;
            int maxIndex = 0;
            float min = float.MaxValue;
            float max = float.MinValue;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                    maxIndex = i;
                }
                if (values[i] < min)
                {
                    min = values[i];
                    minIndex = i;
                }
            }

            if (min != float.MinValue)
                values[minIndex] -= ColorShrinkAmount;
            if (max != float.MaxValue)
                values[maxIndex] += ColorShrinkAmount;

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Mathf.Clamp(values[i], 0, 1);
            }

            ch.Color = new Color(values[0], values[1], values[2]);
        }
    }
}
