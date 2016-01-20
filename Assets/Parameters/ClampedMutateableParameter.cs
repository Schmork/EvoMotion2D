namespace EvoMotion2D.Parameters
{
    public class ClampedMutateableParameter : MutateableParameter
    {
        float min, max;

        public ClampedMutateableParameter (float min, float max) : base(UnityEngine.Random.Range(min, max))
        {
            this.min = min;
            this.max = max;            
        }

        public new ClampedMutateableParameter Mutate()
        {
            var mp = base.Mutate();
            if (mp.Value < min) mp.Value = min;
            if (mp.Value > max) mp.Value = max;

            var cmp = new ClampedMutateableParameter(min, max);
            cmp.Value = mp.Value;
            cmp.MutationChance = mp.MutationChance;
            cmp.MutationAmount = mp.MutationAmount;

            return cmp;
        }
    }
}
