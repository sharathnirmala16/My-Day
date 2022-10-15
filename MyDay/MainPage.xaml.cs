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
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml;
using System.Data;
using Syncfusion.UI.Xaml.Controls.Input;
using Windows.UI.Popups;

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
        private List<String> GridCBItems { get; set; }
        private bool EditingTime { get; set; }
        public static string DB_PATH { get; set; }

        public MainPage()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzM5Mjc2QDMyMzAyZTMzMmUzMEdDNGN1V0JBRWVQUGVRZENERnBSalQ5VS9iTC9yeEMxS2RHMXFoMHhIMDA9");
            this.InitializeComponent();

            DB_PATH = "";
            AllTasks = new ObservableCollection<Task>();
            VisibleTasks = new ObservableCollection<Task>();
            GridCBItems = new List<String>();
            StartConfiguration();
        }

        public async void StartConfiguration()
        {
            MainTable.ItemsSource = VisibleTasks;

            EditingTime = false;

            CreationDateCheckBox.IsChecked = false;
            DueDateCheckBox.IsChecked = false;
            PriorityCheckBox.IsChecked = false;
            PendingCheckBox.IsChecked = false;
            CategoryCheckBox.IsChecked = false;

            CreationDatePicker.IsEnabled = false;
            DueDatePicker.IsEnabled = false;
            PriorityComboBox.IsEnabled = false;
            PendingComboBox.IsEnabled = false;
            CategoryComboBox.IsEnabled = false;

            GridCBItems.Add("High");
            GridCBItems.Add("Normal");
            GridCBItems.Add("Low");

            try
            {
                
            }
            catch (Exception ex) { await new MessageDialog(ex.ToString(), "Loading Tasks Error").ShowAsync(); }
        }

        private string RandomAlphaNumeric(int size = 8)
        {
            string res = "";
            for (int i = 0; i < size; i++)
            {
                if (Convert.ToBoolean(new Random().Next(0, 2))) res += (char)new Random().Next(97, 123);
                else res += (char)new Random().Next(48, 58);
            }
            return res;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Task newTask = new Task()
            {
                Complete = false,
                Category = "Category",
                TaskDescription = "Task Description",
                CreationDate = DateTime.Today,
                DueDate = DateTime.Today,
                DueTime = DateTime.Now.ToString("HH:mm"),
                Priority = "Normal",
                TaskID = RandomAlphaNumeric(),
            };

            AllTasks.Add(newTask);
            VisibleTasks.Add(newTask);
            MainTable.UpdateLayout();
        }

        private int SearchObservableCollection(ObservableCollection<Task> list, string ID)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].TaskID == ID) return i;
            }
            return -1;
        }

        private string GetTaskIDFromMainTable()
        {
            try
            {
                return MainTable.View.GetPropertyAccessProvider().GetValue(MainTable.SelectedItem, MainTable.Columns[MainTable.Columns.Count - 1].MappingName).ToString();
            }
            catch { return ""; }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ID = GetTaskIDFromMainTable();
                AllTasks.RemoveAt(SearchObservableCollection(AllTasks, ID));
                VisibleTasks.RemoveAt(MainTable.SelectedIndex);
                MainTable.UpdateLayout();
            }
            catch
            {
                await new MessageDialog("A Task must be selected for it to be deleted.", "No Selection Error").ShowAsync();
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

        private void CategoryCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryCheckBox.IsChecked == true) CategoryComboBox.IsEnabled = true;
            else CategoryComboBox.IsEnabled = false;
        }

        private void MainTable_AutoGeneratingColumn(object sender, AutoGeneratingColumnArgs e)
        {
            if (e.Column.HeaderText == "Complete")
            {
                e.Column.Width = 90;
                e.Column.AllowEditing = true;
            }
            if (e.Column.HeaderText == "Category")
            {
                e.Column.Width = 130;
                e.Column.AllowEditing = true;
                e.Column.TextAlignment = TextAlignment.Center;
                e.Column.AllowSorting = true;
            }
            if (e.Column.HeaderText == "TaskDescription")
            {
                e.Column.HeaderText = "Task Description";
                e.Column.Width = 800;
                e.Column.AllowEditing = true;
            }
            if (e.Column.HeaderText == "CreationDate")
            {
                GridDateTimeColumn gdtc = new GridDateTimeColumn();
                gdtc.MappingName = "CreationDate";
                gdtc.HeaderText = "Creation Date";
                gdtc.AllowEditing = false;
                gdtc.Width = 200;
                gdtc.FormatString = "dd/MM/yyyy";
                gdtc.TextAlignment = TextAlignment.Center;
                gdtc.AllowSorting = true;
                e.Column = gdtc;
            }
            if (e.Column.HeaderText == "DueDate")
            {
                GridDateTimeColumn gdtc = new GridDateTimeColumn();
                gdtc.MappingName = "DueDate";
                gdtc.HeaderText = "Due Date";
                gdtc.AllowEditing = true;
                gdtc.Width = 200;
                gdtc.FormatString = "dd/MM/yyyy";
                gdtc.TextAlignment = TextAlignment.Center;
                gdtc.AllowSorting = true;
                e.Column = gdtc;
            }
            if (e.Column.HeaderText == "DueTime")
            {
                SfTimePicker timePicker = new SfTimePicker();

                GridTextColumn gtc = new GridTextColumn();
                gtc.MappingName = "DueTime";
                gtc.HeaderText = "Due Time";
                gtc.AllowEditing = true;
                gtc.Width = 100;
                gtc.TextAlignment = TextAlignment.Center;
                gtc.AllowSorting = true;
                e.Column = gtc;
            }
            if (e.Column.HeaderText == "Priority")
            {
                GridComboBoxColumn gcbc = new GridComboBoxColumn();
                gcbc.MappingName = "Priority";
                gcbc.HeaderText = "Priority";
                gcbc.AllowEditing = true;
                gcbc.Width = 100;
                gcbc.ItemsSource = GridCBItems;
                gcbc.TextAlignment = TextAlignment.Center;
                gcbc.AllowSorting = true;
                e.Column = gcbc;
            }
            if (e.Column.HeaderText == "TaskID")
            {
                e.Column.MaximumWidth = 0;
                e.Column.AllowEditing = false;
                e.Column.AllowSorting = false;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void MainTable_CurrentCellEndEdit(object sender, CurrentCellEndEditEventArgs e)
        {
            if (EditingTime)
            {
                try
                {
                    var reflector = MainTable.View.GetPropertyAccessProvider();
                    foreach (var row in MainTable.SelectedItems)
                    {
                        foreach (var column in MainTable.Columns)
                        {
                            if (column.HeaderText == "Due Time")
                            {
                                bool valid = true;
                                string inpTime = reflector.GetValue(row, column.MappingName).ToString();
                                if (inpTime.Length > 0)
                                {
                                    TimeSpan dummy;
                                    valid = TimeSpan.TryParse(inpTime, out dummy);
                                }
                                if (!valid)
                                {
                                    await new MessageDialog(inpTime + " is an invalid time, enter the correct time in 24 hour format", "Error").ShowAsync();
                                    reflector.SetValue(row, column.MappingName, "");
                                }
                            }
                        }
                    }
                }
                catch { }
                EditingTime = false;
            }
        }

        private void MainTable_CurrentCellBeginEdit(object sender, CurrentCellBeginEditEventArgs e)
        {
            if (e.Column.HeaderText == "Due Time") EditingTime = true;
        }

        private void MainTable_CurrentCellValueChanged(object sender, CurrentCellValueChangedEventArgs e)
        {
            int SelectedRow = MainTable.SelectedIndex;
            AllTasks[SearchObservableCollection(AllTasks, GetTaskIDFromMainTable())] = VisibleTasks[SelectedRow];
        }
    }
}
