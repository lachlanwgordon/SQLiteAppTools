using System;
namespace SQLiteAppTools.Extensions
{
    public static class TypeExtensions
    {
        public static int Length(this int value)
        {
            //Straight off stack overflow https://stackoverflow.com/questions/4483886/how-can-i-get-a-count-of-the-total-number-of-digits-in-a-number
            return (int)Math.Floor(Math.Log10(value) + 1);
        }
    }
}
