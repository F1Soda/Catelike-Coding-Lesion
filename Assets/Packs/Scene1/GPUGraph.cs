using UnityEngine;

namespace Packs.Scene1
{
    [ExecuteAlways]
    public class GPUGraph : Graph
    {
        private const int maxResolution = 1000;
        
        [Header("Settings")] 
        [SerializeField] private ComputeShader _computeShader;
        [SerializeField] private Material material;
        [SerializeField] private Mesh mesh;

        private static readonly int
            positionsId = Shader.PropertyToID("_Positions"),
            resolutionId = Shader.PropertyToID("_Resolution"),
            stepId = Shader.PropertyToID("_Step"),
            timeId = Shader.PropertyToID("_Time"),
            transitionProgressId = Shader.PropertyToID("_TransitionProgress");

        private bool _pause;
        private float _time;
        private bool _transitioning;
        private ETypeFunction _transitionFunction;
        private float _duration;
        private ComputeBuffer _positionsBuffer;

        private void OnEnable()
        {
            _positionsBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * 4);
            _transitionFunction = typeFunction;
        }

        private void OnDisable()
        {
            _positionsBuffer.Release(); // Отчистка буфера 
            _positionsBuffer = null;
        }

        private void Update()
        {
            if (_pause) return;
            _time += Time.deltaTime;
            if (_transitioning)
            {
                _duration += Time.deltaTime;
                if (_duration > transitionDuration)
                {
                    _transitioning = false;
                    _duration = 0f;
                    _transitionFunction = typeFunction;
                }
            }


            UpdateFunction();
        }

        public override void UpdateFunction()
        {
            float step = 2f / resolution;
            _computeShader.SetInt(resolutionId, resolution);
            _computeShader.SetFloat(stepId, step);
            _computeShader.SetFloat(timeId, Time.time);
            if (_transitioning)
            {
                _computeShader.SetFloat(
                    transitionProgressId,
                    Mathf.SmoothStep(0f, 1f, _duration / transitionDuration)
                );
            }

            var kernelIndex =
                (int)typeFunction + (int)(_transitioning ? _transitionFunction : typeFunction) * FunctionLibrary.CountFunctions;
            _computeShader.SetBuffer(kernelIndex, positionsId, _positionsBuffer);
            int groups = Mathf.CeilToInt(resolution / 8f);
            _computeShader.Dispatch(kernelIndex, groups, groups, 1);

            material.SetBuffer(positionsId, _positionsBuffer);
            material.SetFloat(stepId, step);
            var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
            Graphics.DrawMeshInstancedProcedural(
                mesh, 0, material, bounds, resolution * resolution
            );
        }

        private void Pause()
        {
            _pause = !_pause;
        }
        

        public override void SetFunction(ETypeFunction type)
        {
            typeFunction = type;
            _transitioning = true;
        }

        public override void SetResolution(int value)
        {
            resolution = value;
        }
        
        public override void SetFunctionImmediately(ETypeFunction type)
        {
            typeFunction = type;
        }
    }
}