using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessLayer;


namespace DVLD_PresentationLayer
{
    public partial class frmManagePeople : Form
    {
        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void _RefreshPeopleList()
        {
            dgvAllPeople.DataSource = clsPerson.GetAllPeople();
        }

        private void _GetPeopleCount()
        {
            int peopleCount = clsPerson.GetALLPeopleCount();
            lblRecords.Text = peopleCount.ToString();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            _RefreshPeopleList();
            _GetPeopleCount();
            comboBox1.SelectedIndex = 0;
        }

        private void icnAddNewPerson_Click(object sender, EventArgs e)
        {
            Form AddNewPerson = new frmAddNewPerson();
            AddNewPerson.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
