namespace SpaceLogistic.Core.CommandPattern
{
    using System;

    public abstract class CommandHandlerBase<TCommand> : ICommandHandler
        where TCommand : class
    {
        public Type CommandType => typeof(TCommand);

        public bool CanExecute(object command)
        {
            return this.CanExecute((TCommand)command);
        }

        public void Execute(object command)
        {
            this.Execute((TCommand)command);
        }

        public abstract bool CanExecute(TCommand command);

        public abstract void Execute(TCommand command);
    }
}