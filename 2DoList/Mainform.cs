using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace _2DOList
{
    public partial class Mainform : Form
    {
        public String fileName = "data.csv";
        public String fileNameDoneTasks = "done_task.csv";
        public String DateTimeFormat = "yyyy. MMMM dd. dddd HH:mm";
        public ArrayList events;
        public ArrayList reminders;

        public Mainform()
        {
            InitializeComponent();
        }

        private void Mainform_Load(object sender, EventArgs e)
        {
            events = new ArrayList();
            reminders = new ArrayList();

            lvTasks.GridLines = true;
            lvTasks.View = View.Details;
            lvTasks.Columns.Add("Prioritás", 80, HorizontalAlignment.Right);
            lvTasks.Columns.Add("Feladat", 300, HorizontalAlignment.Right);
            lvTasks.Columns.Add("Kategória", 80, HorizontalAlignment.Right);
            lvTasks.Columns.Add("Határidő", 180, HorizontalAlignment.Right);
            lvTasks.Columns.Add("Jelzés", 80, HorizontalAlignment.Right);

            LoadToDoList();
            MarkDoneTasks();
        }

        //Feladat hozzáadása a listview-hoz
        public void AddTaskToListView(Task task) 
        {
            String alarm = (task.isAlarm()) ? "Igen" : "Nem";

            ListViewItem lvi = new ListViewItem(task.getPriority());
            lvi.SubItems.Add(task.getName());
            lvi.SubItems.Add(task.getCategory());
            lvi.SubItems.Add(task.getDate().ToString(DateTimeFormat));
            lvi.SubItems.Add(alarm);
            lvTasks.Items.Add(lvi);
        }

        //Feladat szerkesztése a listview-ban
        public void EditTaksInListView(Task task, int index) 
        {
            String alarm = (task.isAlarm()) ? "Igen" : "Nem";
            lvTasks.Items[index].SubItems[0].Text = task.getPriority();
            lvTasks.Items[index].SubItems[1].Text = task.getName();
            lvTasks.Items[index].SubItems[2].Text = task.getCategory();
            lvTasks.Items[index].SubItems[3].Text = task.getDate().ToString(DateTimeFormat);
            lvTasks.Items[index].SubItems[4].Text = alarm;
        }

        //Feladat hozzáadása az adatszerkezethez
        public void AddTaskToArrayList(Task task) 
        {
            events.Add(task);
            //TODO: update events()
        }

        //Nem használt funkció
        public Task GetTask(String name) 
        {
            for (int i = 0; i < events.Count; i++) 
            {
                Task task = (Task)events[i];

                if (task != null) 
                {
                    if (task.getName().ToUpper().Equals(name.ToUpper())) 
                    {
                        return (task);
                    }
                }
            }

            return(null);
        }

        //Nem használt funkció
        public void UpdateEvents(Task task) 
        {
            for (int i = 0; i < events.Count; i++) 
            {
                if (task.Equals(events[i]))
                    events[i] = task;
            }
        }

        //Nem használt funkció
        private void UpdateReminders(Task task) 
        {
            for (int i = 0; i < reminders.Count; i++)
                if (task.Equals(reminders[i]))
                    reminders[i] = task;
        }

        //Nem használt funkció
        public ArrayList Sort(ArrayList events) 
        {
            int count = 0;
            foreach (Task task in events) 
            {
                if (task != null)
                    count++;
            }

            Task[] tmp = new Task[count];
            
            count = 0;

            for (int i = 0; i < events.Count; i++) 
            {
                if (events[i] != null) 
                {
                    tmp[count++] = (Task)events[i];
                }
            }

            for(int i = 0; i < count; i++)
                for (int j = (i + 1); j < count; j++) 
                {
                    if (tmp[i].CompareTo(tmp[j]) == 1) 
                    {
                        Task task = tmp[i];
                        tmp[i] = tmp[j];
                        tmp[j] = task;
                    }
                }

            ArrayList sorted = new ArrayList();

            foreach (Task task in events)
                sorted.Add(task);

            return (sorted);
        }

        //Fájl mentése paraméter alapján
        public void saveToDoList(String filename)
        {
            try
            {
                if (filename == "data.csv")
                {

                    StreamWriter file = new StreamWriter(filename, false, Encoding.Default);
                    foreach (ListViewItem lvi in lvTasks.Items)
                    {
                                        //prioritás                      //név                     //kategória                  //határidő                  //jelzés                 
                        String sor = lvi.SubItems[0].Text + ';' + lvi.SubItems[1].Text + ';' + lvi.SubItems[2].Text + ';' + lvi.SubItems[3].Text + ';' + lvi.SubItems[4].Text;
                        file.WriteLine(sor);
                    }
                    file.Close();
                }

                if (filename == "done_task.csv")
                {
                    StreamWriter file = new StreamWriter(filename, true, Encoding.Default);
                    foreach (ListViewItem lvi in lvTasks.SelectedItems)
                    {
                        String sor = lvi.SubItems[0].Text + ';' + lvi.SubItems[1].Text + ';' + lvi.SubItems[2].Text + ';' + lvi.SubItems[3].Text + ';' + DateTime.Now.ToString(DateTimeFormat) + ';' + lvi.SubItems[4].Text;
                        file.WriteLine(sor);
                    }
                    file.Close();
                }
            }
            catch (Exception e) 
            {
                MessageBox.Show("Hiba a fájl írásakor: " + e.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Fájl betöltése
        private void LoadToDoList()
        {
            try
            {
                if(File.Exists(fileName))
                {
                    StreamReader file = new StreamReader(fileName, Encoding.Default);
                    String sor;
                    while((sor = file.ReadLine()) != null)
                    {
                        String[] sorR = sor.Split(';');
                        Boolean alarm = (sorR[4] == "Igen") ? true : false;
                        Task task = new Task(sorR[1], sorR[2], sorR[0], Convert.ToDateTime(sorR[3]), alarm);
                        AddTaskToListView(task);
                        AddTaskToArrayList(task);
                    }
                    file.Close(); 
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hiba a fájl olvasásakor: " + e.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Lejárt feladatok kiszínezsése
        private void MarkDoneTasks() 
        {
            for (int i = 0; i < events.Count; ++i)
            {
                Task task = (Task)events[i];
                foreach (ListViewItem lvi in lvTasks.Items)
                {
                    DateTime taskDate = Convert.ToDateTime(lvi.SubItems[3].Text);
                    if (taskDate < task.getDate())
                        lvTasks.Items[lvi.Index].ForeColor = Color.DarkRed;
                }
            }
        }

        //Új feladat menü
        private void újFeladatToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddTask form = new AddTask(this);
            form.ShowDialog();
        }

        //Feladat szerkesztése menü
        private void feladatSzerkesztéseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EditTask form = new EditTask(this);
            form.ShowDialog();
        }

        //Kilépés menü
        private void kilépésToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveToDoList("data.csv");
            this.Close();
        }

        //Kilépés X gomb megnyomására
        private void Mainform_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveToDoList("data.csv");
        }

        //Feladat készre állítása dupla egérklikkre
        private void lvTasks_ItemActivate(object sender, EventArgs e)
        {
            saveToDoList("done_task.csv");
            foreach (ListViewItem lvi in lvTasks.SelectedItems)
                lvTasks.Items.Remove(lvi);
            saveToDoList("data.csv");
        }

        //Elkészült feladatok menü
        private void elkészültFeladatokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoneTask form = new DoneTask(this);
            form.ShowDialog();
        }

        //Hamarosan lejáró feladatok menü - nem elérhető ebben a verzióban
        private void hamarosanLejáróFeladatokToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ez a menü ebben a verzióban nem elérhető", "Work In Progress", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //ReminderTask form = new ReminderTask(this);
            //form.ShowDialog();
        }

        //Listview fejléceinek szélessége nem állítható
        private void lvTasks_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = lvTasks.Columns[e.ColumnIndex].Width;
        }

        //Emlékeztető a feladatokhoz
        private void timer1_Tick(object sender, EventArgs e)
        {
            for(int i = 0; i < events.Count; i++)
            {
                Task task = (Task)events[i];

                if(task != null)
                {
                    DateTime alarm = task.getDate();
                    DateTime now = DateTime.Now;

                    //Feladat lejárta előtt 5 perccel jelez
                    if (((alarm.Minute - now.Minute) == 5) && task.isAlarm())
                    {
                        task.setAlarm(false);
                        DialogResult result = MessageBox.Show("A következő feladat hamarosan lejár!\n\n" + task.getName() + "\n" + task.getCategory() + "\nHatáridő: " + task.getDate().ToString(DateTimeFormat) + "\nPrioritás: " + task.getPriority(), "2DOList - Emlékeztető", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (result == DialogResult.OK)
                        {
                            foreach (ListViewItem lvi in lvTasks.Items)
                                if (lvi.SubItems[1].Text == task.getName())
                                    lvTasks.Items[lvi.Index].ForeColor = Color.OrangeRed;
                            reminders.Add(task);
                        }
                    }

                    //Ha a feladat már lejárt
                    if (alarm < now && task.isAlarm()) 
                    {
                        task.setAlarm(false);
                        DialogResult result = MessageBox.Show("A következő feladat határideje lejárt!\n\n" + task.getName() + "\n" + task.getCategory() + "\nHatáridő: " + task.getDate().ToString(DateTimeFormat) + "\nPrioritás: " + task.getPriority(), "2DOList - Emlékeztető", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (result == DialogResult.OK)
                        {
                            foreach (ListViewItem lvi in lvTasks.Items)
                                if (lvi.SubItems[1].Text == task.getName())
                                    lvTasks.Items[lvi.Index].ForeColor = Color.DarkRed;
                            reminders.Add(task);
                        }
                    }
                }
            }
        }
    }
}
