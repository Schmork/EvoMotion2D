namespace EvoMotion2D.Parameters
{
    public class SignedMutateableParameter : MutateableParameter
    {
        public SignedMutateableParameter() : base (UnityEngine.Random.Range(
                -InitialParameters.StaticInitialValueRange,
                +InitialParameters.StaticInitialValueRange))
            { 
        }
    }
}
