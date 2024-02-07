using System;
using GeneralScripts;
using Packs.Scene1;
using Packs.Scene1.UI;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class UIHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIDropDownMenu uiDropDownTypeFunction;
    [SerializeField] private UISliderResolution uiSliderResolution;
    [SerializeField] private UISliderTime uiSliderTime;
    [SerializeField] private UIInputFieldTransitionDuration uiInputField;
    [SerializeField] public Graph graph;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private GameObject menu;
    [SerializeField] private GPUGraph gpuGraph;
    [SerializeField] private CPUGraph cpuGraph;
    [SerializeField] private FrameRateCounter frameRateCounter;
    [SerializeField] private ButtonHandler statisticsButtonHandler;
    [SerializeField] private ButtonHandler renderingTypeButtonHandler;
    [SerializeField] private Serializator serializator;
    private void Start()
    {
        uiSliderResolution.UpdateValue();
        uiSliderTime.UpdateValue();
        uiInputField.UpdateValue();
        uiDropDownTypeFunction.UpdateValue();
        statisticsButtonHandler.SelectButton((int)frameRateCounter.displayMode );
        renderingTypeButtonHandler.SelectButton((int)serializator.Data.typeRendering);
    }

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
        graph.SetResolution(value);
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

    public int GetCurrentTransitionDuration() => graph.transitionDuration;

    public void SetGPUGraph()
    {
        graph = gpuGraph;
        graph.SetResolution(cpuGraph.resolution);
        graph.SetFunctionImmediately(cpuGraph.typeFunction);
        gpuGraph.gameObject.SetActive(true);
        cpuGraph.gameObject.SetActive(false);
    }
    
    public void SetCPUGraph()
    {
        graph = cpuGraph;
        graph.SetResolution(gpuGraph.resolution);
        graph.SetFunctionImmediately(gpuGraph.typeFunction);
        gpuGraph.gameObject.SetActive(false);
        cpuGraph.gameObject.SetActive(true);
    }

    public void SetFPS()
    {
        frameRateCounter.SetType(FrameRateCounter.DisplayMode.FPS);
    }
    
    public void SetMS()
    {
        frameRateCounter.SetType(FrameRateCounter.DisplayMode.MS);
    }
    
}