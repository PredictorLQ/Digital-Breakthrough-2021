using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiMedMyStem
{
    // класс для методов расширения 
    public static class Exp
    {
        public static bool Between(this int value, int startInclusive, int endExclusive)
        {
            return value >= startInclusive && value < endExclusive;
        }
    }
}
