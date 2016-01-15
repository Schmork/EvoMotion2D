namespace EvoMotion2D.Modules
{
    public struct UnsignedMutateableFloat
    {
        private float value;
        private float mutationChance;
        private float mutationAmount;

        private UnsignedMutateableFloat(float val)
        {
            value = val;

            mutationChance = UnityEngine.Random.Range(
                Parameters.StaticMinMutationChance,
                Parameters.StaticMaxMutationChance);

            mutationAmount = UnityEngine.Random.Range(
                Parameters.StaticMinMutationAmount,
                Parameters.StaticMaxMutationAmount);
        }

        private UnsignedMutateableFloat(float val, float chance, float amount)
        {
            value = val;
            mutationChance = chance;
            mutationAmount = amount;
        }

        public static float Random()
        {
            return UnityEngine.Random.Range(
                0,                                          // this is where it differs from SignedMutateableFloat
                +Parameters.StaticInitialValueRange);
        }

        public UnsignedMutateableFloat Clone()
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
            if (value < 0) value = 0;

            return new UnsignedMutateableFloat(newCustomValue, newMutationChance, newMutationAmount);
        }

        #region arithmetic operations

        public static implicit operator UnsignedMutateableFloat(float value)
        {
            return new UnsignedMutateableFloat(value);
        }

        public static UnsignedMutateableFloat operator +(UnsignedMutateableFloat a, UnsignedMutateableFloat b)
        {
            return a.value + b.value;
        }

        public static UnsignedMutateableFloat operator -(UnsignedMutateableFloat a, UnsignedMutateableFloat b)
        {
            return a.value - b.value;
        }

        public static UnsignedMutateableFloat operator *(UnsignedMutateableFloat a, UnsignedMutateableFloat b)
        {
            return a.value * b.value;
        }

        public static UnsignedMutateableFloat operator /(UnsignedMutateableFloat a, UnsignedMutateableFloat b)
        {
            return a.value / b.value;
        }

        #endregion

        #region type conversions

        public static implicit operator double(UnsignedMutateableFloat umf)
        {
            return (double)umf.value;
        }

        public static implicit operator float(UnsignedMutateableFloat umf)
        {
            return (float)umf.value;
        }

        public static implicit operator int(UnsignedMutateableFloat umf)
        {
            return (int)umf.value;
        }

        #endregion

        public override string ToString()
        {
            return value.ToString();
        }

    }
}
