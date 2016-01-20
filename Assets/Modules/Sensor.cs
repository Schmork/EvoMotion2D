using UnityEngine;
using EvoMotion2D.Parameters;

namespace EvoMotion2D.Modules
{
    public class Sensor : MonoBehaviour
    {
        public float UsageFee;
        public GameObject target;
        private GameObject candidate;

        public ClampedMutateableParameter ScanChance;
        public UnsignedMutateableParameter ScanMaxRange, PreyFactor, MaxFleeDistance, ImportanceFactor;

        public WatchType WhatToWatch;

        public enum WatchType
        {
            PREY,
            PREDATOR
        }

        void Awake()
        {
            ScanChance = new ClampedMutateableParameter(0f, 1f);
            ScanMaxRange = new UnsignedMutateableParameter();
            PreyFactor = new UnsignedMutateableParameter();
            MaxFleeDistance = new UnsignedMutateableParameter();
            ImportanceFactor = new UnsignedMutateableParameter();
        }

        void SwitchToCandidate()
        {
            target = candidate;
            candidate = null;
        }

        void OnDrawGizmos()
        {
            if (target != null)
            {
                var color = GetComponentInParent<SpriteRenderer>().color;
                var arrowLenght = 0.4f;
                var direction = target.transform.position - transform.position;

                var clampDir = Vector3.ClampMagnitude(direction * arrowLenght, 4f);

                if (WhatToWatch == WatchType.PREY)
                {
                    DrawArrow.ForGizmoArrow(transform.position, clampDir, color);
                }
                if (WhatToWatch == WatchType.PREDATOR)
                {
                    DrawArrow.ForGizmoX(transform.position, clampDir, color);
                }
            }
        }

        void Update()
        {
            var myMass = GetComponentInParent<Rigidbody2D>().mass;

            if (Random.value < ScanChance.Value)
            {
                GetComponentInParent<Cell.CellHandler>().Mass -= UsageFee;

                var dir = Random.insideUnitCircle.normalized;
                var start = (Vector2)transform.position;

                RaycastHit2D hit = Physics2D.Raycast(start, dir, ScanMaxRange.Value);
                if (hit.collider != null && hit.collider.tag == "Cell")
                {
                    candidate = hit.collider.gameObject;

                    if (name == candidate.name) return;

                    var yourMass = candidate.GetComponent<Rigidbody2D>().mass;

                    if (WhatToWatch == WatchType.PREDATOR && yourMass < myMass) return;
                    if (WhatToWatch == WatchType.PREY) {
                        if (yourMass > myMass) return;
                        if (yourMass < myMass && yourMass > myMass / PreyFactor.Value) return;
                    }                    

                    if (target == null)
                    {
                        SwitchToCandidate();
                        return;
                    }

                    if (getScore(candidate) > getScore(target))
                        SwitchToCandidate();
                }
            }

            if (target != null) {
                if (name == target.name) {		// TODO: make this dirty fix obsolete
                    target = null;
                } else if (WhatToWatch == WatchType.PREDATOR
                    && myMass < target.GetComponent<Rigidbody2D> ().mass
                    && Vector2.Distance (transform.position, target.transform.position) > MaxFleeDistance.Value) {
                        target = null;
                }
            }
        }
        
        float getScore(GameObject cell)
        {
            var dist = Vector2.Distance(transform.position, cell.transform.position);
            var score = cell.GetComponent<Rigidbody2D>().mass / (dist * dist);
            return score;
        }

        public float getScore()
        {
            if (target == null) return 0f;
            return getScore(target);
        }
    }
}