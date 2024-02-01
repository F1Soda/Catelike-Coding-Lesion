using GeneralScripts;
using Packs.Scene1;
using Packs.Scene1.UI;
using UnityEngine;

[ExecuteAlways]
public class UIHandler : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private UIDropDownMenu uiDropDownTypeFunction;
    [SerializeField] private UISliderResolution uiSliderResolution;
    [SerializeField] private UISliderTime uiSliderTime;
    [SerializeField] private UIInputFieldTransitionDuration uiInputField;
    [SerializeField] private Graph graph;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private GameObject menu;


    public void TurnOffOnMenu()
    {
        menu.SetActive(!menu.activeInHierarchy);
    }
    
    private void OnEnable()
    {
        uiDropDownTypeFunction.OnSelectedDropdownOption += ChangeTypeFunction;
        uiSliderResolution.OnSettingSlider += ChangeResolution;
        uiSliderTime.OnSettingSlider += ChangeTimeOfDay;
        uiInputField.OnSettingInputField += ChangeTransitionDuration;
    }

    private void OnDisable()
    {
        uiDropDownTypeFunction.OnSelectedDropdownOption -= ChangeTypeFunction;
        uiSliderResolution.OnSettingSlider -= ChangeResolution;
        uiSliderTime.OnSettingSlider -= ChangeTimeOfDay;
        uiInputField.OnSettingInputField -= ChangeTransitionDuration;
    }

    private void ChangeTypeFunction(ETypeFunction type)
    {
        graph.SetFunction(type);
    }

    private void ChangeResolution(int value)
    {
        graph.resolution = value;
    }

    private void ChangeTimeOfDay(ETimeOfDay time)
    {
        timeManager.SetTime(time);
    }
    
    private void ChangeTransitionDuration(int duration)
    {
        graph.transitionDuration = duration;
    }

    public int GetCurrentIdFunction() => (int)graph.typeFunction;
    public int GetCurrentResolution() => graph.resolution;
    public int GetCurrentTimeOfDay() => (int)timeManager.currentTime;

    public int GetCurrentTransitionDuration() => (int)graph.transitionDuration;

}
