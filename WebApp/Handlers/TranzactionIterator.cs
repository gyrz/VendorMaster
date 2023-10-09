using System.Text;

namespace VendorMaster.Handlers
{
    public class TranzactionIterator
    {
        private readonly IHandle[] tranzactions;
        public TranzactionIterator(params IHandle[] tranzactions)
        {
            this.tranzactions = tranzactions;
        }

        public async Task<string> Start()
        {
            StringBuilder err = new StringBuilder();
            foreach (var tranzaction in tranzactions)
            {
                var output = await tranzaction.Handle();
                err.Append(output);
            }

            return err.ToString();
        }

    }
}
