namespace Generic.Queue
{
    public interface IJobQueue
    {
        void Enqueue(IJob job);

        bool TryDequeue(out IJob job);

        void Close();

        bool Empty();
    }
}