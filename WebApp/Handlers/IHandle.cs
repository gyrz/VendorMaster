namespace VendorMaster.Handlers
{
    public interface IHandle
    {
        Task<(string, int)> Handle();
    }
}
