using UnityEngine;
using System.Collections;

namespace EvoMotion2D
{
    public class InitialParameters : MonoBehaviour
    {

        [Range(0.01f, 100f)]
        public float InitialValueRange;

        [Range(0.000001f, 1f)]
        public float MinMutationChance;
        [Range(0.000001f, 1f)]
        public float MaxMutationChance;

        [Range(0.000001f, 1f)]
        public float MinMutationAmount;
        [Range(0.000001f, 1f)]
        public float MaxMutationAmount;

        public static float StaticInitialValueRange, 
            StaticMinMutationChance, StaticMaxMutationChance,
            StaticMinMutationAmount, StaticMaxMutationAmount;

        //  taking parameters from Unity Editor and providing them as static variables to other scripts
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