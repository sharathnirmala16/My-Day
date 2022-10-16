using System;

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
