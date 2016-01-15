namespace EvoMotion2D.Modules
{
    public struct SignedMutateableFloat
    {
        private float value;
        private float mutationChance;
        private float mutationAmount;

        private SignedMutateableFloat(float val)
        {
            value = val;

            mutationChance = UnityEngine.Random.Range(
                Parameters.StaticMinMutationChance, 
                Parameters.StaticMaxMutationChance);

            mutationAmount = UnityEngine.Random.Range(
                Parameters.StaticMinMutationAmount, 
                Parameters.StaticMaxMutationAmount);
        }

        private SignedMutateableFloat(float val, float chance, float amount)
        {
            value = val;
            mutationChance = chance;
            mutationAmount = amount;
        }

        public static float Random()
        {
            return UnityEngine.Random.Range(
                -Parameters.StaticInitialValueRange,            // this is where it differs from UnsignedMutateableFloat
                +Parameters.StaticInitialValueRange);
        }

        public SignedMutateableFloat Clone()
        {
            float newCustomValue = value;
            float newMutationChance = mutationChance;
            float newMutationAmount = mutationAmount;

            if (Util.Rnd.NextDouble() < newMutationChance)
                newMutationChance += UnityEngine.Random.Range(-newMutationAmount, +newMutationAmount);
            if (newMutationChance < Parameters.StaticMinMutationChance || newMutationChance > Parameters.StaticMaxMutationChance)
                newMutationChance = Parameters.StaticMinMutationChance;


            if (Util.Rnd.NextDouble() < newMutationChance)
                newMutationAmount += UnityEngine.Random.Range(-newMutationAmount, +newMutationAmount);
            if (newMutationAmount < Parameters.StaticMinMutationAmount || newMutationChance > Parameters.StaticMaxMutationAmount)
                newMutationAmount = Parameters.StaticMinMutationAmount;

            if (Util.Rnd.NextDouble() < newMutationChance)
                value += UnityEngine.Random.Range(-newMutationAmount, +newMutationAmount);

            return new SignedMutateableFloat(newCustomValue, newMutationChance, newMutationAmount);
        }

        #region arithmetic operations

        public static implicit operator SignedMutateableFloat(float value)
        {
            return new SignedMutateableFloat(value);
        }

        public static SignedMutateableFloat operator +(SignedMutateableFloat a, SignedMutateableFloat b)
        {
            return a.value + b.value;
        }

        public static SignedMutateableFloat operator -(SignedMutateableFloat a, SignedMutateableFloat b)
        {
            return a.value - b.value;
        }

        public static SignedMutateableFloat operator *(SignedMutateableFloat a, SignedMutateableFloat b)
        {
            return a.value * b.value;
        }

        public static SignedMutateableFloat operator /(SignedMutateableFloat a, SignedMutateableFloat b)
        {
            return a.value / b.value;
        }

        #endregion

        #region type conversions

        public static implicit operator double(SignedMutateableFloat smf)
        {
            return (double)smf.value;
        }

        public static implicit operator float(SignedMutateableFloat smf)
        {
            return (float)smf.value;
        }

        public static implicit operator int(SignedMutateableFloat smf)
        {
            return (int)smf.value;
        }

        #endregion

        public override string ToString()
        {
            return value.ToString();
        }

    }
}
