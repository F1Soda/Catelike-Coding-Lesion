using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [HideInInspector] public string objectID;
    void Awake()
    {
        objectID = name;
    }
}
