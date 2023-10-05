using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicService.Base
{
    public abstract class Validator : IValidator
    {
        private IValidator _nextHandler;

        public IValidator SetNext(IValidator handler)
        {
            this._nextHandler = handler;

            return handler;
        }

        public virtual string Handle(object request = null)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(request);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
