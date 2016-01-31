using System;

namespace EvoMotion2D
{
	public static class Util
	{
		public static string CreatePassword(int length)
		{
            const string valid = "abcdefghikmpqrstuvxyz1234567890";
			var res = new System.Text.StringBuilder();
			while (0 < length--)
			{
                var index = (int)(UnityEngine.Random.value * valid.Length);
                res.Append(valid[index]);
			}
			return res.ToString();
		}
	}
}

