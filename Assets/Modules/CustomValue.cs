// credits to http://vbcity.com/blogs/jatkinson/archive/2010/01/12/create-custom-types-and-initialize-them-without-the-new-keyword-c-vb-net.aspx

namespace EvoMotion2D.Modules
{
    public struct CustomValue
    {
        private float customValue;
        private float mutationChance;
        private float mutationAmount;

        private CustomValue(float value)
        {
            customValue = value;
            mutationChance = Parameters.StaticMinMutationChance;
            mutationAmount = Parameters.StaticMinMutationAmount;
        }

        private CustomValue(float value, float chance, float amount)
        {
            customValue = value;
            mutationChance = chance;
            mutationAmount = amount;
        }

        public static float Random()
        {
            return Util.SignedRange(Parameters.StaticInitialValueRange);
        }

        public CustomValue Clone()
        {
            float newCustomValue = customValue;
            float newMutationChance = mutationChance;
            float newMutationAmount = mutationAmount;

            if (Util.Rnd.NextDouble() < newMutationChance)
                newMutationChance += (float)Util.Rnd.NextDouble() * 2 * newMutationAmount - newMutationAmount;
            if (newMutationChance < Parameters.StaticMinMutationChance || newMutationChance > Parameters.StaticMaxMutationChance)
                newMutationChance = Parameters.StaticMinMutationChance;


            if (Util.Rnd.NextDouble() < newMutationChance)
                newMutationAmount += (float)Util.Rnd.NextDouble() * 2 * newMutationAmount - newMutationAmount;
            if (newMutationAmount < Parameters.StaticMinMutationAmount || newMutationChance > Parameters.StaticMaxMutationAmount)
                newMutationAmount = Parameters.StaticMinMutationAmount;

            if (Util.Rnd.NextDouble() < newMutationChance)
                customValue += (float)Util.Rnd.NextDouble() * 2 * newMutationAmount - newMutationAmount;

            return new CustomValue(newCustomValue, newMutationChance, newMutationAmount);
        }

        #region arithmetic operations

        public static implicit operator CustomValue(float value)
        {
            return new CustomValue(value);
        }

        public static CustomValue operator +(CustomValue a, CustomValue b)
        {
            return a.customValue + b.customValue;
        }

        public static CustomValue operator -(CustomValue a, CustomValue b)
        {
            return a.customValue - b.customValue;
        }

        public static CustomValue operator *(CustomValue a, CustomValue b)
        {
            return a.customValue * b.customValue;
        }

        public static CustomValue operator /(CustomValue a, CustomValue b)
        {
            return a.customValue / b.customValue;
        }

        #endregion

        #region type conversions

        public static implicit operator double(CustomValue value)
        {
            return (double)value.customValue;
        }

        public static implicit operator float(CustomValue value)
        {
            return (float)value.customValue;
        }

        public static implicit operator int(CustomValue value)
        {
            return (int)value.customValue;
        }

        #endregion

        public override string ToString()
        {
            return customValue.ToString();
        }
    }
}
