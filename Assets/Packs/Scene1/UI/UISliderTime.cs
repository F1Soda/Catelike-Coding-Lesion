using System;
using GeneralScripts;
using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class UISliderTime : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Slider _slider;

    [SerializeField] private UIHandler uiHandler;
    [SerializeField] private TMP_Text textTime;

    public event Action<ETimeOfDay> OnSettingSlider;
    
    public void UpdateValue()
    {
        _slider.SetValueWithoutNotify(uiHandler.GetCurrentTimeOfDay());
        textTime.text = ((ETimeOfDay)uiHandler.GetCurrentTimeOfDay()).ToString();
    }

    public void OnValueChanged(float value)
    {
        textTime.text = ((ETimeOfDay)value).ToString();
        OnSettingSlider?.Invoke((ETimeOfDay)value);
    }
}