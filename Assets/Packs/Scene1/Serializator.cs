using System;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using GeneralScripts;
using Packs.Scene1;

public class Serializator : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private SerializeData  data = new SerializeData();

    [SerializeField] private Graph graph;
    [SerializeField] private TimeManager timeManager;

    private readonly string _path = Application.dataPath + "/savedata.json";

    private void Start()
    {
        DeserializeData();
    }

    private void OnApplicationQuit()
    {
        SerializeData();
    }

    private void DeserializeData()
    {
        var tempJson = File.ReadAllText(_path);
        try
        {
            data = JsonConvert.DeserializeObject<SerializeData>(tempJson);
            graph.resolution = data.resolution;
            timeManager.SetTime(data.timeOfDay);
            graph.typeFunction = data.typeOfFunction;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
        }


        print(tempJson);
    }

    private void SerializeData()
    {
        data.resolution = graph.resolution;
        data.timeOfDay = timeManager.currentTime;
        data.typeOfFunction = graph.typeFunction;
        var json = JsonConvert.SerializeObject(data);
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