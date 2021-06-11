namespace SpaceLogistic.Persistence.Model
{
    using System;

    public abstract class TimedProcessData
    {
        protected TimedProcessData(TimeSpan totalTime, double progress)
        {
            this.TotalTime = totalTime;
            this.Progress = progress;
        }

        public TimeSpan TotalTime { get; }

        public double Progress { get; }
    }
}
