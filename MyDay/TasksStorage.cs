using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MyDay
{
    class TasksStorage
    {
        public static string storageDir = "";
        public static string fileLoc = "";

        public TasksStorage()
        {
            InitializeStorage();
        }

        public async static void InitializeStorage()
        {
            storageDir = ApplicationData.Current.LocalFolder.Path;
            fileLoc = Path.Combine(storageDir, "MyTasks.csv");
            await ApplicationData.Current.LocalFolder.CreateFileAsync("MyTasks.csv", CreationCollisionOption.OpenIfExists);
        }

        public static string ConvertTaskToString(Task task)
        {
            string res = "";
            res += task.Complete.ToString() + "," + task.Category + "," + task.TaskDescription + "," + task.CreationDate.ToString() + ",";
            res += task.DueDate.ToString() + "," + task.DueTime.ToString() + "," + task.Priority + "," + task.TaskID + Environment.NewLine;
            return res;
        }

        public static void SaveTask(Task task)
        {
            File.AppendAllText(fileLoc, ConvertTaskToString(task));
        }

        public static Task ParseStringToTask(string myTask)
        {
            string[] cells = myTask.Split(',');
            return new Task()
            {
                Complete = Convert.ToBoolean(cells[0]),
                Category = cells[1],
                TaskDescription = cells[2],
                CreationDate = Convert.ToDateTime(cells[3]).Date,
                DueDate = Convert.ToDateTime(cells[4]).Date,
                DueTime = cells[5],
                Priority = cells[6],
                TaskID = cells[7],
            };
        }

        public static string[] LoadTasks()
        {
            string[] myTasks = File.ReadAllText(fileLoc).Split(Environment.NewLine);

            return myTasks;
        }

        public static void DeleteStorageFile()
        {
            File.Delete(fileLoc);
        }
    }
}
