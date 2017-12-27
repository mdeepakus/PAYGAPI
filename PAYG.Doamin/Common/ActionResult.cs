using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Common
{
    public class ActionResult<T>
    {
        public string ErrorMessage { get; set; }

        public Result Result { get; set; }

        public T Object { get; set; }
    }
}

