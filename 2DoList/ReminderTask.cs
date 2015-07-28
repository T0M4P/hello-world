using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DOList
{
    public partial class ReminderTask : Form
    {
        public Mainform MainForm { get; set; }

        private String fileReminder = "reminder.csv";
       
        public ReminderTask(Mainform _MainForm)
        {
            MainForm = _MainForm;
            InitializeComponent();
        }

        private void ReminderTask_Load(object sender, EventArgs e)
        {
            lvReminder.GridLines = true;
            lvReminder.View = View.Details;
            lvReminder.Columns.Add("Prioritás", 100, HorizontalAlignment.Right);
            lvReminder.Columns.Add("Feladat", 320, HorizontalAlignment.Right);
            lvReminder.Columns.Add("Kategória", 100, HorizontalAlignment.Right);
            lvReminder.Columns.Add("Határidő", 200, HorizontalAlignment.Right);

            AddTaskToListView();
        }

        private void AddTaskToListView() 
        {
            foreach(Task task in MainForm.reminders)
            {
                String priority = task.getPriority();
                String name = task.getName();
                String category = task.getCategory();
                DateTime date = task.getDate();

                ListViewItem lvi = new ListViewItem(priority);
                lvi.SubItems.Add(name);
                lvi.SubItems.Add(category);
                lvi.SubItems.Add(date.ToString(MainForm.DateTimeFormat));
                lvReminder.Items.Add(lvi);
            }
        }

        private void AddTaskToListView(String priority, String name, String category, DateTime date) 
        {
            ListViewItem lvi = new ListViewItem(priority);
            lvi.SubItems.Add(name);
            lvi.SubItems.Add(category);
            lvi.SubItems.Add(date.ToString(MainForm.DateTimeFormat));
            lvReminder.Items.Add(lvi);
        }

        private void RemoveTaskFromListView() 
        {
            foreach (ListViewItem lvi in lvReminder.Items)
                if (lvi.SubItems[3].ToString() == DateTime.Now.ToString(MainForm.DateTimeFormat))
                    lvReminder.Items.Remove(lvi);
        }

        private void SaveReminder() 
        {
            try
            {
                if (File.Exists(fileReminder)) 
                {
                    StreamWriter file = new StreamWriter(fileReminder, true, Encoding.Default);
                    foreach (ListViewItem lvi in lvReminder.Items) 
                    {
                        String sor = lvi.SubItems[0].Text + ';' + lvi.SubItems[1].Text + ';' + lvi.SubItems[2].Text + ';' + lvi.SubItems[3].Text;
                        file.WriteLine(sor);
                    }
                    file.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hiba a fájl mentésekor:" + e.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReminder() 
        {
            try
            {
                if (File.Exists(fileReminder)) 
                {
                    StreamReader file = new StreamReader(fileReminder, Encoding.Default);
                    String sor;
                    while ((sor = file.ReadLine()) != null) 
                    {
                        String[] sorR = sor.Split(';');
                        AddTaskToListView(sorR[0], sorR[1], sorR[2], Convert.ToDateTime(sorR[3]));
                    }
                    file.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hiba a fájl betöltése közben: " + e.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
