using UnityEngine;

namespace AssemblyCSharp.Modules
{
    public class Sensor : MonoBehaviour
    {
        public float UsageFee;
        public GameObject target, candidate;

        public Parameter ScanChance = new Parameter(true);
        public Parameter ScanMaxRange = new Parameter(true);
        public Parameter PreyFactor = new Parameter(false);
        public Parameter MaxFleeDistance = new Parameter(true);

        public void SwitchToCandidate()
        {
            target = candidate;
            candidate = null;
        }

        void Update()
        {
            var myMass = GetComponentInParent<Rigidbody2D>().mass;

            if (Random.value < ScanChance.Value)
            {
                GetComponentInParent<CellHandler>().Mass -= UsageFee;

                var dir = Random.insideUnitCircle.normalized;
                var start = (Vector2)transform.position;

                RaycastHit2D hit = Physics2D.Raycast(start, dir, ScanMaxRange.Value);
                if (hit.collider != null && hit.collider.tag == "Cell")
                {
                    candidate = hit.collider.gameObject;

                    if (name == candidate.name)
                        return;

                    var yourMass = candidate.GetComponent<Rigidbody2D>().mass;
                    if (yourMass < myMass && yourMass > myMass / PreyFactor.Value)
                        return;

                    if (target == null)
                    {
                        target = candidate;
                        return;
                    }

                    if (getScore(candidate) > getScore(target))
                        target = candidate;
                }
            }

            if (target != null) {
                if (name == target.name) {		// TODO: make this dirty fix obsolete
                    SwitchToCandidate();
                } else if (myMass < target.GetComponent<Rigidbody2D> ().mass
                    && Vector2.Distance (transform.position, target.transform.position) > MaxFleeDistance.Value) {
                    SwitchToCandidate();
                }/* else if (myMass < target.GetComponent<Rigidbody2D> ().mass
                    && name.Contains(target.name))
                    SwitchToCandidate();*/
            }

            if (object.Equals (target, null))
                SwitchToCandidate();
        }

        float getScore(GameObject cell)
        {
            var dist = Vector2.Distance(transform.position, cell.transform.position);
            var score = cell.GetComponent<Rigidbody2D>().mass / (dist * dist);
            return score;
        }
    }
}