using System;
namespace AssemblyCSharp.NeuronalNetwork
{
	public class ActionVector
	{
		public bool Move {
			get;
			private set;
		}

		public float DirX {
			get;
			private set;
		}

		public float DirY {
			get;
			private set;
		}

		public bool[] Switch {
			get;
			private set;
		}

		public ActionVector (WebLayer outputLayer)
		{
			if (outputLayer == null)		// TODO: make this check obsolete - outputLayer should never be null!
				return;

			Switch = new bool[outputLayer.Neurons.Length - 3];

			int i = 0;
			foreach(var s in Switch)
				Switch[i] = outputLayer.Neurons [i++].GetValue () > 0;
			Move = outputLayer.Neurons [i++].GetValue() > 0;
			DirX = outputLayer.Neurons [i++].GetValue ();	
			DirY = outputLayer.Neurons [i++].GetValue ();
		}
	}
}

