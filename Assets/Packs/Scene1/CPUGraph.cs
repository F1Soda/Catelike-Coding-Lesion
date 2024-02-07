using System.Collections.Generic;
using UnityEngine;

namespace Packs.Scene1
{
    [ExecuteAlways]
    public class CPUGraph : Graph
    {
        [Header("References")]
        [SerializeField] private GameObject pointPrefab;

        private List<GameObject> points;
        private bool pause;
        private float time;

        private bool transitioning;
        private ETypeFunction transitionFunction;
        private float duration;
        private int _transitionDuration;

        private void OnEnable()
        {
            CreateGraph();
        }

        private void OnDisable()
        {
            CleaGraph();
        }

        public override void UpdateFunction()
        {
            FunctionLibrary.Function from = FunctionLibrary.GetFunction(typeFunction),
                to = FunctionLibrary.GetFunction(transitionFunction);
            var progress = duration / transitionDuration;
            var step = 2f / resolution;
            var len = points.Count;
            var v = 0.5f * step - 1f;
            for (int i = 0, x = 0, z = 0; i < len; i++, x++)
            {
                if (x == resolution)
                {
                    x = 0;
                    z += 1;
                    v = (z + 0.5f) * step - 1f;
                }

                var u = (x + 0.5f) * step - 1f;
                if (transitioning)
                    points[i].transform.localPosition = FunctionLibrary.Morph(u, v, time, from, to, progress);
                else
                    points[i].transform.localPosition = FunctionLibrary.GetFunction(typeFunction)(u, v, time);
            }
        }

        public override void SetFunction(ETypeFunction type)
        {
            transitionFunction = type;
            transitioning = true;
        }

        private void Update()
        {
            if (pause) return;
            time += Time.deltaTime;
            if (transitioning)
            {
                duration += Time.deltaTime;
                if (duration > transitionDuration)
                {
                    transitioning = false;
                    duration = 0f;
                    typeFunction = transitionFunction;
                }
            }
            UpdateFunction();
        }

        private void CreateGraph()
        {
            var step = 2f / resolution;
            var scale = Vector3.one * step;
            CleaGraph();
            points = new List<GameObject>(resolution * resolution);
            var len = resolution * resolution;
            for (int i = 0; i < len; i++)
            {
                var point = Instantiate(pointPrefab, transform);
                point.transform.localScale = scale;
                points.Add(point);
            }
        }

        private void CleaGraph()
        {
            if (points is not null)
            {
                foreach (var point in points)
                    DestroyImmediate(point);
                points.Clear();
            }
            var tempCountChildren = transform.childCount;
            for (int i = 0; i < tempCountChildren; i++)
                DestroyImmediate(transform.GetChild(0).gameObject);
        }

        private void Pause()
        {
            pause = !pause;
        }

        public override void SetResolution(int value)
        {
            if (value > 200 || value < 10)
                return;
            resolution = value;
            CreateGraph();
            UpdateFunction();
        }

        public override void SetFunctionImmediately(ETypeFunction type)
        {
            typeFunction = type;
        }
    }
}