namespace QColonFrame
{
    public partial class QCEntity
    {
        public Guid Id { get; private set; }

        public QCEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
