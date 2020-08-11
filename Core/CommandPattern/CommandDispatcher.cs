namespace SpaceLogistic.Core.CommandPattern
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly Dictionary<Type, ICommandHandler> commandHandlers;

        public CommandDispatcher(IEnumerable<ICommandHandler> commandHandlers)
        {
            this.commandHandlers = commandHandlers.ToDictionary(commandHandler => commandHandler.CommandType);
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