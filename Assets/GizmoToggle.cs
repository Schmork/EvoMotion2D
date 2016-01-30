using UnityEngine;

public class GizmoToggle : MonoBehaviour {

    void Start()
    {
        DrawArrow.doNotDraw = !GetComponent<UnityEngine.UI.Toggle>().isOn;
    }

    public void TargetDraw(bool enabled)
    {
        DrawArrow.doNotDraw = !enabled;
    }

    public void SensorDraw(bool enabled)
    {
        EvoMotion2D.Modules.Sensor.GizmoEnabled = enabled;
    }
}
