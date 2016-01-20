using UnityEngine;

public class GizmoToggle : MonoBehaviour {
    public void setDrawEnabled(bool enabled)
    {
        DrawArrow.doNotDraw = !enabled;
    }
}
