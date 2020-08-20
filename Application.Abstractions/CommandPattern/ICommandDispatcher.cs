namespace SpaceLogistic.Application.CommandPattern
{
    public interface ICommandDispatcher
    {
        bool CanExecute(object command);

        void Execute(object command);
    }
}
