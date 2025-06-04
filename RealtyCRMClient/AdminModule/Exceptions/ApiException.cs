using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient.AdminModule.Exceptions
{
    public class ApiException : Exception
    {
        /// <summary>
        /// HTTP-статус код ошибки.
        /// </summary>
        public int StatusCode { get; }

        public ApiException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public ApiException(string message, int statusCode, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
