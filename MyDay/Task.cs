using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MyDay
{
    public class Task
    {
        public bool Complete { get; set; }
        public String Category { get; set; }
        public String TaskDescription { get; set; }
        public DatePicker CreatedDate { get; set; }
        public DatePicker DueDate { get; set; }
        public TimePicker DueTime { get; set; }
        public ComboBox Priority { get; set; }
    }
}
