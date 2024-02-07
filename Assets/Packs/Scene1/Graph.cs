using UnityEngine;

namespace Packs.Scene1
{
    public abstract class Graph : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Range(10, 200)] public int resolution = 10;
        [SerializeField] public ETypeFunction typeFunction = ETypeFunction.Ripple;
        [SerializeField, Min(0f)] public int transitionDuration = 1;

        public abstract void UpdateFunction();

        public abstract void SetFunction(ETypeFunction type);

        public abstract void SetResolution(int value);

        public abstract void SetFunctionImmediately(ETypeFunction type);
    }
}