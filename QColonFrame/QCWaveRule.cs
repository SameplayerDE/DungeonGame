namespace QColonFrame
{
    public abstract class QCWaveRule<T>
    {
        public abstract bool IsValid(T[,] grid, int x, int y);
    }
}
