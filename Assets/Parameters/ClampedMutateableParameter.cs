namespace EvoMotion2D.Parameters
{
    public class ClampedMutateableParameter : MutateableParameter
    {
        static float min = 0.00001f;
        static float max = 1;

        public static implicit operator ClampedMutateableParameter(float value)
        {
            System.Diagnostics.Debug.Assert(value >= 0);

            MutateableParameter temp = value;
            return temp as ClampedMutateableParameter;
        }
        public static new ClampedMutateableParameter Random()
        {
            return UnityEngine.Random.Range(min, max);
        }

        public new ClampedMutateableParameter Mutate()
        {
            var temp = base.Mutate() as ClampedMutateableParameter;
            if (temp.Value < min) temp.Value = min;
            if (temp.Value > max) temp.Value = max;
            return temp;
        }
    }
}
