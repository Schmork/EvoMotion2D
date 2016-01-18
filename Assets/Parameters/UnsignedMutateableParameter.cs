namespace EvoMotion2D.Parameters
{
    public class UnsignedMutateableParameter : MutateableParameter
    {
        public static implicit operator UnsignedMutateableParameter(float value)
        {
            System.Diagnostics.Debug.Assert(value >= 0);

            MutateableParameter temp = value;
            return temp as UnsignedMutateableParameter;
        }

        public static new UnsignedMutateableParameter Random()
        {
            return UnityEngine.Random.Range(
                0,
                +InitialParameters.StaticInitialValueRange);
        }

        public new UnsignedMutateableParameter Mutate()
        {
            var temp = base.Mutate() as UnsignedMutateableParameter;
            if (temp.Value < 0) temp.Value = 0;
            return temp;
        }
    }
}
