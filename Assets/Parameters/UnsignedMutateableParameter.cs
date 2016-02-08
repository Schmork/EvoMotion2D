namespace EvoMotion2D.Parameters
{
    public class UnsignedMutateableParameter : MutateableParameter
    {
        public UnsignedMutateableParameter() : base(UnityEngine.Random.Range(
                0,
                +InitialParameters.StaticInitialValueRange))
        { }

		public UnsignedMutateableParameter(float max) : base(UnityEngine.Random.Range(0, max))
		{ }
        
        public new UnsignedMutateableParameter Mutate()
        {
            //UnityEngine.Debug.Log("Before : value = " + Value + ", chance = " + MutationChance + ", amount = " + MutationAmount);

            var mp = base.Mutate();
            if (mp.Value < 0) mp.Value = 0;
            
            var ump = new UnsignedMutateableParameter();
            ump.Value = mp.Value;
            ump.MutationChance = mp.MutationChance;
            ump.MutationAmount = mp.MutationAmount;

            //UnityEngine.Debug.Log("After  : value = " + ump.Value + ", chance = " + ump.MutationChance + ", amount = " + ump.MutationAmount);

            return ump;
        }
    }
}
