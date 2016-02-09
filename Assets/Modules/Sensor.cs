// each cell can have several sensors, either watching for food (PREY) or danger (PREDATOR).


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
            if (target != null)         // show prey / predator indicators
            {
                var color = ch.Color;
                var arrowLenght = 0.4f;
                var direction = target.transform.position - transform.position;

                var clampDir = Vector3.ClampMagnitude(direction * arrowLenght, 40f);

                var start = getPointAtEdge(clampDir);
                if (WhatToWatch == WatchType.PREY)
                {
                    DrawArrow.GizmoArrow(start, clampDir, color);
                }
                if (WhatToWatch == WatchType.PREDATOR)
                {
                    DrawArrow.GizmoBlock(start, clampDir, color);
                }
            }

            if (!GizmoEnabled) return;      // show sensor raycasts

            var colorBright = GetComponentInParent<SpriteRenderer>().color;
            var colorDark = new Color(colorBright.r, colorBright.g, colorBright.b, 0.1f);

            DrawSensors.Draw(getPointAtEdge(leftRayDirection), leftRayDirection * rayRange, colorDark);
            DrawSensors.Draw(getPointAtEdge(rightRayDirection), rightRayDirection * rayRange, colorDark);

            DrawSensors.Draw(getPointAtEdge(sensorRayDirection), sensorRayDirection * rayRange, colorBright);
        }

        // checks if target is still valid or if some danger (some bigger cell) is in the way
        public void Recheck()
        {
            if (target == null) return;         // nothing to recheck

            var rayDir = target.transform.position - transform.position;
            var start = getPointAtEdge(rayDir);
            RaycastHit2D hit = Physics2D.Raycast(start, rayDir, rayDir.magnitude * 3f);
            if (hit.collider != null)
            {
                var obstacle = hit.collider.gameObject;
                if (obstacle.GetComponent<CellHandler>().Mass > ch.Mass)
                {
                    if (WhatToWatch == WatchType.PREDATOR) target = obstacle;
                    if (WhatToWatch == WatchType.PREY) target = null;
                }
            }
        }

        void Update()
        {
            var myMass = ch.Mass;

            float totalFOV = 360.0f;        // 360 = full surround sight
            float halfFOV = totalFOV / 2.0f;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, transform.forward);
            Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, transform.forward);
            leftRayDirection = leftRayRotation * transform.right;
            rightRayDirection = rightRayRotation * transform.right;

            if (Random.value < ScanChance.Value * Time.deltaTime)
            {
                ch.Mass -= UsageFee;

                var rayDir = Random.value * totalFOV - halfFOV;
                sensorRayDirection = Quaternion.AngleAxis(rayDir, transform.forward) * transform.right;

                var dir = Random.insideUnitCircle.normalized;
                var start = getPointAtEdge(dir);

                //Debug.Log(transform.parent.parent.name + ", " + ScanMaxRange.Value);
                RaycastHit2D hit = Physics2D.Raycast(start, dir, ScanMaxRange.Value);
                if (hit.collider != null)
                {
                    candidate = hit.collider.gameObject;
                    var yourMass = candidate.GetComponent<CellHandler>().Mass;

                    if (WhatToWatch == WatchType.PREDATOR && yourMass < myMass) return;     // are we safe?
                    if (WhatToWatch == WatchType.PREY)
                    {
                        if (yourMass > myMass) return;                                              // no prey; it is too big
                        if (yourMass < myMass && yourMass > myMass / PreyFactor.Value) return;      // no prey; it is too small
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

            if (target != null                                                                                 // we have a target
                && WhatToWatch == WatchType.PREDATOR                                                           // we're supposed to look for danger
                && myMass < target.GetComponent<Rigidbody2D>().mass                                            // our target is dangerous
                && Vector2.Distance(transform.position, target.transform.position) > MaxFleeDistance.Value)    // but it's far away
                target = null;                                                                                 // so, forget about it.
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

        Vector2 getPointAtEdge(Vector2 direction)
        {
            return (Vector2)transform.position + ch.Radius * direction.normalized;
        }
    }
}