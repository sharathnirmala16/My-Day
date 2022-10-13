using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.UI.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyDay
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Task> VisibleTasks { get; set; }
        private ObservableCollection<Task> AllTasks { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            AllTasks = new ObservableCollection<Task>();
            VisibleTasks = new ObservableCollection<Task>();
            StartConfiguration();
        }

        public void StartConfiguration()
        {
            CreationDateCheckBox.IsChecked = false;
            DueDateCheckBox.IsChecked = false;
            PriorityCheckBox.IsChecked = false;
            PendingCheckBox.IsChecked = false;

            CreationDatePicker.IsEnabled = false;
            DueDatePicker.IsEnabled = false;
            PriorityComboBox.IsEnabled = false;
            PendingComboBox.IsEnabled = false;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

            VisibleTasks.Add(new Task()
            {
                Complete = false, 
                Category = "Category", 
                TaskDescription = "Task Description",
            });
            MainTable.UpdateLayout();
        }


        async private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VisibleTasks.RemoveAt(MainTable.SelectedIndex);
                MainTable.UpdateLayout();
            }
            catch
            {
                await new Windows.UI.Popups.MessageDialog("A Task must be selected for it to be deleted.", "No Selection Error").ShowAsync();
            }
        }

        private void MainTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Complete")
            {
                e.Column.Width = DataGridLength.ConvertFromString("100");
            }
            if (e.Column.Header.ToString() == "Category")
            {
                e.Column.Width = DataGridLength.ConvertFromString("120");
            }
            if (e.Column.Header.ToString() == "TaskDescription")
            {
                e.Column.Header = "Task Description";
                e.Column.Width = DataGridLength.ConvertFromString("800");
            }
            if (e.Column.Header.ToString() == "CreatedDate")
            {
                e.Column.Header = "Created Date";
                e.Column.Width = DataGridLength.ConvertFromString("150");
            }
            if (e.Column.Header.ToString() == "DueDate")
            {
                e.Column.Header = "Due Date";
                e.Column.Width = DataGridLength.ConvertFromString("150");
            }
            if (e.Column.Header.ToString() == "DueTime")
            {
                e.Column.Header = "Due Time";
                e.Column.Width = DataGridLength.ConvertFromString("150");
            }
            if (e.Column.Header.ToString() == "Priority")
            {
                e.Column.Header = "Due Time";
                e.Column.Width = DataGridLength.ConvertFromString("150");
            }
        }

        private void CreationDateCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (CreationDateCheckBox.IsChecked == true) CreationDatePicker.IsEnabled = true;
            else CreationDatePicker.IsEnabled = false;
        }

        private void PriorityCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (PriorityCheckBox.IsChecked == true) PriorityComboBox.IsEnabled = true;
            else PriorityComboBox.IsEnabled = false;
        }

        private void DueDateCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (DueDateCheckBox.IsChecked == true) DueDatePicker.IsEnabled = true;
            else DueDatePicker.IsEnabled = false;
        }

        private void PendingCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (PendingCheckBox.IsChecked == true) PendingComboBox.IsEnabled = true;
            else PendingComboBox.IsEnabled = false;
        }
    }
}
