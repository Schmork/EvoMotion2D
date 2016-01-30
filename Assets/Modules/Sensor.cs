using UnityEngine;
using EvoMotion2D.Parameters;
using EvoMotion2D.Cell;

namespace EvoMotion2D.Modules
{
    public class Sensor : MonoBehaviour
    {
        public float UsageFee;
        public GameObject target;
        private GameObject candidate, self;

        public ClampedMutateableParameter ScanChance;
        public UnsignedMutateableParameter ScanMaxRange, PreyFactor, MaxFleeDistance, ImportanceFactor;

        public WatchType WhatToWatch;

        Vector3 leftRayDirection, rightRayDirection, sensorRayDirection;
        float rayRange;

        CellHandler ch;

        public static bool GizmoEnabled;

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
            rayRange = ScanMaxRange.Value;
        }

        void Start()
        {
            self = transform.parent.parent.gameObject;
            ch = GetComponentInParent<CellHandler>();
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
                var color = ch.Color;
                var arrowLenght = 0.4f;
                var direction = target.transform.position - transform.position;

                var clampDir = Vector3.ClampMagnitude(direction * arrowLenght, 40f);

                if (WhatToWatch == WatchType.PREY)
                {
                    DrawArrow.GizmoArrow(transform.position, clampDir, color);
                }
                if (WhatToWatch == WatchType.PREDATOR)
                {
                    DrawArrow.GizmoBlock(transform.position, clampDir, color);
                }
            }

            if (!GizmoEnabled) return;
            
            var colorBright = GetComponentInParent<SpriteRenderer>().color;
            var colorDark = new Color(colorBright.r, colorBright.g, colorBright.b, 0.1f);

            DrawSensors.Draw(transform.position, leftRayDirection * rayRange, colorDark);
            DrawSensors.Draw(transform.position, rightRayDirection * rayRange, colorDark);
            
            DrawSensors.Draw(transform.position, sensorRayDirection * rayRange, colorBright);
        }

        // checks if target is still valid or if some danger (some bigger cell) is in the way
        public void Recheck()
        {
            if (target == null || WhatToWatch == WatchType.PREDATOR) return;

            var rayDir = target.transform.position - transform.position;
            var start = (Vector2)transform.position;
            RaycastHit2D hit = Physics2D.Raycast(start, rayDir, rayDir.magnitude * 3f);
            if (hit.collider != null && hit.collider.tag == "Cell")
            {
                var obstacle = hit.collider.gameObject;
                if (obstacle.GetComponent<CellHandler>().Mass > ch.Mass)
                    target = null;
            }

        }

        void Update()
        {
            var myMass = ch.Mass;

            float totalFOV = 360.0f;
            float halfFOV = totalFOV / 2.0f;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, transform.forward);
            Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, transform.forward);
            leftRayDirection = leftRayRotation * transform.right;
            rightRayDirection = rightRayRotation * transform.right;

            if (Random.value < ScanChance.Value)
            {
                ch.Mass -= UsageFee;

                var rayDir = Random.value * totalFOV - halfFOV;
                sensorRayDirection = Quaternion.AngleAxis(rayDir, transform.forward) * transform.right;

                var dir = Random.insideUnitCircle.normalized;
                var start = (Vector2)transform.position;

                RaycastHit2D hit = Physics2D.Raycast(start, dir, ScanMaxRange.Value);
                if (hit.collider != null && hit.collider.tag == "Cell")
                {
                    candidate = hit.collider.gameObject;
                    
                    if (self.name == candidate.name) return;
                    
                    var yourMass = candidate.GetComponent<CellHandler>().Mass;

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
                if (self.name == target.name) {		// TODO: make this dirty fix obsolete
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
            var score = cell.GetComponent<CellHandler>().Mass / (dist * dist);
            if (cell.name.Contains(self.name)) score /= 10f;     // don't eat your own offspring unless there's nothing else

            return score;
        }

        public float getScore()
        {
            if (target == null) return 0f;
            return getScore(target);
        }
    }
}