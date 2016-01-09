using System;
namespace AssemblyCSharp.NeuronalNetwork
{
	public class WebLayer : NeuronLayer
	{
		public WebLayer (int count, int index)
		{
			Index = index;

			Neurons = new WebNeuron[count];

			for (int i = 0; i < count; i++) {
				Neurons[i] = new WebNeuron(i, this);
			}
			//UnityEngine.Debug.Log ("WebLayer ctor: " + Neurons.Length + " Neurons created");
		}
	}
}

