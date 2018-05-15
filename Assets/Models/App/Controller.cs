namespace App
{
    public class Controller : IController
    {
        IMoveController _moveController;
        IFireController _fireController;

        public IMoveController MoveController
        {
            get { return _moveController; }
        }

        public IFireController FireController
        {
            get { return _fireController; }
        }

        public Controller(IMoveController moveController, IFireController fireController)
        {
            _moveController = moveController;
            _fireController = fireController;
        }
    }
}
