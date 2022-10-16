using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Collections.ObjectModel;
using Syncfusion.UI.Xaml.Grid;
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
        private ObservableCollection<Task> AllTasks { get; set; }
        private List<string> GridCBItems { get; set; }
        private List<string> TaskIDs { get; set; }
        private bool EditingTime { get; set; }
        private bool EditingTaskDescription { get; set; }
        public static string DB_PATH { get; set; }

        public MainPage()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzM5Mjc2QDMyMzAyZTMzMmUzMEdDNGN1V0JBRWVQUGVRZENERnBSalQ5VS9iTC9yeEMxS2RHMXFoMHhIMDA9");
            this.InitializeComponent();

            AllTasks = new ObservableCollection<Task>();
            TaskIDs = new List<string>();
            GridCBItems = new List<string>();
            StartConfiguration();
        }

        public async void StartConfiguration()
        {
            DB_PATH = "";
            EditingTime = false;
            EditingTaskDescription = false;

            GridCBItems.Add("High");
            GridCBItems.Add("Normal");
            GridCBItems.Add("Low");

            try
            {
                LoadAllTasks();
            }
            catch (Exception ex) { await new MessageDialog(ex.ToString(), "Loading Tasks Error").ShowAsync(); }
            MainTable.ItemsSource = AllTasks;
            FillCategoryComboBox();
            UpdateUpcomingTaskLabel();
            MainTable.AllowResizingColumns = true;
        }

        private void LoadAllTasks()
        {
            foreach (string task in TasksStorage.LoadTasks())
            {
                if (task.Length > 0 && task[0] != 'C') AllTasks.Add(TasksStorage.ParseStringToTask(task));
            }
            EnumerateTaskIDs();
            MainTable.UpdateLayout();
        }

        public void EnumerateTaskIDs()
        {
            TaskIDs.Clear();
            foreach(var row in AllTasks) TaskIDs.Add(row.TaskID);
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
            TaskIDs.Add(newTask.TaskID);
            MainTable.UpdateLayout();
            SaveButton.IsEnabled = true;
            FilterButton.IsEnabled = false;
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
                _ = TaskIDs.Remove(ID);
                MainTable.UpdateLayout();
            }
            catch
            {
                await new MessageDialog("A Task must be selected for it to be deleted.", "No Selection Error").ShowAsync();
            }
            FilterButton.IsEnabled = false;
            SaveButton.IsEnabled = true;
        }

        private void MainTable_AutoGeneratingColumn(object sender, AutoGeneratingColumnArgs e)
        {
            if (e.Column.HeaderText == "Complete")
            {
                //e.Column.Width = 90;
                e.Column.AllowEditing = true;
            }
            if (e.Column.HeaderText == "Category")
            {
                //e.Column.Width = 130;
                e.Column.AllowEditing = true;
                e.Column.TextAlignment = TextAlignment.Center;
                e.Column.AllowSorting = true;
            }
            if (e.Column.HeaderText == "TaskDescription")
            {
                e.Column.HeaderText = "Task Description";
                //e.Column.Width = 200;
                e.Column.AllowEditing = true;
            }
            if (e.Column.HeaderText == "CreationDate")
            {
                GridDateTimeColumn gdtc = new GridDateTimeColumn();
                gdtc.MappingName = "CreationDate";
                gdtc.HeaderText = "Creation Date";
                gdtc.AllowEditing = false;
                //gdtc.Width = 200;
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
                //gdtc.Width = 200;
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
                //gtc.Width = 100;
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
                //gcbc.Width = 100;
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
            TasksStorage.DeleteStorageFile();
            foreach (Task myTask in AllTasks) TasksStorage.SaveTask(myTask);
            SaveButton.IsEnabled = false;
            FilterButton.IsEnabled = true;
            UpdateUpcomingTaskLabel();
        }

        private int PriorityNum(string priority)
        {
            switch (priority)
            {
                case "High":
                    return 2;
                case "Normal":
                    return 1;
            }
            return 0;
        }

        private void UpdateUpcomingTaskLabel()
        {
            if (AllTasks.Count > 0)
            {
                Task result = AllTasks[0];
                foreach (Task task in AllTasks)
                {
                    if (task.DueDate < result.DueDate) result = task;
                    else if (task.DueDate == result.DueDate)
                    {
                        if (Convert.ToDateTime(task.DueTime) < Convert.ToDateTime(result.DueTime)) result = task;
                        else if (Convert.ToDateTime(task.DueTime) == Convert.ToDateTime(result.DueTime))
                        {
                            if (PriorityNum(task.Priority) > PriorityNum(result.Priority)) result = task;
                        }
                    }
                }
                UpcomingLabel.Text = "Upcoming Task: " + result.TaskDescription;
            }
            else UpcomingLabel.Text = "No tasks left";
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

            else if (EditingTaskDescription)
            {
                try
                {
                    var reflector = MainTable.View.GetPropertyAccessProvider();
                    foreach (var row in MainTable.SelectedItems)
                    {
                        foreach (var column in MainTable.Columns)
                        {
                            if (column.HeaderText == "Task Description")
                            {
                                string td = reflector.GetValue(row, column.MappingName).ToString();
                                if (td.IndexOf(',') != -1)
                                {
                                    await new MessageDialog("No comma allowed in the task description.", "Task Description Error").ShowAsync();
                                    reflector.SetValue(row, column.MappingName, "");
                                }
                            }
                        }
                    }
                }
                catch { }
                EditingTaskDescription = false;
            }
            int SelectedRow = MainTable.SelectedIndex;
            AllTasks[SearchObservableCollection(AllTasks, GetTaskIDFromMainTable())] = AllTasks[SelectedRow];

            FillCategoryComboBox();
        }

        private void FillCategoryComboBox()
        {
            HashSet<string> uniqueCategories = new HashSet<string>();
            foreach (Task task in AllTasks) uniqueCategories.Add(task.Category);

            CategoryComboBox.ItemsSource = uniqueCategories;
        }

        private void MainTable_CurrentCellBeginEdit(object sender, CurrentCellBeginEditEventArgs e)
        {
            if (e.Column.HeaderText == "Due Time") EditingTime = true;
            else if (e.Column.HeaderText == "Task Description") EditingTaskDescription = true;
            FilterButton.IsEnabled = false;
            SaveButton.IsEnabled = true;
        }

        private void MainTable_CurrentCellValueChanged(object sender, CurrentCellValueChangedEventArgs e)
        {
            
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilterButton.Label == "Reset Filters")
            {
                AllTasks.Clear();
                LoadAllTasks();
            }
            else
            {
                //Delete Tasks for each Filter
                if (CreationDatePicker.Date.HasValue)
                {
                    foreach (Task task in AllTasks.ToList())
                    {
                        if (new DateTimeOffset?(task.CreationDate.Date).Value.Date != CreationDatePicker.Date.Value.Date) AllTasks.Remove(task);
                    }
                }

                if (DueDatePicker.Date.HasValue)
                {
                    foreach (Task task in AllTasks.ToList())
                    {
                        if (new DateTimeOffset?(task.DueDate.Date).Value.Date != DueDatePicker.Date.Value.Date) AllTasks.Remove(task);
                    }
                }

                if (PriorityComboBox.SelectedIndex != -1)
                {
                    string value = ((ContentControl)PriorityComboBox.SelectedItem).Content.ToString();
                    foreach (Task task in AllTasks.ToList())
                    {
                        if (task.Priority != value) AllTasks.Remove(task);
                    }
                }

                if (PendingComboBox.SelectedIndex != -1)
                {
                    bool complete = true;
                    if (((ContentControl)PendingComboBox.SelectedItem).Content.ToString() == "Complete") complete = true;
                    else complete = false;
                    foreach (Task task in AllTasks.ToList())
                    {
                        if (task.Complete != complete) AllTasks.Remove(task);
                    }
                }

                if (CategoryComboBox.SelectedIndex != -1)
                {
                    string value = CategoryComboBox.Items[CategoryComboBox.SelectedIndex].ToString();
                    foreach (Task task in AllTasks.ToList())
                    {
                        if (task.Category != value) AllTasks.Remove(task);
                    }
                }
            }

            if (FilterButton.Label == "Apply Filters")
            {
                FilterButton.Icon = new SymbolIcon(Symbol.Refresh);
                FilterButton.Label = "Reset Filters";
                CreateButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                SaveButton.IsEnabled = false;
            }
            else 
            {
                FilterButton.Icon = new SymbolIcon(Symbol.Filter);
                FilterButton.Label = "Apply Filters";
                CreateButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                SaveButton.IsEnabled = true;
            }
            MainTable.UpdateLayout();
        }

        private void CategoryLabel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            CategoryComboBox.SelectedItem = null;
        }

        private void PendingLabel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            PendingComboBox.SelectedItem = null;
        }

        private void PriorityLabel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            PriorityComboBox.SelectedItem = null;
        }

        private void DueDateLabel_SelectionChanged(object sender, RoutedEventArgs e)
        {
            DueDatePicker.Date = null;
        }

        private void CreationDateLabel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            CreationDatePicker.Date = null;
        }
    }
}
