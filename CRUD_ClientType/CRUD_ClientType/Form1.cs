using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_ClientType
{
    public partial class Form1 : Form
    {
        INTEC_AGOST_OCT22Entities db = new INTEC_AGOST_OCT22Entities();
        List<string> Msg = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Metodos
            GetClientType();
            GetRestrictions();
            GetPermissions();
            
        }

        private void GetClientType() 
        {
            var clientType = db.ClientTypes.ToList();
            dgvPeople.DataSource = clientType;
        }

        private void GetPermissions()
        {
            var permissions = db.Permissions.ToList();
            foreach (var item in permissions)
            {
                cblPermissions.Items.Add(item.Name);
            }
        }

        private void GetRestrictions()
        {
            var restrictions = db.Restrictions.ToList();
            foreach (var item in restrictions)
            {
                cblRestrictions.Items.Add(item.Name);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            btnSave.Enabled = true;
            btnSave.ForeColor = Color.Green;
            btnCancel.Enabled = true;

        }

        private void SaveForm()
        {
            var clientType = new ClientType();
            clientType.Name = txtName.Text;
            clientType.Description = txtDescription.Text;
            clientType.CreatedDate = DateTime.Now;
            clientType.Enabled = true;

            db.ClientTypes.Add(clientType);
            var ClientSaved = db.SaveChanges() > 0;

            if(ClientSaved)
            {
                var user = new User();
                user.Id = Guid.NewGuid().ToString();
                user.Username = txtUserName.Text;
                user.Password = txtPass.Text;
                user.LicenseTypeId = Convert.ToInt32(cbLicenseType.SelectedValue);
                user.Enabled = true;
                user.CreatedDate = DateTime.Now;

                db.Users.Add(user);
                var userSaved = db.SaveChanges() > 0;

                if (userSaved)
                {
                    MessageBox.Show("The people has been added");

                    GetClientType();
                    DefaultControl();

                    btnAdd.Enabled = true;
                    btnSave.Enabled = false;
                    btnSave.ForeColor = Color.Black;
                    btnCancel.Enabled = false;

                }

            }
        }

        private void DefaultControl()
        {
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtPass.Text = string.Empty;
            txtUserName.Text = string.Empty;
            cbLicenseType.SelectedIndex = 0;
            
        }

        private bool ValidateForm()
        {
            Msg = new List<string>();
            bool result = true;

            if(string.IsNullOrEmpty(txtName.Text))
            {
                Msg.Add("The field Name) is required.");
                result = false;
            }

            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                Msg.Add("The field Description) is required.");
                result = false;
            }

            return result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                SaveForm();
            }
            else
            {
                string errors = string.Empty;
                int errorIndex = 1;
                foreach (var item in Msg)
                {
                    errors += $"{item.ToString()}\n";
                    errorIndex++;
                }
                MessageBox.Show(errors, "ERRORS", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

    }
}
