using System;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using GeneralScripts;
using Packs.Scene1;
using TMPro;

public class Serializator : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private SerializeData  data = new SerializeData();

    [SerializeField] public Graph graph;
    [SerializeField] public TimeManager timeManager;
    [SerializeField] private TMP_InputField transitionDurationInputField;
    [SerializeField] private FrameRateCounter frameRateCounter;

    private readonly string _path = Application.dataPath + "/savedata.json";

    public SerializeData Data => data;

    private void Start()
    {
        DeserializeData();
    }
    
    public void DeserializeData()
    {
        var tempJson = File.ReadAllText(_path);
        try
        {
            data = JsonConvert.DeserializeObject<SerializeData>(tempJson);
            graph.resolution = data.resolution;
            timeManager.SetTime(data.timeOfDay);
            graph.SetFunction(data.typeOfFunction);
            frameRateCounter.SetType(data.displayMode);
            graph.transitionDuration = data.transitionDuration;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
        }
        print(tempJson);
    }

    public void SerializeData()
    {
        data.resolution = graph.resolution;
        data.timeOfDay = timeManager.currentTime;
        data.typeOfFunction = graph.typeFunction;
        data.displayMode = frameRateCounter.displayMode;
        data.transitionDuration = graph.transitionDuration;
        data.typeRendering = graph is CPUGraph ? ETypeRendering.CPU : ETypeRendering.GPU;
        var json = JsonConvert.SerializeObject(data);
        print(json);
        try
        {
            File.WriteAllText(_path, json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}