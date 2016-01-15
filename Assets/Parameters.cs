using UnityEngine;
using System.Collections;

namespace EvoMotion2D
{
    public class Parameters : MonoBehaviour
    {

        [Range(0.01f, 100f)]
        public float InitialValueRange = 10f;

        [Range(0.000001f, 1f)]
        public float MinMutationChance = 0.001f;
        [Range(0.000001f, 1f)]
        public float MaxMutationChance = 0.001f;

        [Range(0.000001f, 1f)]
        public float MinMutationAmount = 0.001f;
        [Range(0.000001f, 1f)]
        public float MaxMutationAmount = 0.001f;

        public static float StaticInitialValueRange, 
            StaticMinMutationChance, StaticMaxMutationChance,
            StaticMinMutationAmount, StaticMaxMutationAmount;

        void Start()
        {
            StaticInitialValueRange = InitialValueRange;

            if (MinMutationChance > MaxMutationChance || MinMutationAmount > MaxMutationAmount)
                throw new System.ArgumentException();

            StaticMinMutationChance = MinMutationChance;
            StaticMaxMutationChance = MaxMutationChance;

            StaticMinMutationAmount = MinMutationAmount;
            StaticMaxMutationAmount = MaxMutationAmount;
        }
    }
}