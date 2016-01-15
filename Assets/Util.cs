using System;
namespace AssemblyCSharp
{
	public static class Util
	{
		private static Random rnd;
		public static Random Rnd {
			get 
			{
				if (rnd == null)
					rnd = new Random();
				return rnd;
			}
		}

		public static float SignedRange(float unsignedRange) {
			return (float)Rnd.NextDouble() * 2 * unsignedRange - unsignedRange;
		}

        public static float UnsignedRange(float SignedRange)
        {
            return (float)Rnd.NextDouble() * SignedRange;
        }

        public static float Clamp(float x) {
			return (float)Math.Tanh (x);
		}

		public static string CreatePassword(int length)
		{
			const string valid = "ABCDEF1234567890";
			var res = new System.Text.StringBuilder();
			Random rnd = new Random();
			while (0 < length--)
			{
				res.Append(valid[rnd.Next(valid.Length)]);
			}
			return res.ToString();
		}
	}
}

