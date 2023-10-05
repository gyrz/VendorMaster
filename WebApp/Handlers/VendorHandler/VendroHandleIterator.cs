using System.Text;

namespace VendorMaster.Handlers.VendorHandler
{
    public class VendorHandleIterator
    {
        private readonly IHandle[] handlers;
        public VendorHandleIterator(params IHandle[] handlers)
        {
            this.handlers = handlers;
        }

        public async Task<string> Start()
        {
            StringBuilder err = new StringBuilder();
            foreach (var handler in handlers)
            {
                var output = await handler.handle();
                err.Append(output);
            }

            return err.ToString();
        }

    }
}
