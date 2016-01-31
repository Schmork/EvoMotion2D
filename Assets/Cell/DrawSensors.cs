using UnityEngine;
using System.Collections;

public class DrawSensors : MonoBehaviour {
    public static bool drawEnabled = true;

    public static void Draw(Vector3 pos, Vector3 direction, Color color)
    {
        if (!drawEnabled) return;

        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);
    }
}
