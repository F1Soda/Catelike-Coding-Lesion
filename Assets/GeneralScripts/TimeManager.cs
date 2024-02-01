using System;
using UnityEngine;

namespace GeneralScripts
{
    public enum ETimeOfDay
    {
        Morning,
        Afternoon,
        Evening,
        Night,
    }
    [ExecuteAlways]
    public class TimeManager : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform sunTransform;
        [SerializeField] public ETimeOfDay currentTime;
        
        public void SetTime(ETimeOfDay time)
        {
            currentTime = time;
            var currentRotation = transform.eulerAngles;
            switch (time)
            {
                case ETimeOfDay.Morning:
                    currentRotation.x = 0f;
                    break;
                case ETimeOfDay.Afternoon:
                    currentRotation.x = 90f;
                    break;
                case ETimeOfDay.Evening:
                    currentRotation.x = 190f;
                    break;
                case ETimeOfDay.Night:
                    currentRotation.x = 340f;
                    break;
                default:
                    throw new Exception("Error: undefined type of ETimeOfDay");
            }
            sunTransform.eulerAngles = currentRotation;
        }
    }
}