using System;
using System.Collections.Generic;
using System.Linq;
using GeneralScripts;
using Packs.Scene1;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public List<DontDestroy> dontDestroyObjects;
    public int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;


    private Serializator serializator;
    private void Start()
    {
        if (instance is not null)
        {
            instance.InitializeScene();
            Destroy(gameObject);
            return;
        }
        else
            instance = this;
        DontDestroyOnLoad(gameObject);
        
        dontDestroyObjects = Resources.FindObjectsOfTypeAll<DontDestroy>().ToList();
        foreach (var obj in dontDestroyObjects)
            DontDestroyOnLoad(obj.gameObject);

        serializator = FindObjectOfType<Serializator>();
    }

    private void InitializeScene()
    {
        instance.ClearDontDestroyObject();
        
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                SetUICamera();
                serializator.timeManager = FindObjectOfType<TimeManager>();
                serializator.graph = FindObjectOfType<Graph>();
                serializator.DeserializeData();
                break;
            case 1:
                break;
            default:
                throw new Exception("Неопознаная сцена");
        }
    }

    private void DeinitializeScene()
    {
        switch (CurrentSceneIndex)
        {
            case 0:
                serializator.SerializeData();
                break;
            default:
                break;
        }
    }

    private void SetUICamera()
    {
        var mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>()
            .GetUniversalAdditionalCameraData();
        mainCamera.cameraStack.Add(GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>());
    }

    public void LoadScene(int index)
    {
        if(index == CurrentSceneIndex)
            return;
        if (index < 0 || index >= SceneManager.sceneCount)
            throw new Exception("Неизвестный индекс сцены:" + index);
        DeinitializeScene();
        SceneManager.LoadScene(index);
    }
    
    public void LoadNextScene()
    {
        DeinitializeScene();
        SceneManager.LoadScene((CurrentSceneIndex + 1)%2);
    }

    private void ClearDontDestroyObject()
    {
        var listDontDestroyObjectOnScene = Resources.FindObjectsOfTypeAll<DontDestroy>().ToList();
        var len = listDontDestroyObjectOnScene.Count;
        var finded = false;
        for (int i = 0; i < len; i++)
        {
            var currentObjInScene = listDontDestroyObjectOnScene[i];
            foreach (var objInList in dontDestroyObjects)
            {
                if (currentObjInScene.GetInstanceID() == objInList.GetInstanceID())
                {
                    finded = true;
                    break;
                }

                if (string.Equals(currentObjInScene.objectID, objInList.objectID))
                {
                    Destroy(currentObjInScene.gameObject);
                    finded = true;
                    break;
                }
            }
            if (!finded)
            {
                dontDestroyObjects.Add(currentObjInScene);
            }
            finded = false;
        }
    }
}