namespace EvoMotion2D.Parameters
{
	public interface IMutateableParameter
	{
        IMutateableParameter Random();

        IMutateableParameter Mutate();
	}
}

