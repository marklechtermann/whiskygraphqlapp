namespace WhiskyApp.DataAccess
{
    public interface IUnitOfWork
    {
        IWhiskyRepository WhiskyRepository { get; }

        IDestilleryRepository DestilleryRepository { get; }

        void Save();
    }
}