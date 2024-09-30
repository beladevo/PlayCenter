using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Calender.Services;
using Calender.UserControls;

namespace Calender
{
    public partial class MainWindow : Window
    {
        private readonly DataService _dataService;
        private List<CalendarItem> _calendarItems;
        private CalendarItem _itemBeingEdited = null;
        private bool _isTimeTextChanging = false;

        public MainWindow()
        {
            InitializeComponent();
            _dataService = new DataService();
            LoadData();
            DateTime today = DateTime.Today;
            MyCalendar.SelectedDate = today;
            UpdateDateDisplay(today);
        }

        private async void LoadData()
        {
            _calendarItems = await _dataService.LoadDataAsync();
            DisplayCalendarItems(MyCalendar.SelectedDate ?? DateTime.Today);
        }

        private void DisplayCalendarItems(DateTime selectedDate)
        {
            calendarItemsPanel.Children.Clear();

            var itemsForDate = _calendarItems
                .Where(item => item.Time.Date == selectedDate.Date)
                .ToList();

            int totalTasks = itemsForDate.Count;
            TasksCounterTextBlock.Text = $"{totalTasks} task{(totalTasks != 1 ? "s" : "")}";


            foreach (var item in itemsForDate)
            {
                var calendarItemControl = new Item
                {
                    ItemId = item.ItemId,
                    Title = item.Title,
                    Time = item.NotificationHour,
                    Color = new SolidColorBrush(item.IsChecked ? Colors.LightPink : Colors.White),
                    Icon = item.IsChecked ? FontAwesome.WPF.FontAwesomeIcon.CheckCircle : FontAwesome.WPF.FontAwesomeIcon.CircleThin,
                    IconBell = item.IsMuted ? FontAwesome.WPF.FontAwesomeIcon.BellSlash : FontAwesome.WPF.FontAwesomeIcon.Bell
                };

                calendarItemControl.DeleteItem += CalendarItemControl_DeleteItem;
                calendarItemControl.CheckItem += CalendarItemControl_CheckItem;
                calendarItemControl.MuteItem += CalendarItemControl_MuteItem;
                calendarItemControl.EditItem += CalendarItemControl_EditItem;

                calendarItemsPanel.Children.Add(calendarItemControl);
            }
        }

        private async void CalendarItemControl_DeleteItem(object sender, int itemId)
        {
            var item = _calendarItems.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null)
            {
                _calendarItems.Remove(item);
                await _dataService.SaveDataAsync(_calendarItems);
                DisplayCalendarItems(MyCalendar.SelectedDate ?? DateTime.Today);
            }
            else
            {
                MessageBox.Show("Item not found.");
            }
        }

        private async void CalendarItemControl_CheckItem(object sender, int itemId)
        {
            var item = _calendarItems.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null)
            {
                item.IsChecked = !item.IsChecked;
                await _dataService.SaveDataAsync(_calendarItems);
                DisplayCalendarItems(MyCalendar.SelectedDate ?? DateTime.Today);
            }
            else
            {
                MessageBox.Show("Item not found.");
            }
        }

        private async void CalendarItemControl_MuteItem(object sender, int itemId)
        {
            var item = _calendarItems.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null)
            {
                item.IsMuted = !item.IsMuted;
                await _dataService.SaveDataAsync(_calendarItems);
                DisplayCalendarItems(MyCalendar.SelectedDate ?? DateTime.Today);
            }
            else
            {
                MessageBox.Show("Item not found.");
            }
        }

        private void CalendarItemControl_EditItem(object sender, int itemId)
        {
            _itemBeingEdited = _calendarItems.FirstOrDefault(i => i.ItemId == itemId);

            if (_itemBeingEdited != null)
            {
                txtNote.Text = _itemBeingEdited.Title;
                txtTime.Text = _itemBeingEdited.NotificationHour;
                btnAddIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Save;
                txtNote.Focus();
            }
            else
            {
                MessageBox.Show("Item not found.");
            }
        }
        private async void AddNewItem(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtNote.Text))
            {
                MessageBox.Show("Please enter a note.");
                return;
            }

            if (!TimeSpan.TryParseExact(txtTime.Text, "hh\\:mm", null, out TimeSpan notificationTime))
            {
                MessageBox.Show("Please enter a valid time in HH:mm format.");
                return;
            }

            DateTime selectedDate = MyCalendar.SelectedDate ?? DateTime.Today;

            if (_itemBeingEdited != null)
            {
                _itemBeingEdited.Title = txtNote.Text;
                _itemBeingEdited.NotificationHour = txtTime.Text;
                _itemBeingEdited.Time = selectedDate; 
                await _dataService.SaveDataAsync(_calendarItems);
                DisplayCalendarItems(selectedDate);
                txtNote.Text = string.Empty;
                txtTime.Text = string.Empty;
                lblNote.Visibility = Visibility.Visible;
                lblTime.Visibility = Visibility.Visible;
                btnAddIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.PlusCircle;
                _itemBeingEdited = null;
            }
            else
            {
                var newItem = new CalendarItem
                {
                    ItemId = _dataService.GetNextItemId(_calendarItems),
                    Title = txtNote.Text,
                    Time = selectedDate,
                    NotificationHour = txtTime.Text,
                    IsChecked = false,
                    IsMuted = false
                };

                _calendarItems.Add(newItem);
                await _dataService.SaveDataAsync(_calendarItems);
                DisplayCalendarItems(selectedDate);
                txtNote.Text = string.Empty;
                txtTime.Text = string.Empty;
                lblNote.Visibility = Visibility.Visible;
                lblTime.Visibility = Visibility.Visible;
            }
        }
        private void txtTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
         
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                return;
            }

            
            string currentText = txtTime.Text.Remove(txtTime.SelectionStart, txtTime.SelectionLength);
            currentText = currentText.Insert(txtTime.SelectionStart, e.Text);

        
            string input = currentText.Replace(":", "");

        
            if (input.Length > 4)
            {
                e.Handled = true;
                return;
            }

            e.Handled = false;
        }

        private void txtTime_PreviewKeyDown(object sender, KeyEventArgs e)
        {
          
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                int selectionStart = txtTime.SelectionStart;

                if (selectionStart > 0)
                {
                 
                    string text = txtTime.Text.Remove(selectionStart - 1, 1);
                    txtTime.Text = text;
                    txtTime.SelectionStart = selectionStart - 1;
                }

                e.Handled = true;
            }
        }

        private void txtTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isTimeTextChanging)
                return;

            _isTimeTextChanging = true;

            lblTime.Visibility = string.IsNullOrEmpty(txtTime.Text) ? Visibility.Visible : Visibility.Collapsed;

            string text = txtTime.Text.Replace(":", "");
            int selectionStart = txtTime.SelectionStart;

         
            if (text.Length == 4)
            {
                text = text.Insert(2, ":");
            }

            txtTime.Text = text;
            txtTime.SelectionStart = selectionStart;

            if (TimeSpan.TryParseExact(txtTime.Text, "hh\\:mm", null, out TimeSpan time))
            {
                txtTime.Foreground = Brushes.Black;
            }
            else
            {
                txtTime.Foreground = Brushes.Red;
            }

            _isTimeTextChanging = false;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void lblNote_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNote.Focus();
        }

        private void lblTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtTime.Focus();
        }

        private void txtNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblNote.Visibility = string.IsNullOrEmpty(txtNote.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyCalendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = MyCalendar.SelectedDate.Value;
                UpdateDateDisplay(selectedDate);
            }
        }

        private void UpdateDateDisplay(DateTime date)
        {
            DayTextBlock.Text = date.Day.ToString();
            MonthTextBlock.Text = date.ToString("MMMM");
            DayOfWeekTextBlock.Text = date.ToString("dddd");
            DisplayCalendarItems(date);
        }

        private void PreviousDay_Click(object sender, RoutedEventArgs e)
        {
            if (MyCalendar.SelectedDate.HasValue)
            {
                DateTime previousDate = MyCalendar.SelectedDate.Value.AddDays(-1);
                MyCalendar.SelectedDate = previousDate;
                UpdateDateDisplay(previousDate);
            }
        }

        private void NextDay_Click(object sender, RoutedEventArgs e)
        {
            if (MyCalendar.SelectedDate.HasValue)
            {
                DateTime nextDate = MyCalendar.SelectedDate.Value.AddDays(1);
                MyCalendar.SelectedDate = nextDate;
                UpdateDateDisplay(nextDate);
            }
        }
    }
}
