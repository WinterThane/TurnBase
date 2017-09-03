using System.Collections.Generic;
using System.Linq;

namespace TurnBase
{
    class FrameCounter
    {
        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFPS { get; private set; }
        public float CurrentFPS { get; private set; }

        public const int MAXIMUM_SAMPLES = 100;

        private Queue<float> sampleBuffer = new Queue<float>();

        public FrameCounter()
        { }

        public bool Update(float deltaTime)
        {
            CurrentFPS = 1.0f / deltaTime;

            sampleBuffer.Enqueue(CurrentFPS);

            if(sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                sampleBuffer.Dequeue();
                AverageFPS = sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFPS = CurrentFPS;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;

            return true;
        }
    }
}
