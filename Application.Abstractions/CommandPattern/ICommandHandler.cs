namespace SpaceLogistic.Application.CommandPattern
{
    using System;

    public interface ICommandHandler
    {
        Type CommandType { get; }

        bool CanExecute(object command);

        void Execute(object command);
    }
}