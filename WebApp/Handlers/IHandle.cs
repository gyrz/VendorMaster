namespace VendorMaster.Handlers
{
    public interface IHandle
    {
        Task<string> Handle();
        Task<string> RollBack();
        Task RemoveAll();
    }
}
