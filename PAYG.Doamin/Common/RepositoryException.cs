using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Common
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message)
            : base(message)
        {
        }

        public RepositoryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
