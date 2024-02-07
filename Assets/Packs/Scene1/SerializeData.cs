using System;
using Packs.Scene1;

namespace GeneralScripts
{
    public enum ETypeRendering
    {
        GPU,
        CPU
    }


    [Serializable]
    public class SerializeData
    {
        public int resolution;
        public ETimeOfDay timeOfDay;
        public ETypeFunction typeOfFunction;
        public int transitionDuration;
        public FrameRateCounter.DisplayMode displayMode;
        public ETypeRendering typeRendering;
    }
}