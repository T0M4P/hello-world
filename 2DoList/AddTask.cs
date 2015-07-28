using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DOList
{
    public partial class AddTask : Form
    {
        //A Mainform elemeit használhatja
        public Mainform MainForm { get; set; }
        public AddTask(Mainform _Mainform)
        {
            MainForm = _Mainform;
            InitializeComponent();
        }

        private void AddTask_Load(object sender, EventArgs e)
        {
            tbTask.Text = "";
            cmbCategory.SelectedIndex = 0;
            cmbPriority.SelectedIndex = 1;
            dtpDate.Value = DateTime.Now;
            dtpDate.CustomFormat = MainForm.DateTimeFormat;
            cbAlarm.Checked = false;
        }

        //Feladat hozzáadása
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbTask.TextLength <= 0)
                MessageBox.Show("A feladat nevének kitöltése kötelező", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (cmbCategory.SelectedIndex < 0)
                MessageBox.Show("Válassz kategóriát", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (cmbPriority.SelectedIndex < 0)
                MessageBox.Show("Válassz prioritást", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (dtpDate.Value < DateTime.Now)
                MessageBox.Show("A feladat határidejét rosszul adtad meg", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                String name = tbTask.Text.Trim();
                String category = cmbCategory.SelectedItem.ToString().Trim();
                String priority = cmbPriority.SelectedItem.ToString().Trim();
                DateTime date = dtpDate.Value;
                Boolean alarm = cbAlarm.Checked;

                Task newTask = new Task(name, category, priority, date, alarm);
                
                MainForm.AddTaskToListView(newTask);
                MainForm.AddTaskToArrayList(newTask);
                
                MainForm.saveToDoList("data.csv");

                MessageBox.Show("Feladat sikeresen hozzáadva", "Sikeres hozzáadás", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
