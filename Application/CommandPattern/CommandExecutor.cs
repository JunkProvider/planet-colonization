namespace SpaceLogistic.Application.CommandPattern
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;

    public sealed class CommandExecutor : IHostedService
    {
        private readonly Dictionary<Type, ICommandHandler> commandHandlers;

        private readonly CommandDispatcher dispatcher;

        public CommandExecutor(CommandDispatcher dispatcher, IEnumerable<ICommandHandler> commandHandlers)
        {
            this.dispatcher = dispatcher;
            this.commandHandlers = commandHandlers.ToDictionary(commandHandler => commandHandler.CommandType);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.dispatcher.SetExecutor(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public bool CanExecute(object command)
        {
            var commandType = command.GetType();

            if (!this.commandHandlers.TryGetValue(commandType, out var commandHandler))
            {
                throw new InvalidOperationException();
            }

            return commandHandler.CanExecute(command);
        }

        public void Execute(object command)
        {
            var commandType = command.GetType();

            if (!this.commandHandlers.TryGetValue(commandType, out var commandHandler))
            {
                throw new InvalidOperationException();
            }

            commandHandler.Execute(command);
        }
    }
}