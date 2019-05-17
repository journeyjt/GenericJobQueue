namespace Generic.Queue
{
    public interface IJobQueue
    {
        void Enqueue(IJob job);

        bool TryDequeue(out IJob job);

        bool Empty();

        int Size();
    }
}