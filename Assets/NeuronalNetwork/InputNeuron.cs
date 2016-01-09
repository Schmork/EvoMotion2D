using System;
using UnityEngine;

namespace AssemblyCSharp.NeuronalNetwork
{
	public class InputNeuron : Neuron
	{
		Sensor sensor;
		Rigidbody2D body;

		public enum Channel
		{
			TargetMASS,
			TargetDIFX,
			TargetDIFY,
			CandidateMASS,
			CandidateDIFX,
			CandidateDIFY
		}

		public Channel channel {
			get;
			private set;
		}

		public InputNeuron (Sensor sensor, int index, Channel channel, int number, NeuronLayer layer)
		{
			Index = number;
			Layer = layer;
			this.sensor = sensor;
			this.channel = channel;

			//UnityEngine.Debug.Log ("InputNeuron ctor 1, sensor == null? " + (sensor == null));
		}

		public InputNeuron (GameObject sensorContainer, InputNeuron parent)
		{
			var index = parent.Index;
			var sensor = sensorContainer.transform.GetChild (index);

			//UnityEngine.Debug.Log ("InputNeuron: sensorContainer child count = " + sensorContainer.transform.childCount);

			this.sensor = sensor.GetComponent<Sensor>();
			this.channel = parent.channel;
			this.Index = parent.Index;

			//UnityEngine.Debug.Log ("InputNeuron ctor 2, sensor == null? " + (sensor == null));
		}

		public override float GetValue ()
		{
			//UnityEngine.Debug.Log (channel);
			var me = sensor.transform.parent.parent;
			var tar = sensor.target;
			var can = sensor.candidate;
				return 0f;
			
			if (tar == null && can == null)
				return 0f;

			if (tar != null) {
				if (channel == Channel.TargetDIFX)
					return tar.transform.position.x - me.transform.position.x;
				if (channel == Channel.TargetDIFY)
					return tar.transform.position.y - me.transform.position.y;
				if (channel == Channel.TargetMASS)
					return tar.GetComponent<Rigidbody2D> ().mass - me.GetComponent<Rigidbody2D> ().mass;
			}

			if (can != null) {
				if (channel == Channel.CandidateDIFX)
					return can.transform.position.x - me.transform.position.x;
				if (channel == Channel.CandidateDIFY)
					return can.transform.position.y - me.transform.position.y;
				if (channel == Channel.CandidateMASS)
					return can.GetComponent<Rigidbody2D> ().mass - me.GetComponent<Rigidbody2D> ().mass;
			}

			throw new NotSupportedException("Input Neurons must have either channel MASS, DIFX or DIFY for 'Target' or 'Candidate'. This one has neither.");
		}
	}
}


