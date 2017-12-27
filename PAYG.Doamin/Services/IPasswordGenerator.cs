using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Services
{
    public interface IPasswordGenerator
    {
        string Generate(int length, int numberOfNonAlpanumericCharacters);
    }
}
