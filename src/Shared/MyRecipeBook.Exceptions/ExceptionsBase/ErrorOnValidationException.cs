using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : MyRecipeBookException
    {
        public IList<string> ErroMessages { get; set; }

        public ErrorOnValidationException(IList<string> erroMessages)
        {
               ErroMessages = erroMessages;
        }
    }
}
