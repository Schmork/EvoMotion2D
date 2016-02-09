// To prevent the cells from running away in all directions, an invisible pull towards the middle is applied.


using UnityEngine;
using System.Collections.Generic;
using System;

namespace EvoMotion2D.Cell
{
    public class StayInBounds : MonoBehaviour
    {

        [Range(0, 500)]
        public float MaxDistance;

        public float ForceFactor;
        public float minForce;

        // Update is called once per frame
        void Update()
        {
            foreach (Transform t in transform)        // get all children aka all cells
            {
                var d = Vector2.Distance(Vector2.zero, t.position);
                if (d > MaxDistance)
                {
                    var rb2d = t.GetComponent<Rigidbody2D>();
                    var force = rb2d.mass * Time.deltaTime * ForceFactor;
                    var actualMinForce = Time.deltaTime * minForce * rb2d.mass;
                    if (force < actualMinForce) force = actualMinForce;
                    rb2d.AddForce( - t.position * force);
                }
            }
        }
    }
}