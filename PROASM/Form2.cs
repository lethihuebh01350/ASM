using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace PROASM
{
    public partial class Form2 : Form
    {
        private Form1 parentForm;
        public Form2(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        public void LoadBillData(ListViewItem selectedItem)
        {
            txtName.Text = selectedItem.SubItems[0].Text;
            txtAddress.Text = selectedItem.SubItems[1].Text;
            txtPhone.Text = selectedItem.SubItems[2].Text;
            txtOldIndex.Text = selectedItem.SubItems[4].Text;
            txtNewIndex.Text = selectedItem.SubItems[5].Text;
            txtCustomerType.Text = selectedItem.SubItems[3].Text;
            txtNumberOfPeople.Text = selectedItem.SubItems[7].Text;
            txtVAT.Text = selectedItem.SubItems[9].Text;
            txtEnvironmentFee.Text = selectedItem.SubItems[8].Text;
            txtWaterUsage.Text = selectedItem.SubItems[6].Text;
            txtTotalAmount.Text = selectedItem.SubItems[10].Text;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
            parentForm.Show();
        }
    }
}