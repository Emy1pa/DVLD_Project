using DVLD_BusinessLayer;
using DVLD_PresentationLayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;


namespace DVLD_PresentationLayer
{
    public partial class frmAddNewPerson : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        int _PersonID;
        clsPerson _Person;

        public frmAddNewPerson(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;

            if (_PersonID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }

        private void _FillCountriesInComoboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {

                cbCountry.Items.Add(row["CountryName"]);

            }

        }
        public frmAddNewPerson()
        {
            InitializeComponent();
        }

       

        private void _LoadData()
        {
            _FillCountriesInComoboBox();
            cbCountry.SelectedIndex = 0;

            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "Add New Person";
                _Person = new clsPerson();
                return;
            }

            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("This form will be closed because No Contact with ID = " + _PersonID);
                this.Close();
                return;
            }

            lblMode.Text = "Edit Person ID = " + _PersonID;
            lblPersonID.Text = _PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;

            if (_Person.ThirdName == null)
                txtThirdName.Text = "";
            else txtThirdName.Text = _Person.ThirdName;

            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;
            rbMale.Checked = _Person.Gendor == 0;
            txtPhone.Text = _Person.Phone;

            if (_Person.Email == null)
                txtEmail.Text = "";
            else txtEmail.Text = _Person.Email;

            txtAddress.Text = _Person.Address;

            if (_Person.ImagePath != "")
            {
                pictureBox1.Load(_Person.ImagePath);
            }

            //this will select the country in the combobox.
            cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.Find(_Person.ID).CountryName);
        }

        public void DateOfBirthMaxDate()
        {
            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-18);
        }

        private void Gendor_CheckedChanged(object sender, EventArgs e) {

            if (rbMale.Checked) {
                pctPerson.Image = Resources.Male_512;
            } else if (rbFemale.Checked)
            {
                pctPerson.Image = Resources.Female_512;
            }
        }



        private void frmAddNewPerson_Load(object sender, EventArgs e)
        {
            DateOfBirthMaxDate();
            _LoadData();

        }

      

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFirstName, "First Name is required.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFirstName, null);
            }
        }

        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtSecondName.Text))
            {
                e.Cancel= true;
                errorProvider1.SetError(txtSecondName, "Second Name is required.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtSecondName, null);
            }
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLastName.Text)) {

                e.Cancel = true;
                errorProvider1.SetError(txtLastName, "Last Name is required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLastName, null);
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (clsPerson.isPersonExistByNationalNo(txtNationalNo.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person.");
            } else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtAddress.Text))
            {

                e.Cancel = true;
                errorProvider1.SetError(txtAddress, "Address is required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtAddress, null);
            }
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhone.Text))
            {

                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "Phone is required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPhone, null);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Length == 0)
            {
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, null);
            } 
            else if (txtEmail.Text.Contains("@"))
            {
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, null);
            } else
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format");
            }
        }
    }
}
