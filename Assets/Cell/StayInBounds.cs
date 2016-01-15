using UnityEngine;

namespace EvoMotion2D.Cell
{
    public class StayInBounds : MonoBehaviour
    {

        [Range(0, 500)]
        public float Width, Height;

        // Update is called once per frame
        void Update()
        {

            float dist = Vector2.Distance(transform.position, Vector2.zero);
            if (dist > 30)
            {
                var dir = transform.position.normalized;
                var mag = transform.position.magnitude / 10f;
                if (mag > 1) mag = Mathf.Pow(mag, 0.3f);
                GetComponent<Rigidbody2D>().AddForce(-dir * mag);
            }
        }
    }
}