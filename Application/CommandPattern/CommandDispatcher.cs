namespace SpaceLogistic.Application.CommandPattern
{
    public sealed class CommandDispatcher : ICommandDispatcher
    {
        private CommandExecutor executor;
        
        public bool CanExecute(object command)
        {
            return this.executor.CanExecute(command);
        }

        public void Execute(object command)
        {
            this.executor.Execute(command);
        }

        public void SetExecutor(CommandExecutor executor)
        {
            this.executor = executor;
        }
    }
}