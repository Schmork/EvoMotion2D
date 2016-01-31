using UnityEngine;

namespace EvoMotion2D.Cell
{
    class DragAdjuster : MonoBehaviour
    {
        CellHandler ch;
        Rigidbody2D rb;

        void Start()
        {
            ch = GetComponent<CellHandler>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            var fattieBonus = 0f;
            if (ch.Mass > 1) fattieBonus = Mathf.Sqrt(ch.Mass / 10f);

            rb.drag = rb.velocity.magnitude - fattieBonus;

            var minDrag = 0.04f;
            if (rb.drag < minDrag) rb.drag = minDrag;
        }
    }
}
