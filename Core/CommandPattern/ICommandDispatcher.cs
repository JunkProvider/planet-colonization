namespace SpaceLogistic.Core.CommandPattern
{
    public interface ICommandDispatcher
    {
        bool CanExecute(object command);

        void Execute(object command);
    }
}
