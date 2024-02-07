using System;
using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class UISliderResolution : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider _slider;
    [SerializeField] private UIHandler uiHandler;
    [SerializeField] private TMP_Text textResolution;

    public event Action<int> OnSettingSlider;
    
    public void UpdateValue()
    {
        _slider.SetValueWithoutNotify(uiHandler.GetCurrentResolution());
        textResolution.text = uiHandler.GetCurrentResolution().ToString();
    }

    public void OnValueChanged(float value)
    {
        textResolution.text = ((int)value).ToString();
        OnSettingSlider?.Invoke((int)value);
    }
    
}
