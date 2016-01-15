using UnityEngine;
using System.Collections;
using EvoMotion2D.Modules;
using System.Collections.Generic;

namespace EvoMotion2D.Cell
{
    public class SensorHandler : MonoBehaviour
    {

        public GameObject SensorContainer, SensorPrefab;

        [Range(0, 23)]
        public int NumberOfSensors;

        void Awake()
        {
            while (SensorContainer.transform.childCount < NumberOfSensors)
            {
                var sensor = (GameObject)GameObject.Instantiate(SensorPrefab, transform.position, Quaternion.identity);
                sensor.transform.parent = SensorContainer.transform;
            }
        }

        public Sensor[] getSensors()
        {
            return SensorContainer.GetComponentsInChildren<Sensor>();
        }
    }
}