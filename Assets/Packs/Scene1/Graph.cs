using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Packs.Scene1
{
#if UNITY_EDITOR
    [ExecuteAlways]
#endif
    public class Graph : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private GameObject pointPrefab;

        [Header("Settings")] [SerializeField, Range(10, 100)] [Delayed]
        public int resolution = 10;

        [SerializeField] public ETypeFunction typeFunction = ETypeFunction.Ripple;
        [SerializeField, Min(0f)] public float transitionDuration = 1f;
        
        
        private static GameObject _pointPrefab;
        private static Transform _transform;
        private static List<GameObject> _points;
        private static int _resolution;
        private static int _pastResolution = _resolution;
        private static bool _pause;
        private static float _time;
        private static ETypeFunction _typeFunction;

        private bool transitioning;
        private ETypeFunction transitionFunction;
        private float duration;

        private void Start()
        {
            _resolution = resolution;
            _typeFunction = typeFunction;
            if (_pointPrefab is null)
                _pointPrefab = pointPrefab;
            if (_transform is null)
                _transform = transform;
            CreateGraph();
        }

        public void SetFunction(ETypeFunction type)
        {
            transitionFunction = type;
            transitioning = true;
        }
        
        private void Update()
        {
            _resolution = resolution;
            _typeFunction = typeFunction;
            if (_pointPrefab is null)
                _pointPrefab = pointPrefab;
            if (_transform is null)
                _transform = transform;
            if (_pastResolution != _resolution)
            {
                CreateGraph();
                DrawGraph();
                _pastResolution = _resolution;
            }

            if (_pause) return;
            _time  += Time.deltaTime;
            if (transitioning)
            {
                duration += Time.deltaTime;
                if (duration > transitionDuration)
                {
                    transitioning = false;
                    duration = 0f;
                    _typeFunction = typeFunction = transitionFunction;
                }
            }

            if(transitioning)
                DrawTransitionGraph();
            else
                DrawGraph();
            
            
        }

#if UNITY_EDITOR
        [MenuItem("Tutorial/Create Graph %#d")]
#endif
        private static void CreateGraph()
        {
            var step = 2f / _resolution;
            var scale = Vector3.one * step;
            CleaGraph();
            _points = new List<GameObject>(_resolution * _resolution);
            var len = _resolution * _resolution;
            for (int i = 0; i < len; i++)
            {
                var point = Instantiate(_pointPrefab, _transform);
                point.transform.localScale = scale;
                _points.Add(point);
            }
        }
#if UNITY_EDITOR
        [MenuItem("Tutorial/Clear Graph &d")]
#endif
        private static void CleaGraph()
        {
            if (_points is not null)
            {
                foreach (var point in _points)
                {
                    DestroyImmediate(point);
                }

                _points.Clear();
            }


            var tempCountChildren = _transform.childCount;
            for (int i = 0; i < tempCountChildren; i++)
            {
                DestroyImmediate(_transform.GetChild(0).gameObject);
            }
        }
#if UNITY_EDITOR
        [MenuItem("Tutorial/Pause &w")]
#endif
        private static void Pause()
        {
            _pause = !_pause;
        }

        private void DrawGraph()
        {
            if (_points is null)
            {
                CreateGraph();
            }

            var step = 2f / _resolution;
            var len = _points.Count;
            var v = 0.5f * step - 1f;
            for (int i = 0, x = 0, z = 0; i < len; i++, x++)
            {
                if (x == _resolution)
                {
                    x = 0;
                    z += 1;
                    v = (z + 0.5f) * step - 1f;
                }

                var u = (x + 0.5f) * step - 1f;
                _points[i].transform.localPosition = FunctionLibrary.GetFunction(_typeFunction)(u, v, _time);
            }
        }
        
        private void DrawTransitionGraph()
        {
            FunctionLibrary.Function from = FunctionLibrary.GetFunction(typeFunction),
                to = FunctionLibrary.GetFunction(transitionFunction);
            float progress = duration / transitionDuration;
            
            var step = 2f / _resolution;
            var len = _points.Count;
            var v = 0.5f * step - 1f;
            for (int i = 0, x = 0, z = 0; i < len; i++, x++)
            {
                if (x == _resolution)
                {
                    x = 0;
                    z += 1;
                    v = (z + 0.5f) * step - 1f;
                }

                var u = (x + 0.5f) * step - 1f;
                _points[i].transform.localPosition = FunctionLibrary.Morph(u, v, _time, from, to, progress);
            }
        }
        
    }
}