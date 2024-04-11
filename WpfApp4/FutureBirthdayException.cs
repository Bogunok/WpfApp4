using System;

namespace WpfApp4
{
    internal class FutureBirthdayException : Exception
    {
        public FutureBirthdayException():base("Date of birth can't be in the future!") { }   
    }
}
