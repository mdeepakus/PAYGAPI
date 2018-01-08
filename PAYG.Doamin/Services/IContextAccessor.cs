using PAYG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Services
{
    public interface IContextAccessor
    {
        ApplicationUser CurrentUser { get; }
    }
}
