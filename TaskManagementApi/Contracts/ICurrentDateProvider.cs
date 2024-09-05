namespace TaskManagementApi.Contracts
{
    public interface ICurrentDateProvider
    {
        DateTimeOffset Now { get; }

        DateTimeOffset NowUtc { get; }
    }
}
