namespace EvoMotion2D.Modules
{
	public abstract class Parameter : IParameter
	{
        protected float InitialValueRange { get; private set; }
        float MinMutationChance = 0.001f;
        float MinMutationAmount = 0.001f;

        public float Value {
			get;
			set;
		}

		public float MutationChance {
			get;
			set;
		}

		public float MutationAmount {
			get;
			set;
		}
        
		public Parameter()
		{
			MutationChance = Util.SignedRange (InitialValueRange);
			MutationAmount = Util.SignedRange (InitialValueRange);
		}

		public void Mutate ()
		{
			if (Util.Rnd.NextDouble() < MutationChance)
                MutationChance += (float)Util.Rnd.NextDouble() * 2 * MutationAmount - MutationAmount;
            if (MutationChance < MinMutationChance)
                MutationChance = MinMutationChance;

            if (Util.Rnd.NextDouble() < MutationChance)
                MutationAmount += (float)Util.Rnd.NextDouble() * 2 * MutationAmount - MutationAmount;
            if (MutationAmount < MinMutationAmount)
                MutationAmount = MinMutationAmount;

            if (Util.Rnd.NextDouble() < MutationChance)
                Value += (float)Util.Rnd.NextDouble() * 2 * MutationAmount - MutationAmount;
        }
	}
}

