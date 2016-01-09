using System;
using System.Collections.Generic;

namespace AssemblyCSharp.NeuronalNetwork
{
	public abstract class Neuron {
		public int Index = -333;
		public NeuronLayer Layer;

		public abstract float GetValue();

		public override string ToString ()
		{
			return Layer.ToString() +  ", Neuron " + Index;
		}
	}
}

