using System.Text;

namespace VendorMaster.Handlers
{
    public class TranzactionIterator
    {
        private readonly bool forced;
        private readonly IHandle[] tranzactions;
        public TranzactionIterator(bool forced, params IHandle[] tranzactions)
        {
            this.tranzactions = tranzactions;
            this.forced = forced;
        }

        public async Task<IterationResult> Start()
        {
            int i = 0;
            StringBuilder err = new StringBuilder();
            foreach (var tranzaction in tranzactions)
            {
                var output = await tranzaction.Handle();
                err.Append(output);
                
                if (forced && !string.IsNullOrEmpty(output))
                {
                    var rbErrors = await RollBack(i);
                    return new IterationResult() 
                    { 
                        Error = err.ToString(), 
                        RollBackError = rbErrors 
                    };
                }
                i++;
            }

            foreach (var tranzaction in tranzactions)
                await tranzaction.RemoveAll();

            return new IterationResult() 
            { 
                Error = err.ToString()
            };
        }

        private async Task<string> RollBack(int lastId)
        {
            StringBuilder err = new StringBuilder();
            for (int i = lastId; i >= 0; i--)
            {
                var output = await tranzactions[i].RollBack();
                if(string.IsNullOrEmpty(output))
                    err.Append(output);
            }

            return err.ToString();
        }
    }
}
