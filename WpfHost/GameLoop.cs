﻿namespace SpaceLogistic.WpfHost
{
    using System;
    using System.Timers;
    using System.Windows.Threading;
    using SpaceLogistic.Application;
    using SpaceLogistic.Core.Model;
    using SpaceLogistic.Core.Services;
    using SpaceLogistic.WpfView.View;

    public sealed class GameLoop : IGameLoop
    {
        private readonly IGameUpdateService gameUpdateService;

        private readonly IGameProvider gameProvider;

        private readonly GameViewModel gameViewModel;

        private readonly Dispatcher dispatcher;

        private readonly Timer updateTimer;

        private DateTime lastUpdateTime;

        public GameLoop(IGameUpdateService gameUpdateService, IGameProvider gameProvider, GameViewModel gameViewModel, Dispatcher dispatcher)
        {
            this.gameUpdateService = gameUpdateService;
            this.gameProvider = gameProvider;
            this.gameViewModel = gameViewModel;
            this.dispatcher = dispatcher;

            this.updateTimer = new Timer(TimeSpan.FromSeconds(0.05).TotalMilliseconds);
            this.updateTimer.AutoReset = false;
            this.updateTimer.Elapsed += this.OnUpdateTimerElapsed;
        }

        public void Start()
        {
            this.gameUpdateService.Startup(this.gameProvider.Get());
            this.lastUpdateTime = DateTime.UtcNow;
            this.updateTimer.Start();
        }

        public void Stop()
        {
            this.updateTimer.Stop();
        }

        private void OnUpdateTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.Update();
            this.updateTimer.Start();
        }

        private void Update()
        {
            var now = DateTime.UtcNow;
            var elapsedTime = now - this.lastUpdateTime;
            this.lastUpdateTime = now;

            this.gameUpdateService.Update(this.gameProvider.Get(), elapsedTime);

            this.dispatcher.Invoke(() => this.gameViewModel.Update());
        }
    }
}
