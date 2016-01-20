using System;

namespace EvoMotion2D.Parameters
{
	public class MutateableParameter
	{
        public float Value { get; set; }
        public float MutationChance { get; set; }
        public float MutationAmount { get; set; }

        public MutateableParameter(float value)
		{
            Value = value;
            MutationChance = UnityEngine.Random.Range(
                InitialParameters.StaticMinMutationChance,
                InitialParameters.StaticMaxMutationChance);

            MutationAmount = UnityEngine.Random.Range(
                InitialParameters.StaticMinMutationAmount,
                InitialParameters.StaticMaxMutationAmount);
        }

        private MutateableParameter(float value, float chance, float amount)
        {
            Value = value;
            MutationChance = chance;
            MutationAmount = amount;
        }

		public MutateableParameter Mutate ()
		{
            float newValue = Value;
            float newMutationChance = MutationChance;
            float newMutationAmount = MutationAmount;

            if (UnityEngine.Random.value < MutationChance)
                newMutationChance += UnityEngine.Random.Range(-newMutationAmount, +newMutationAmount);
            if (newMutationChance < InitialParameters.StaticMinMutationChance)
                newMutationChance = InitialParameters.StaticMinMutationChance;
            if (newMutationChance > InitialParameters.StaticMaxMutationChance)
                newMutationChance = InitialParameters.StaticMaxMutationChance;

            if (UnityEngine.Random.value < MutationChance)
                newMutationAmount += UnityEngine.Random.Range(-newMutationAmount, +newMutationAmount);
            if (newMutationAmount < InitialParameters.StaticMinMutationAmount)
                newMutationAmount = InitialParameters.StaticMinMutationAmount;
            if (newMutationAmount > InitialParameters.StaticMaxMutationAmount)
                newMutationAmount = InitialParameters.StaticMaxMutationAmount;

            if (UnityEngine.Random.value < MutationChance)
                newValue += UnityEngine.Random.Range(-newMutationAmount, +newMutationAmount);

            return new MutateableParameter(newValue, newMutationChance, newMutationAmount);
        }
    }
}

