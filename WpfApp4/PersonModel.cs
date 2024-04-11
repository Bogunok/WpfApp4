using System;


namespace WpfApp4
{
    internal class PersonModel
    {
        private DateTime _dateOfBirth;
        private string _surname;
        private string _name;
        private string _mail;
        private bool _isAdult;
        private string _sunSign;
        private string _chineseSign;
        private bool _isBirthday;

        public PersonModel(string name, string surname, string mail, DateTime dateOfBirth)
        {
            this._dateOfBirth = dateOfBirth;
            this._surname = surname;
            this._name = name;
            this._mail = mail;
            this._isAdult = FindOutIfPersonIsAdult();
            this._sunSign = CalculateSunSign();
            this._chineseSign = CalculateChineseSign();
            this._isBirthday = CalculateBirthday();
        }

        public PersonModel(string name, string surname, string mail) :
            this(name, surname, mail, DateTime.Today)
        { }

        public PersonModel(string name, string surname, DateTime dateOfBirth) :
            this(name, surname, "", dateOfBirth)
        { }

        public PersonModel() 
        {

        }

        public DateTime Birthdate
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Surname
        {
            get { return _surname; }
            set { _surname = value; }
        }

        public string Mail
        {
            get { return _mail; }
            set { _mail = value; }
        }

        public bool IsAdult
        {
            get { return _isAdult; }
            set { _isAdult = value; }
        }

        public string SunSign
        {
            get { return _sunSign; }
            set { _sunSign = value; }
        }

        public string ChineseSign
        {
            get { return _chineseSign; }
            set { _chineseSign = value; }
        }

        public bool IsBirthday
        {
            get { return _isBirthday; }
            set { _isBirthday = value; }
        }

        public bool FindOutIfPersonIsAdult()
        {
            DateTime current = DateTime.Now;
            int age = current.Year - _dateOfBirth.Year;

            if (current.Month < _dateOfBirth.Month || (current.Month == _dateOfBirth.Month && current.Day < _dateOfBirth.Day))
            {
                age--;
            }
            return age >= 18;
        }

        public int Age()
        {
            DateTime current = DateTime.Now;
            int age = current.Year - _dateOfBirth.Year;

            if (current.Month < _dateOfBirth.Month || (current.Month == _dateOfBirth.Month && current.Day < _dateOfBirth.Day))
            {
                age--;
            }
            return age;
        }

        public bool CalculateBirthday()
        {
            DateTime current = DateTime.Now;
            if (current.Month == _dateOfBirth.Month && current.Day == _dateOfBirth.Day) return true;
            return false;
        }


        public string CalculateSunSign()
        {
            DateTime[] sunSignDates = new DateTime[]
            {
                new DateTime(_dateOfBirth.Year, 1, 20),   // Aquarius
                new DateTime(_dateOfBirth.Year, 2, 19),   // Pisces
                new DateTime(_dateOfBirth.Year, 3, 21),   // Aries
                new DateTime(_dateOfBirth.Year, 4, 20),   // Taurus
                new DateTime(_dateOfBirth.Year, 5, 21),   // Gemini
                new DateTime(_dateOfBirth.Year, 6, 21),   // Cancer
                new DateTime(_dateOfBirth.Year, 7, 23),   // Leo
                new DateTime(_dateOfBirth.Year, 8, 23),   // Virgo
                new DateTime(_dateOfBirth.Year, 9, 23),   // Libra
                new DateTime(_dateOfBirth.Year, 10, 23),  // Scorpio
                new DateTime(_dateOfBirth.Year, 11, 22),  // Sagittarius
                new DateTime(_dateOfBirth.Year, 12, 22),  // Capricorn

            };

            string[] zodiacSigns = { "Capricorn", "Aquarius", "Pisces", "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Sagittarius" };

            for (int i = 0; i < sunSignDates.Length; i++)
            {
                if (_dateOfBirth < sunSignDates[i]) return zodiacSigns[i];
            }

            return zodiacSigns[0];
        }

        public string CalculateChineseSign()
        {
            int yearOfZodiac = _dateOfBirth.Year % 12;

            string[] zodiacSigns = { "Monkey", "Rooster", "Dog", "Pig", "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat" };

            return zodiacSigns[yearOfZodiac];
        }
    }
}

