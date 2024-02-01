using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Mathf;

namespace Packs.Scene1
{
    public enum ETypeFunction
    {
        Wave,
        MultiWave,
        Ripple,
        Sphere,
        Torus
    }

    public static class FunctionLibrary
    {
        public delegate Vector3 Function(float u, float v, float t);

        private static Function[] _functions = { Wave, MultiWave, Ripple, Sphere, Torus };

        public static Function GetFunction(ETypeFunction type)
        {
            return _functions[(int)type];
        }

        public static int CountFunctions => _functions.Length;

        private static Vector3 Wave(float u, float v, float t)
        {
            Vector3 p;
            p.x = u;
            p.y = Sin(PI * (u + v + t));
            p.z = v;
            return p;
        }

        private static Vector3 MultiWave(float u, float v, float t)
        {
            Vector3 p;
            p.x = u;
            p.y = Sin(PI * (u + t * 0.5f));
            p.y += Sin(2f * PI * (v + t)) * 0.5f;
            p.y += Sin(PI * (u + v + 0.25f * t));
            p.y *= (2f / 5f);
            p.z = v;
            return p;
        }

        private static Vector3 Ripple(float u, float v, float t)
        {
            Vector3 p;
            var d = Sqrt(u * u + v * v);
            p.x = u;
            p.y = Sin(PI * (4f * d - t));
            p.y /= (1f + 10f * d);
            p.z = v;
            return p;
        }

        private static Vector3 Sphere(float u, float v, float t)
        {
            float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t)); // 0.5f + 0.5f * Sin(PI * t);
            float s = r * Cos(0.5f * PI * v);
            Vector3 p;
            p.x = s * Sin(PI * u);
            p.y = r * Sin(PI * 0.5f * v);
            p.z = s * Cos(PI * u);
            return p;
        }

        private static Vector3 Torus(float u, float v, float t)
        {
            float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
            float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
            float s = r1 +  r2 * Cos( PI * v);
            Vector3 p;
            p.x = s * Sin(PI * u);
            p.y = r2 * Sin(PI * v);
            p.z = s * Cos(PI * u);
            return p;
        }

        public static Vector3 Morph(float u, float v, float t, Function from, Function to, float progress)
        {
            return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
        }
        
    }
}