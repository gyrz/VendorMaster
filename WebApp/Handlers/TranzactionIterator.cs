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
            StringBuilder err = new StringBuilder();
            foreach (var tranzaction in tranzactions)
            {
                var output = await tranzaction.Handle();
                err.Append(output);
                
                if (forced && !string.IsNullOrEmpty(output))
                {
                    var rbErrors = await RollBack();
                    return new IterationResult() 
                    { 
                        Error = err.ToString(), 
                        RollBackError = rbErrors 
                    };
                }
            }

            foreach (var tranzaction in tranzactions)
                await tranzaction.RemoveAll();

            return new IterationResult() 
            { 
                Error = err.ToString()
            };
        }

        public async Task<string> RollBack()
        {
            StringBuilder err = new StringBuilder();
            for (int i = tranzactions.Length - 1; i >= 0; i--)
            {
                var output = await tranzactions[i].RollBack();
                if(string.IsNullOrEmpty(output))
                    err.Append(output);
            }

            return err.ToString();
        }
    }
}
