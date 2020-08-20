namespace SpaceLogistic.Application.CommandPattern
{
    public sealed class NullCommandDispatcher : ICommandDispatcher
    {
        public static NullCommandDispatcher Instance { get; } = new NullCommandDispatcher();

        public bool CanExecute(object command)
        {
            return true;
        }

        public void Execute(object command)
        {
        }
    }
}
