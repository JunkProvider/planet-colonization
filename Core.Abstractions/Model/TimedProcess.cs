namespace SpaceLogistic.Core.Model
{
    using System;

    public abstract class TimedProcess
    {
        protected TimedProcess(TimeSpan totalTime)
            : this(totalTime, 0)
        {
        }

        protected TimedProcess(TimeSpan totalTime, double progress)
        {
            this.TotalTime = totalTime;
            this.Progress = progress;
        }

        public TimeSpan TotalTime { get; }

        public TimeSpan RemainingTime => TimeSpan.FromMilliseconds(this.TotalTime.TotalMilliseconds - (this.TotalTime.TotalMilliseconds * this.Progress));

        public double Progress { get; private set; }

        public bool IsCompleted => this.Progress >= 1;

        public void UpdateProgress(TimeSpan elapsedTime)
        {
            var passedTime = TimeSpan.FromMilliseconds(this.Progress * this.TotalTime.TotalMilliseconds) + elapsedTime;
            this.Progress = Math.Min(1, passedTime.TotalMilliseconds / this.TotalTime.TotalMilliseconds);
        }
    }
}
