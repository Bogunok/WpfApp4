using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PersonViewModel pvm;
        public MainWindow()
        {
            InitializeComponent();
            pvm = new PersonViewModel(this);
            DataContext = pvm;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            pvm.Mode = 1;
            ModeLabel.Content = "Mode : Add";
            NameInput.Clear();
            SurnameInput.Clear();
            MailInput.Clear();
            BirthdatePicker.SelectedDate = null;
        }

        private void EditPerson_Click(object sender, RoutedEventArgs e)
        {
            pvm.Mode = 2;
            if (Table.SelectedIndex == -1) return;
            pvm.PersonIndex = Table.SelectedIndex;
            ModeLabel.Content = "Mode : Edit ["+pvm.PersonIndex+"]";
            NameInput.Text = pvm.People[pvm.PersonIndex].Name;
            SurnameInput.Text = pvm.People[pvm.PersonIndex].Surname;
            MailInput.Text = pvm.People[pvm.PersonIndex].Mail;
            BirthdatePicker.SelectedDate = pvm.People[pvm.PersonIndex].Birthdate;
        }

        private void DeletePerson_Click(object sender, RoutedEventArgs e)
        {
            foreach(var person in Table.SelectedItems)
            {
                pvm.People.Remove((PersonModel)person);
            }
            Table.Items.Refresh();
        }

        private void SavePerson_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                string path = System.IO.Path.Combine(projectDir, "people.json");
                using (FileStream fs = File.Create(path))
                {
                    JsonSerializer.SerializeAsync(fs, pvm.People).Wait();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            pvm.FilteredPeople.Clear();
            DateTime from;
            if (FromBirthdatePicker.SelectedDate.HasValue)
            {
                from = FromBirthdatePicker.SelectedDate.Value;
            }
            else
            {
                from = DateTime.MinValue;
            }

            DateTime to;
            if (ToBirthdatePicker.SelectedDate.HasValue)
            {
                to = ToBirthdatePicker.SelectedDate.Value;
            }
            else
            {
                to = DateTime.MaxValue;
            }
            string namePrefix = FilterNameInput.Text;
            string surnamePrefix = FilterSurnameInput.Text;
            string mailPrefix = FilterMailInput.Text;
            string domain = FilterDomainInput.Text;
            string sunSign = ((ComboBoxItem)FilterSunSign.SelectedItem).Content.ToString();
            string chineseSign = ((ComboBoxItem)FilterChineseSign.SelectedItem).Content.ToString();
            string isAdult = ((ComboBoxItem)FilterAge.SelectedItem).Content.ToString();
            bool ifAdult = false;
            if (isAdult.Equals("Child")) ifAdult = false;
            else if (isAdult.Equals("Adult")) ifAdult = true;
            string isBirthday = ((ComboBoxItem)FilterBirthday.SelectedItem).Content.ToString();
            bool ifBirthday = false;
            if (isBirthday.Equals("Is birthday")) ifBirthday = true;
            else if (isBirthday.Equals("Not birthday")) ifBirthday = false;
            foreach(PersonModel person in pvm.People)
            {
                if (!string.IsNullOrWhiteSpace(namePrefix) && !person.Name.StartsWith(namePrefix)) continue;
                if (!string.IsNullOrWhiteSpace(surnamePrefix) && !person.Surname.StartsWith(surnamePrefix)) continue;
                if (!string.IsNullOrWhiteSpace(mailPrefix) && !person.Mail.StartsWith(mailPrefix)) continue;
                string expectedDomain = person.Mail.Split("@")[1];
                if (!string.IsNullOrWhiteSpace(domain) && !domain.Equals(expectedDomain)) continue;
                if (!sunSign.Equals("Any") && !person.SunSign.Equals(sunSign)) continue;
                if (!chineseSign.Equals("Any") && !person.ChineseSign.Equals(chineseSign)) continue;
                if (!isAdult.Equals("Any") && person.IsAdult != ifAdult) continue;
                if (!isBirthday.Equals("Any") && person.IsBirthday != ifBirthday) continue;
                if (person.Birthdate < from || person.Birthdate > to) continue;
                pvm.FilteredPeople.Add(person);
            }
            Table.ItemsSource = pvm.FilteredPeople;
            Table.Items.Refresh();
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            FromBirthdatePicker.SelectedDate = null;
            ToBirthdatePicker.SelectedDate = null;
            FilterNameInput.Clear();
            FilterSurnameInput.Clear();
            FilterMailInput.Clear();
            FilterDomainInput.Clear();
            FilterSunSign.SelectedIndex = 0;
            FilterChineseSign.SelectedIndex = 0;
            FilterAge.SelectedIndex = 0;
            FilterBirthday.SelectedIndex = 0;
            Table.ItemsSource = pvm.People;
        }
    }
}
