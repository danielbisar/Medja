using System;

namespace Medja.Utils.Text
{
    public class ExpectException : Exception
    {
        public ExpectException(string message)
            : base(message)
        {
        }
    }
}



