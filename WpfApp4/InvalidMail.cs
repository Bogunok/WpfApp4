using System;

namespace WpfApp4
{
    internal class InvalidMail : Exception
    {
        public InvalidMail():base("Invalid mail!") { }
    }
}
