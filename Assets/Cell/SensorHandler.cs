using UnityEngine;
using System.Collections;
using AssemblyCSharp.Modules;
using System.Collections.Generic;

public class SensorHandler : MonoBehaviour {

	public GameObject SensorContainer, SensorPrefab;

	[Range(0, 23)]
	public int NumberOfSensors;

	void Awake () {
		while(SensorContainer.transform.childCount < NumberOfSensors) {
			var sensor = (GameObject)GameObject.Instantiate(SensorPrefab, transform.position, Quaternion.identity);
			sensor.transform.parent = SensorContainer.transform;
		}
	}

    public Sensor[] getSensors()
    {
        return SensorContainer.GetComponentsInChildren<Sensor>();
    }
}
