using UnityEngine;
using System.Collections;

public class SensorHandler : MonoBehaviour {

	public GameObject SensorContainer, SensorPrefab;

	[Range(0, 23)]
	public int NumberOfSensors;

	void Start () {
		int i = 0;
		while(SensorContainer.transform.childCount < NumberOfSensors) {
			var sensor = (GameObject)GameObject.Instantiate(SensorPrefab, transform.position, Quaternion.identity);
			sensor.transform.parent = SensorContainer.transform;
		}
	}
}
