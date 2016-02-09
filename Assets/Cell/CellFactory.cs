// this class creates new cells.
// there are two scenarios how this can happen: 
//  1) random new creation at start or during runtime to replace dead cells
//  2) as thrust to propel another cell forward
// in the latter case, mutation to the cell's parameters is applied.


using UnityEngine;
using EvoMotion2D.Cell;
using System.Collections.Generic;

namespace EvoMotion2D
{
    public class CellFactory : MonoBehaviour
    {
        public float ColorDrift;
        public static float colorDrift;

        public float SpawnRadius;
        public static float spawnRadius;

        public List<Sprite> Images;
        public static List<Sprite> images;

        static List<string> names = new List<string>();

        void Start()
        {
            colorDrift = ColorDrift;
            spawnRadius = SpawnRadius;
            images = Images;
            readNamesFromFile();
        }

        void readNamesFromFile()
        {
            var dir = System.IO.Directory.GetCurrentDirectory();
            var file = new System.IO.StreamReader(dir + "/Assets/Cell/names.txt");
            string line;
            while ((line = file.ReadLine()) != null) names.Add(line);
            file.Close();
        }

        public static GameObject Spawn(GameObject cell, GameObject spawnArea, float mass)
        {

            var point = Random.insideUnitCircle * spawnRadius;

            var obstacle = Physics2D.OverlapCircle(point, CellHandler.getRadius(mass * 2f));
            if (obstacle) return null;

            var spawn = Instantiate(cell);

            spawn.transform.position = point;
            spawn.transform.rotation = new Quaternion(0, 0, Random.value, Random.value);
            spawn.GetComponent<CellHandler>().Mass = mass;
            spawn.GetComponent<CellHandler>().Generation = 0;
            spawn.name = names[(int)(Random.value * names.Count)] + "-" + (int)Random.value % 10;
            spawn.transform.parent = spawnArea.transform;

            var spriteRenderer = spawn.GetComponent<SpriteRenderer>();

            spriteRenderer.color = new Color(Random.Range(0.3f, 1),
                                              Random.Range(0.3f, 1),
                                              Random.Range(0.3f, 1));
            //spriteRenderer.sprite = images [(int)(Random.value * images.Count)];
            
            return spawn;
        }

        public static GameObject Thrust(GameObject cell, float mass)
        {
            var thrust = Instantiate(cell);
            var tCh = thrust.GetComponent<CellHandler>();
            var ch = cell.GetComponent<CellHandler>();
            ch.Children++;
            tCh.Children = 0;

            tCh.CollectedMass = 0;
            tCh.Generation = ch.Generation + 1;

            tCh.Mass = mass;
            ch.Mass -= mass;

            var extraSpace = 1.01f;
            Vector2 thrustDirection = -cell.GetComponent<Rigidbody2D>().transform.right;
            var distance = thrustDirection * (ch.Radius + tCh.Radius) * extraSpace;

            tCh.Pos = ch.Pos + distance;
            tCh.Vel = ch.Vel;

            var rotationRange = 15f;     // thrust ojects divert by +/- this value from their parent's rotation
            var rotationVariance = Random.Range(-rotationRange, rotationRange);
            thrust.transform.Rotate(new Vector3(0, 0, 180 - rotationVariance));

            thrustDirection *= 4f * ch.Mass;// * Time.deltaTime;  // adjust force
            var factorThrustFasterThanCell = 2.5f;

            cell.GetComponent<Rigidbody2D>().AddForce(-thrustDirection);
            thrust.GetComponent<Rigidbody2D>().AddForce(thrustDirection * mass / ch.Mass * factorThrustFasterThanCell);

            thrust.transform.parent = cell.transform.parent;
            thrust.name = cell.name + randomChar();

            var parentColor = ch.Color;
            var drift = Random.insideUnitSphere * colorDrift;    // slight change in color for children

            var childColor = new Color(parentColor.r + drift.x,
                                    parentColor.g + drift.y,
                                    parentColor.b + drift.z);

            tCh.Color = childColor;

            mutateParameters(cell, thrust);

            return thrust;
        }

        static void mutateParameters(GameObject parent, GameObject child)
        {
            var cSensorHandler = child.GetComponentInChildren<SensorHandler>();
            var pSensorHandler = parent.GetComponentInChildren<SensorHandler>();

            for (int i = 0; i < pSensorHandler.NumberOfSensors; i++)
            {
                var tSensor = cSensorHandler.getSensors()[i];
                var cSensor = pSensorHandler.getSensors()[i];

                tSensor.MaxFleeDistance = cSensor.MaxFleeDistance.Mutate();
                tSensor.PreyFactor = cSensor.PreyFactor.Mutate();
                tSensor.ScanChance = cSensor.ScanChance.Mutate();
                tSensor.ScanMaxRange = cSensor.ScanMaxRange.Mutate();
                tSensor.ImportanceFactor = cSensor.ImportanceFactor.Mutate();

                tSensor.WhatToWatch = cSensor.WhatToWatch;
            }

            var tController = child.GetComponent<Controller>();
            var cController = parent.GetComponent<Controller>();

            tController.Cooldown = cController.Cooldown.Mutate();
            tController.MaxPreyAngle = cController.MaxPreyAngle.Mutate();
            tController.MaxPredatorAngle = cController.MaxPredatorAngle.Mutate();
            tController.MinTurnForce = cController.MinTurnForce.Mutate();
            tController.MaxTurnForce = cController.MaxTurnForce.Mutate();
            child.GetComponent<Thruster>().ThrustToMassRatio = parent.GetComponent<Thruster>().ThrustToMassRatio.Mutate();
        }

        static char randomChar()
        {
            const string valid = "abcdefghikmpqrstuvxyz1234567890";
            var index = (int)(UnityEngine.Random.value * valid.Length);

            return valid[index];
        }
    }
}