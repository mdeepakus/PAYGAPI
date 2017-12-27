using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Extensions
{
    public static class Ensure
    {

        [DebuggerStepThrough]
        public static void ArgumentNotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNull(string argument, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentInRange(int argument, string argumentName, int? minValue = null, int? maxValue = null)
        {
            if (minValue.HasValue && argument < minValue.Value)
            {
                throw new ArgumentOutOfRangeException(argumentName, $"${argumentName} must be greater than {minValue.Value - 1}");
            }

            if (maxValue.HasValue && argument > maxValue.Value)
            {
                throw new ArgumentOutOfRangeException(argumentName, $"${argumentName} must be less than {maxValue.Value + 1}");
            }
        }
    }
}
