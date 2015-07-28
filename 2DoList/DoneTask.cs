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
    public partial class DoneTask : Form
    {
        //A Mainform elemeit veheti át
        Mainform MainForm { get; set; }
        public DoneTask(Mainform _MainForm)
        {
            MainForm = _MainForm;
            InitializeComponent();
        }

        private void DoneTask_Load(object sender, EventArgs e)
        {
            lvDoneTask.Columns.Add("Feladat", 250, HorizontalAlignment.Right);
            lvDoneTask.Columns.Add("Kategória", 80, HorizontalAlignment.Right);
            lvDoneTask.Columns.Add("Határidő", 200, HorizontalAlignment.Right);
            lvDoneTask.Columns.Add("Elkészült", 200, HorizontalAlignment.Right);
            lvDoneTask.GridLines = true;
            lvDoneTask.View = View.Details;

            LoadDoneTasks();
        }

        //Feladat hozzáadása a listview-hoz
        private void AddFinishedTasks(Task task, DateTime dateCompleted) 
        {
            ListViewItem lvi = new ListViewItem(task.getName());
            lvi.SubItems.Add(task.getCategory());
            lvi.SubItems.Add(task.getDate().ToString(MainForm.DateTimeFormat));
            lvi.SubItems.Add(dateCompleted.ToString(MainForm.DateTimeFormat));
            
            lvDoneTask.Items.Add(lvi);
        }

        //Elkészült feladatok betöltése fájlból
        private void LoadDoneTasks() 
        {
            try
            {
                if(File.Exists(MainForm.fileNameDoneTasks))
                {
                    StreamReader file = new StreamReader(MainForm.fileNameDoneTasks, Encoding.Default);
                    String item;
                    while((item = file.ReadLine()) != null)
                    {
                        String[] sorR = item.Split(';');
                        Boolean alarm = (sorR[5] == "Igen") ? true : false;
                        DateTime dateCompleted = Convert.ToDateTime(sorR[4]);
                        Task task = new Task(sorR[1], sorR[2], sorR[0], Convert.ToDateTime(sorR[3]), alarm);
                        task.setComplete(true);
                        
                        AddFinishedTasks(task, dateCompleted);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hiba a fájl olvasása közben: " + e.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Listview fejlécei fix méretűek
        private void lvDoneTask_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = lvDoneTask.Columns[e.ColumnIndex].Width;
        }
    }
}
