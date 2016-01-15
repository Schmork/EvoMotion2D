using UnityEngine;

namespace EvoMotion2D.Cell
{
    public class CollisionHandler : MonoBehaviour
    {
        void OnTriggerStay2D(Collider2D coll)
        {
            var other = coll.gameObject;
            if (other.tag != "Cell")
                return;

            var myRb = GetComponent<Rigidbody2D>();
            var yourRb = other.GetComponent<Rigidbody2D>();

            if (myRb.mass > yourRb.mass)
            {

                var myCh = GetComponent<CellHandler>();
                var yourCh = other.GetComponent<CellHandler>();

                var myColor = GetComponent<SpriteRenderer>().color;
                var yourColor = other.GetComponent<SpriteRenderer>().color;

                var transferredMass = myCh.Radius + yourCh.Radius - Vector2.Distance(transform.position, other.transform.position);
                transferredMass /= 4f;
                var colorFactor = transferredMass / myRb.mass / 2f;

                if (transferredMass < 0) return;
                if (transferredMass > yourRb.mass)
                {
                    transferredMass = yourRb.mass;
                }

                yourRb.mass -= transferredMass;
                myRb.mass += transferredMass;

                var red = yourColor.r - myColor.r;
                var green = yourColor.g - myColor.g;
                var blue = yourColor.b - myColor.b;

                var newColor = new Color(myColor.r + red * colorFactor,
                                     myColor.g + green * colorFactor,
                                     myColor.b + blue * colorFactor, 1);
                GetComponent<SpriteRenderer>().color = newColor;

                if (myRb.mass > Shrinker.StaticMinMass)
                    GetComponent<Controller>().enabled = true;
            }
        }
    }
}