namespace VendorMaster.Handlers
{
    public class IterationResult
    {
        public string Error { get; set; }
        public string RollBackError { get; set; }
        public bool IsSuccess
        {
            get
            {
                return string.IsNullOrEmpty(Error);
            }
        }
    }
}
