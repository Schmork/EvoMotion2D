namespace EvoMotion2D.Parameters
{
    public class SignedMutateableParameter : MutateableParameter
    {
        public static implicit operator SignedMutateableParameter(float value)
        {
            MutateableParameter temp = value;
            return temp as SignedMutateableParameter;
        }

        public static new SignedMutateableParameter Random()
        {
            return UnityEngine.Random.Range(
                -InitialParameters.StaticInitialValueRange,
                +InitialParameters.StaticInitialValueRange);
        }
    }
}
