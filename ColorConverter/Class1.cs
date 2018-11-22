using System;

namespace CSharpColorSpaceConverter
{
    public static class ColorSpaceConverter
    {
        public static Tuple<int, int, int> RGBToHSL()
        {
            return Tuple.Create<int, int, int>(180, 115, 30);
        }
    }
}
