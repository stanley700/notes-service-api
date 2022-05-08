using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NotesService.Core.Exceptions
{
    public class MessageException : Exception
    {
        public string Code { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; } 
        public MessageException(string code, string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, Exception exception = null)
            :base(message, exception)
        {
            Code = code;
            HttpStatusCode = httpStatusCode;
        }
    }
}
