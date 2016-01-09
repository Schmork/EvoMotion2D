using System;
using System.Collections.Generic;

namespace AssemblyCSharp.NeuronalNetwork
{
	public class WebNeuron : Neuron
	{
		int range = 10;
		
		public float Offset;
		public float Factor;
		
		public List<Neuron> inNeurons = new List<Neuron>();
		
		public WebNeuron (int index, NeuronLayer layer)
		{
			//UnityEngine.Debug.Log("WebNeuron ctor1 - index " + index);
			
			Layer = layer;
			Index = index;
			Offset = (float)Util.Rnd.NextDouble() * 2 * range - range;			
			Factor = (float)Util.Rnd.NextDouble() * 2 * range - range;
		}
		
		public WebNeuron (Neuron paragon, Brain brain) {
			//UnityEngine.Debug.Log("WebNeuron ctor2");

			var webNeuron = paragon as WebNeuron;

			if (webNeuron != null) {
				Offset = webNeuron.Offset;
				brain.MutateValue(ref Offset);

				Factor = webNeuron.Factor;
				brain.MutateValue(ref Factor);
			}
		}
		
		public override float GetValue() {			
			//UnityEngine.Debug.Log(("WebNeuron GetValue() of " + inNeurons.Count + " neurons : " + ToString()));

			float sum = 0;
			foreach (var inNeuron in inNeurons) {
				//UnityEngine.Debug.Log("Neuron " + ToString() + " collects " + x + " from " + inNeuron.ToString());
				sum += inNeuron.GetValue();
			}
			return Util.Clamp (sum * Factor + Offset);
		}
	}
}
