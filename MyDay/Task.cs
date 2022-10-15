using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Syncfusion.UI.Xaml.Controls;

namespace MyDay
{
    public class Task
    {
        public bool Complete { get; set; }
        public String Category { get; set; }
        public String TaskDescription { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public String DueTime { get; set; }
        public String Priority { get; set; }
        public String TaskID { get; set; }
    }
}
