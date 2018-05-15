namespace App
{
    public interface IController
    {
        IMoveController MoveController { get; }
        IFireController FireController { get; }
    }
}
