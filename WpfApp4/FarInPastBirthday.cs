using System;

namespace WpfApp4
{
    internal class FarInPastBirthday : Exception
    {
        public FarInPastBirthday():base("Date of birth can't be so far in the past!") { }      
    }
}
