using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DOList
{
    public class Task
    {
        private String name;
        private String category;
        private String priority;
        private DateTime date;
        private Boolean alarm;
        private Boolean complete;

        public Task() 
        {
            name = "Default";
            category = "Privát";
            priority = "!!";
            date = DateTime.Now;
            alarm = false;
            complete = true;
        }

        public Task(String name, String category, String priority, DateTime date, Boolean alarm) 
        {
            this.name = name;
            this.category = category;
            this.priority = priority;
            this.date = date;
            this.alarm = alarm;
        }

        public String getName() 
        {
            return (name);
        }

        public String getCategory()
        {
            return (category);
        }

        public String getPriority()
        {
            return (priority);
        }

        public DateTime getDate()
        {
            return (date);
        }

        public Boolean isAlarm()
        {
            return (alarm);
        }

        public Boolean isComplete()
        {
            return (complete);
        }

        public void setName(String name) 
        {
            this.name = name;
        }

        public void setCategory(String category)
        {
            this.category = category;
        }

        public void setPriority(String priority)
        {
            this.priority = priority;
        }

        public void setDate(DateTime date)
        {
            this.date = date;
        }

        public void setAlarm(Boolean alarm)
        {
            this.alarm = alarm;
        }

        public void setComplete(Boolean complete)
        {
            this.complete = complete;
        }

        public int CompareTo(Task task)
        {
            if (this.name.ToUpper().CompareTo(task.getName().ToUpper()) < 0)
            {
                return (-1);
            }
            else if (this.name.ToUpper().CompareTo(task.getName().ToUpper()) > 0)
            {
                return (1);
            }
            else
            {
                return (0);
            }
        }
    }
}
