using System;
namespace AssemblyCSharp.NeuronalNetwork
{
	public class InputLayer : NeuronLayer
	{
		/*
		 *	1	sensor 1 target dmass	// mass difference
		 * 	2	sensor 1 target dx		// x difference
		 * 	3	sensor 1 target dy		// y difference
		 * 	4 	sensor 1 candid	dmass	
		 * 	5 	sensor 1 candid	dx
		 * 	6 	sensor 1 candid dy
		 * 	
		 *  n = 6 * sensorCount
		 * 
		 */

		public InputLayer (UnityEngine.GameObject sensorContainer, int index)
		{
			Index = index;
			Neurons = new InputNeuron[6 * sensorContainer.transform.childCount];

			int n = 0;
			int i = 0;
			foreach (var sensor in sensorContainer.GetComponentsInChildren<Sensor>()) {
				Neurons[n++] = new InputNeuron(sensor, i, InputNeuron.Channel.TargetMASS, index, this);
				Neurons[n++] = new InputNeuron(sensor, i, InputNeuron.Channel.CandidateMASS, index, this);
				Neurons[n++] = new InputNeuron(sensor, i, InputNeuron.Channel.TargetDIFX, index, this);
				Neurons[n++] = new InputNeuron(sensor, i, InputNeuron.Channel.CandidateDIFX, index, this);
				Neurons[n++] = new InputNeuron(sensor, i, InputNeuron.Channel.TargetDIFY, index, this);
				Neurons[n++] = new InputNeuron(sensor, i, InputNeuron.Channel.CandidateDIFY, index, this);
				i++;
			}
			//UnityEngine.Debug.Log ("InputLayer ctor: " + Neurons.Length + " Neurons created");
		}

		public override string ToString ()
		{
			return "Input Layer " + Index + " (" + Neurons.Length + " neurons) ";
		}
	}
}

