using UnityEngine;

public class TimeScale : MonoBehaviour {

    public UnityEngine.UI.Slider slider;
    
    public void ValueChanged()
    {
        Time.timeScale = slider.value;
    }
}
