using System;
using System.Collections.Generic;
namespace AssemblyCSharp.NeuronalNetwork
{
	public class NeuronLayer
	{
		public int Index = -999;
		public Neuron[] Neurons;

		public override String ToString() {
			return "Layer " + Index + " (" + Neurons.Length + " neurons) ";
		}
	}
}

