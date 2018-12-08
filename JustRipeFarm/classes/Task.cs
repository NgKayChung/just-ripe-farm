using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Task
    {
        private int taskID;
        private string taskTitle;
        private string taskType;
        private string taskDescription;
        private string status;
        private DateTime startDateTime;
        private DateTime endDateTime;
        private string fieldID;
        private string cropID;
        private int methodID;
        private DateTime assignedDateTime;
        private string assignedByID;

        public Task()
        {

        }

        public Task(string task_title, string task_type, string task_desc, string stat, DateTime start_dateTime, DateTime end_dateTime, string field_id, string crop_id, int method_id, DateTime assigned_dateTime, string assignedBy_id)
        {
            taskTitle = task_title;
            taskType = task_type;
            taskDescription = task_desc;
            status = stat;
            startDateTime = start_dateTime;
            endDateTime = end_dateTime;
            fieldID = field_id;
            cropID = crop_id;
            methodID = method_id;
            assignedDateTime = assigned_dateTime;
            assignedByID = assignedBy_id;
        }

        public Task(int task_id, string task_title, string task_type, string task_desc, string stat, DateTime start_dateTime, DateTime end_dateTime, string field_id, string crop_id, int method_id, DateTime assigned_dateTime, string assignedBy_id)
        {
            taskID = task_id;
            taskTitle = task_title;
            taskType = task_type;
            taskDescription = task_desc;
            status = stat;
            startDateTime = start_dateTime;
            endDateTime = end_dateTime;
            fieldID = field_id;
            cropID = crop_id;
            methodID = method_id;
            assignedDateTime = assigned_dateTime;
            assignedByID = assignedBy_id;
        }

        public int TaskID { get => taskID; set => taskID = value; }
        public string TaskTitle { get => taskTitle; set => taskTitle = value; }
        public string TaskType { get => taskType; set => taskType = value; }
        public string TaskDescription { get => taskDescription; set => taskDescription = value; }
        public string Status { get => status; set => status = value; }
        public DateTime StartDateTime { get => startDateTime; set => startDateTime = value; }
        public DateTime EndDateTime { get => endDateTime; set => endDateTime = value; }
        public string FieldID { get => fieldID; set => fieldID = value; }
        public string CropID { get => cropID; set => cropID = value; }
        public int MethodID { get => methodID; set => methodID = value; }
        public DateTime AssignedDateTime { get => assignedDateTime; set => assignedDateTime = value; }
        public string AssignedByID { get => assignedByID; set => assignedByID = value; }
    }
}