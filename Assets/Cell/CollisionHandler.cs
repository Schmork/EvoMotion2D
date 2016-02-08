using UnityEngine;

namespace EvoMotion2D.Cell
{
    public class CollisionHandler : MonoBehaviour
    {
        Rigidbody2D myRb;
        CellHandler myCh;
        SpriteRenderer mySr;

		public float RejuvenationFactor;		// age is reduced by collectedMass * this factor

        void Awake()
        {
            myRb = GetComponent<Rigidbody2D>();
            myCh = GetComponent<CellHandler>();
            mySr = GetComponent<SpriteRenderer>();
        }

        void OnTriggerStay2D(Collider2D coll)
        {
            var other = coll.gameObject;
            if (other.tag != "Cell")
                return;
            
            var yourRb = other.GetComponent<Rigidbody2D>();

            if (myRb.mass > yourRb.mass)
            {
                var yourCh = other.GetComponent<CellHandler>();
                
                var transferredMass = myCh.Radius + yourCh.Radius - Vector2.Distance(transform.position, other.transform.position) + 0.02f;
                transferredMass *= 0.01f;
                
                if (transferredMass < 0) return;
                if (transferredMass > yourRb.mass)
                {
                    transferredMass = yourRb.mass;
                }

                yourRb.mass -= transferredMass;
                myRb.mass += transferredMass;

                myCh.CollectedMass += transferredMass;
				myCh.Age -= (int)(transferredMass * RejuvenationFactor);

                var myColor = mySr.color;
                var yourColor = other.GetComponent<SpriteRenderer>().color;
                var colorFactor = transferredMass / myRb.mass / 2f;

                var red = yourColor.r - myColor.r;
                var green = yourColor.g - myColor.g;
                var blue = yourColor.b - myColor.b;

                var newColor = new Color(myColor.r + red * colorFactor,
                                     myColor.g + green * colorFactor,
                                     myColor.b + blue * colorFactor, 1);
                mySr.color = newColor;

                if (myRb.mass > Shrinker.StaticMinMass)
                    GetComponent<Controller>().Activate();
            }
        }
    }
}