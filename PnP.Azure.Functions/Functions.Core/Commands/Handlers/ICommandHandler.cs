namespace Functions.Core.Commands.Handlers
{
    public interface ICommandHandler<in TCommand, out TResult>
    {
        TResult Execute(TCommand command);
    }
}