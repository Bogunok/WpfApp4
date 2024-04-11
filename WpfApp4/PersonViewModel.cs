using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Text.Json;

namespace WpfApp4
{
    internal class PersonViewModel
    {
        private PersonModel _person;
        private List<PersonModel> _people;
        private List<PersonModel> _filteredPeople;
        private MainWindow _window;
        private int _mode = 0;
        private int _personIndex;

        public PersonViewModel(MainWindow mainWindow)
        {
            _person = new PersonModel("", "", "", DateTime.Now);
            _window = mainWindow;
            ProceedCommand = new RelayCommand(Proceed, CanProceed);
            _people = new List<PersonModel>();
            _filteredPeople = new List<PersonModel>();
            _window.Table.ItemsSource = _people;
            try
            {
                string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                string path = System.IO.Path.Combine(projectDir, "people.json");
                using (FileStream fs = File.OpenRead(path))
                {
                    _people = JsonSerializer.DeserializeAsync<List<PersonModel>>(fs).Result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            _window.Table.ItemsSource = null;
            _window.Table.ItemsSource = People; 
            if (People.Count == 0) FillTable();
        }

        public ICommand ProceedCommand { get; }

        public string Name
        {
            get { return _person.Name; }
            set { _person.Name = value; }
        }

        public string Surname
        {
            get { return _person.Surname; }
            set { _person.Surname = value; }
        }

        public string Mail
        {
            get { return _person.Mail; }
            set { _person.Mail = value; }
        }

        public DateTime Birthdate
        {
            get { return _person.Birthdate; }
            set { _person.Birthdate = value; }
        }

        public bool IsAdult
        {
            get { return _person.IsAdult; }
        }

        public string SunSign
        {
            get { return _person.SunSign; }
        }

        public string ChineseSign
        {
            get { return _person.ChineseSign; }
        }

        public bool IsBirthday
        {
            get { return _person.IsBirthday; }
        }

        public int Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public int PersonIndex
        {
            get { return _personIndex; }
            set { _personIndex = value; }
        }

        public List<PersonModel> People
        {
            get { return  _people; }
            set { _people = value; }
        }


        public List<PersonModel> FilteredPeople
        {
            get { return _filteredPeople; }
            set { _filteredPeople = value; }
        }

        private bool CanProceed(object parameter)
        {
            if (string.IsNullOrEmpty(_window.NameInput.Text)) return false;
            if (string.IsNullOrEmpty(_window.SurnameInput.Text)) return false;
            if (string.IsNullOrEmpty(_window.MailInput.Text)) return false;
            if (_mode == 0) return false;
            if (!_window.BirthdatePicker.SelectedDate.HasValue) return false;
            return true;
        }

        private async void Proceed(object parameter)
        {
            _window.PersonData.IsEnabled = false;
            _window.NameOutput.Text = "";
            _window.SurnameOutput.Text = "";
            _window.MailOutput.Text = "";
            _window.DateOfBirthOutput.Text = "";
            _window.IsAdultOutput.Text = "";
            _window.SunSignOutput.Text = "";
            _window.ChineseSignOutput.Text = "";
            _window.HappyBirthdayGreeting.Content = "";
            try
            {
                string name = _window.NameInput.Text;
                string surname = _window.SurnameInput.Text;
                string mail = _window.MailInput.Text;
                if (!CheckMail(mail)) throw new InvalidMail();
                DateTime date = _window.BirthdatePicker.SelectedDate.Value;
                if (CheckAge(date) < 0) throw new FutureBirthdayException();
                if (CheckAge(date) > 135) throw new FarInPastBirthday();
                await Task.Run(() => { _person = new PersonModel(name, surname, mail, date); });
                if (Mode == 1) People.Add(_person);
                if (Mode == 2) People[PersonIndex] = _person;
                _window.Table.Items.Refresh();
                _window.NameOutput.Text = Name;
                _window.SurnameOutput.Text = Surname;
                _window.MailOutput.Text = Mail;
                _window.DateOfBirthOutput.Text = Birthdate.ToShortDateString();
                _window.IsAdultOutput.Text = IsAdult ? "Yes" : "No";
                _window.SunSignOutput.Text = SunSign;
                _window.ChineseSignOutput.Text = ChineseSign;
                _window.HappyBirthdayGreeting.Content = IsBirthday ? "Happy birthday!" : "";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally { _window.PersonData.IsEnabled = true; }
        }

        private bool CheckMail(string mail)
        {
            if (!mail.Contains("@")) return false;
            if (!mail.EndsWith(".com") && !mail.EndsWith(".net")) return false;
            return true;
        }

        public int CheckAge(DateTime dateOfBirth)
        {
            DateTime current = DateTime.Now;
            int age = current.Year - dateOfBirth.Year;

            if (current.Month < dateOfBirth.Month || (current.Month == dateOfBirth.Month && current.Day < dateOfBirth.Day))
            {
                age--;
            }
            return age;
        }

        void FillTable()
        {
            Random random = new Random();

            string[] names = {
        "John", "Jane", "Michael", "Emily", "William", "Olivia", "James", "Emma", "Benjamin", "Ava",
        "Jacob", "Sophia", "Ethan", "Isabella", "Alexander", "Mia", "Daniel", "Charlotte", "Matthew", "Amelia",
        "David", "Harper", "Joseph", "Evelyn", "Samuel", "Abigail", "Henry", "Emily", "Jackson", "Elizabeth",
        "Sebastian", "Sofia", "Gabriel", "Chloe", "Andrew", "Grace", "Nathan", "Avery", "Ryan", "Ella",
        "Nicholas", "Madison", "Caleb", "Scarlett", "Luke", "Victoria", "Christian", "Aria", "Dylan", "Lily"
    };

            string[] lastNames = {
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
        "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin",
        "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
        "Walker", "Young", "Hall", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
        "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter"
    };

            string[] domains = {
        "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", 
        "aol.com", "icloud.com", "mail.com", "protonmail.com", "live.com"
    };
            List<DateTime> datesOfBirth = new List<DateTime>();
            for (int i = 0; i < 50; i++)
            {
                DateTime startDate = new DateTime(1980, 1, 1);
                int range = (DateTime.Today - startDate).Days;
                DateTime randomDate = startDate.AddDays(random.Next(range));
                datesOfBirth.Add(randomDate);
            }

            for (int i = 0; i < 50; i++)
            {
                string name = names[random.Next(names.Length)];
                string surname = lastNames[random.Next(lastNames.Length)];
                _people.Add(new PersonModel(name, surname, name + surname + "@" + domains[random.Next(domains.Length)], datesOfBirth[random.Next(datesOfBirth.Count)]));
            }

            _window.Table.Items.Refresh();
        }
    }
}
