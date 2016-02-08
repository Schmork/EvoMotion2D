using UnityEngine;
using System.Linq;

namespace EvoMotion2D.Cell
{
    public class StayInBounds : MonoBehaviour
    {

        [Range(0, 500)]
        public float MaxDistance;

		float minForce = 0.25f;

        // Update is called once per frame
        void Update()
        {
			var strayOffs = CellFactory.Cells.
				Where (cell => Vector2.Distance (cell.transform.position, Vector2.zero) > MaxDistance);

			foreach (var cell in CellFactory.Cells)
				cell.GetComponent<CellHandler> ().HasStrayedOff = false;

			foreach(var cell in strayOffs)
            {
				cell.GetComponent<CellHandler>().HasStrayedOff = true;
				var t = cell.transform;
				var dir = t.position.normalized;

				var rb2d = t.GetComponent<Rigidbody2D>();
				
				var force = t.position.magnitude / 500f;
				if (force > 1) force = Mathf.Pow(force, 0.01f);
				if (force < minForce) force = minForce;
				force = minForce;

				rb2d.AddForce(-dir * force * rb2d.mass);
            }
        }
    }
}