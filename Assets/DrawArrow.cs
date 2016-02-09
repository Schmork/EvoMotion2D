// taken from: http://forum.unity3d.com/threads/debug-drawarrow.85980/

using UnityEngine;

public static class DrawArrow
{
    public static bool doNotDraw = true;
    
    public static void GizmoArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.6f, float arrowHeadAngle = 20.0f)
    {
        if (doNotDraw) return;

        direction = direction.normalized * 2f;

        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 1, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, -1, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
    }

    public static void GizmoBlock(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.7f, float arrowHeadAngle = 20.0f)
    {
        if (doNotDraw) return;
        direction = direction.normalized * 1f;

        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0) * new Vector3(0, 1, 0);

        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, -right * arrowHeadLength);
    }

    public static void GizmoSensor(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.6f, float arrowHeadAngle = 20.0f)
    {
        if (doNotDraw) return;

        direction = direction.normalized * 4f;

        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);
        
        Gizmos.DrawSphere(pos + direction, arrowHeadLength);
    }

    public static void GizmoHeading(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.7f, float arrowHeadAngle = 20.0f)
    {
        //if (doNotDraw) return;
        direction = direction.normalized * 4f;

        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 1, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, -1, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
    }
}
