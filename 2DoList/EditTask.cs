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
using System.Diagnostics;

namespace _2DOList
{
    public partial class EditTask : Form
    {
        //A MainForm elemeit veheti át
        Mainform MainForm { get; set; }
        public EditTask(Mainform _MainForm)
        {
            MainForm = _MainForm;
            InitializeComponent();
        }

        private void EditTask_Load(object sender, EventArgs e)
        {
            LoadTasks();
        }

        //Feladatok betöltése
        void LoadTasks() 
        {
            try
            {
                if (File.Exists("data.csv")) 
                {
                    StreamReader file = new StreamReader("data.csv", Encoding.Default);
                    String item;
                    while ((item = file.ReadLine()) != null) 
                    {
                        String[] sorR = item.Split(';');
                        cmbChoice.Items.Add(sorR[1]);
                    }
                    file.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hiba a feladatok betöltése közben: " + e.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Feladat kiválasztása
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (cmbChoice.SelectedIndex < 0)
                MessageBox.Show("Válassz feladatot", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else 
            {
                tbTask.Text = cmbChoice.SelectedItem.ToString();
                cmbCategory.Text = MainForm.lvTasks.Items[cmbChoice.SelectedIndex].SubItems[2].Text;
                cmbPriority.Text = MainForm.lvTasks.Items[cmbChoice.SelectedIndex].SubItems[0].Text;
                dtpDate.Value = Convert.ToDateTime(MainForm.lvTasks.Items[cmbChoice.SelectedIndex].SubItems[3].Text);

                if (MainForm.lvTasks.Items[cmbChoice.SelectedIndex].SubItems[4].Text == "Igen")
                    cbAlarm.Checked = true;
                else
                    cbAlarm.Checked = false;
            }
        }

        //Feladat szerkesztése
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (cmbChoice.SelectedIndex < 0)
                MessageBox.Show("Nincs feladat a listában", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (tbTask.TextLength <= 0)
                MessageBox.Show("A feladat nevének kitöltése kötelező", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (cmbCategory.SelectedIndex < 0)
                MessageBox.Show("Válassz kategóriát", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (cmbPriority.SelectedIndex < 0)
                MessageBox.Show("Válassz prioritást", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (dtpDate.Value < DateTime.Now)
                MessageBox.Show("A feladat határidejét rosszul adtad meg", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else 
            {
                int index = cmbChoice.SelectedIndex;
                String name = tbTask.Text.Trim();
                String category = cmbCategory.SelectedItem.ToString().Trim();
                String priority = cmbPriority.SelectedItem.ToString().Trim();
                DateTime date = dtpDate.Value;
                Boolean alarm = cbAlarm.Checked;

                //TODO: task frissítése az events ArrayListben
                Task editedTask = new Task(name, category, priority, date, alarm);

                MainForm.EditTaksInListView(editedTask, index);
                
                //MainForm.AddTaskToArrayList(editedTask);
                //MainForm.UpdateEvents(editedTask);

                MessageBox.Show("Feladat sikeresen szerkesztve", "Sikeres szerkesztés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        //Feladat törlése
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbChoice.Items.Count != 0)
            {
                DialogResult result = MessageBox.Show("Valóban törölni szeretnéd a feladatot?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    MainForm.lvTasks.Items.RemoveAt(cmbChoice.SelectedIndex);
                    MessageBox.Show("Feladat törölve", "Sikeres törlés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MainForm.saveToDoList("data.csv");
                    this.Close();
                }
            }
            else
                MessageBox.Show("Nincs feladat a listában", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
