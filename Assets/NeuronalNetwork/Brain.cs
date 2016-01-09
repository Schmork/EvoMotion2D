using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp.NeuronalNetwork
{
	public class Brain : MonoBehaviour
	{
		public Brain parent;

		public int NumberOfLayers = 3;

		public NeuronLayer[] NeuronLayers {
			get;
			set;
		}

		float mutationChance = 0.01f;
		float mutationAmount = 0.01f;
		float absoluteMin = 0.0001f;

		InputLayer inputLayer;
		WebLayer outputLayer;

		GameObject SensorContainer;

		void Start() {
			createNewRandomBrain ();
			
			if (parent != null)
				mutateExistingBrain (parent);
		}

		void createNewRandomBrain() {
			SensorContainer = transform.GetChild (0).gameObject;

			int nl = 0;		// neuron layer #
			NeuronLayers = new NeuronLayer[NumberOfLayers + 2];	// 2 extra for input & output layer

			inputLayer = new InputLayer (SensorContainer, nl);
			NeuronLayers[nl] = inputLayer;					// 0: input
			nl++;

			int i;
			for (i = 0; i < NumberOfLayers; i++) {			// 1 to n-2: web
				NeuronLayers[nl] = new WebLayer (10, nl);
				nl++;
			}
			
			outputLayer = new WebLayer (3 + SensorContainer.transform.childCount, nl);
			NeuronLayers [nl] = outputLayer;	// n-1: output

			i = nl;
			do {
				connectAtoB (NeuronLayers [i - 1], NeuronLayers [i]);
				//Debug.Log("connected layer " + (i - 1) + " to " + i);
			} while (--i > 0);
		}

		void mutateExistingBrain(Brain parent) {
			MutateValue (ref mutationChance);
			if (mutationChance < absoluteMin)
				mutationChance = absoluteMin;
			MutateValue (ref mutationAmount);
			if (mutationAmount < absoluteMin)
				mutationAmount = absoluteMin;

			for (int neuronCount = 0; neuronCount < parent.inputLayer.Neurons.Length; neuronCount++) {
				//var sensor = parent.GetComponent<Sensor>();
				var parentNeuron = (InputNeuron)parent.NeuronLayers[0].Neurons[neuronCount];
				var sensorContainer = transform.GetChild(0).gameObject;
				NeuronLayers[0].Neurons[neuronCount] = new InputNeuron(sensorContainer, parentNeuron);	
			}
			
			for (int layerCount = 1; layerCount < NeuronLayers.Length; layerCount++) {
				for (int neuronCount = 0; neuronCount < NeuronLayers[layerCount].Neurons.Length; neuronCount++) {
					var parentNeuron = parent.NeuronLayers[layerCount].Neurons[neuronCount];
					NeuronLayers[layerCount].Neurons[neuronCount] = new WebNeuron(parentNeuron, this);
				}
			}
		}

		void connectAtoB(NeuronLayer source, NeuronLayer destination) {
			foreach (Neuron sourceNeuron in source.Neurons) {
				foreach (WebNeuron destinationNeuron in destination.Neurons) {
					destinationNeuron.inNeurons.Add(sourceNeuron);
				}
			}
		}

		public ActionVector GetOutput ()
		{
			return new ActionVector (outputLayer);
		}

		public void MutateValue(ref float value) {
			if (Util.Rnd.NextDouble() < mutationChance)
				value += (float)Util.Rnd.NextDouble() * 2 * mutationAmount - mutationAmount;
		}
	}
}

