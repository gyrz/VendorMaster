using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicService.Base
{
    public interface IValidator
    {
        IValidator SetNext(IValidator handler);

        string Handle(object request = null);
    }
}
