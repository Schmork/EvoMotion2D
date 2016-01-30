using UnityEngine;

namespace EvoMotion2D.Cell
{
    public class StayInBounds : MonoBehaviour
    {

        [Range(0, 500)]
        public float MaxDistance;

        // Update is called once per frame
        void Update()
        {

            float dist = Vector2.Distance(transform.position, Vector2.zero);
            if (dist > MaxDistance)
            {
                var dir = transform.position.normalized;
                var mag = transform.position.magnitude / 500f;
                if (mag > 1) mag = Mathf.Pow(mag, 0.01f);
                GetComponent<Rigidbody2D>().AddForce(-dir * mag);
            }
        }
    }
}