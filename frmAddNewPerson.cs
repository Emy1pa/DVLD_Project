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
       

        private void _LoadData()
        {
            _FillCountriesInComoboBox();
            cbCountry.SelectedIndex = 0;

            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "Add New Person";
                _Person = new clsPerson();
                lnkRemoveImg.Visible = (!string.IsNullOrEmpty(_Person.ImagePath));
                return;
            }

            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("This form will be closed because No Person with ID = " + _PersonID);
                this.Close();
                return;
            }

            lblMode.Text = "Edit Person ID = " + _PersonID;

            lblPersonID.Text = _PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtNationalNo.Text = _Person.NationalNo;
            txtNationalNo.Enabled = _Mode == enMode.AddNew;

            if (_Person.ThirdName == null)
                txtThirdName.Text = "";
            else txtThirdName.Text = _Person.ThirdName;

            txtLastName.Text = _Person.LastName;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            rbMale.Checked = _Person.Gendor == 0;
            rbFemale.Checked = _Person.Gendor == 1;
            txtPhone.Text = _Person.Phone;

            if (_Person.Email == null)
                txtEmail.Text = "";
            else txtEmail.Text = _Person.Email;

            txtAddress.Text = _Person.Address;

            if (!string.IsNullOrEmpty(_Person.ImagePath))
            {
                pctPerson.Load(_Person.ImagePath);
            }

            lnkRemoveImg.Visible = (!string.IsNullOrEmpty(_Person.ImagePath));
            //lnkRemoveImg.Visible = (_Person.ImagePath != "");


            //this will select the country in the combobox.
            cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.Find(_Person.NationalityCountryID).CountryName);
        }

        public void DateOfBirthMaxDate()
        {
            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-18);
        }

        private void Gendor_CheckedChanged(object sender, EventArgs e) {

            if (rbMale.Checked && pctPerson.ImageLocation == null) {
                pctPerson.Image = Resources.Male_512;
            } else if (rbFemale.Checked && pctPerson.ImageLocation == null)
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
            }
            else if (string.IsNullOrEmpty(txtNationalNo.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is required.");

            }
            else
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
            if (!this.ValidateChildren())
            {
                MessageBox.Show(
                    "Some fields are not valid. Hover over the red icons to see the error.", // message
                    "Validation Error",          
                    MessageBoxButtons.OK,       
                    MessageBoxIcon.Error        
                ); return;
            }

            int CountryID = clsCountry.Find(cbCountry.Text).ID;

            _Person.FirstName = txtFirstName.Text;
            _Person.SecondName = txtSecondName.Text;
            _Person.ThirdName = txtThirdName.Text;
            _Person.LastName = txtLastName.Text;
            _Person.Phone = txtPhone.Text;
            _Person.NationalNo = txtNationalNo.Text;
            _Person.NationalityCountryID = CountryID;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Email = txtEmail.Text;
            _Person.Address = txtAddress.Text;
            _Person.Gendor = (byte)(rbMale.Checked ? 0 : 1);

            if (pctPerson.ImageLocation != null)
                _Person.ImagePath = pctPerson.ImageLocation;
            else
                _Person.ImagePath = "";

            if (_Person.Save())
            {
                MessageBox.Show("Data Saved Successfully.");
                _Mode = enMode.Update;
                lblMode.Text = "Edit Person ID = " + _Person.ID;
                lblPersonID.Text = _Person.ID.ToString();
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.");


           

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

        private void btnAddPersonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lnkSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                //MessageBox.Show("Selected Image is:" + selectedFilePath);

                pctPerson.Load(selectedFilePath);
                lnkRemoveImg.Visible = true;
                // ...
            }
        }

        private void lnkRemoveImg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pctPerson.ImageLocation = null;
            lnkRemoveImg.Visible = false;
            pctPerson.Image = Resources.Male_512;
        }
    }
}
