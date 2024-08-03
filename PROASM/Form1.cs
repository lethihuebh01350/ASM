using System;
using System.Windows.Forms;

namespace PROASM
{
    public partial class Form1 : Form
    {
        private bool isCalculationDone = false;

        public Form1()
        {
            InitializeComponent();

            // Register event handlers once
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnEdit.Click += new EventHandler(btnEdit_Click);
            btnClear.Click += new EventHandler(btnClear_Click);

            cmbCustomerType.DropDown += new EventHandler(cmbCustomerType_DropDown);
            cmbCustomerType.SelectedIndexChanged += new EventHandler(cmbCustomerType_SelectedIndexChanged);
            lsvlclass.SelectedIndexChanged += new EventHandler(lsvlclass_SelectedIndexChanged);

            // Add event handlers for key press validation
            txtPhone.KeyPress += new KeyPressEventHandler(txtPhone_KeyPress);
            txtOldIndex.KeyPress += new KeyPressEventHandler(txtIndex_KeyPress);
            txtNewIndex.KeyPress += new KeyPressEventHandler(txtIndex_KeyPress);
            txtNumberOfPeople.KeyPress += new KeyPressEventHandler(txtNumberOfPeople_KeyPress);
        }

        private void InitializeComboBox()
        {
            cmbCustomerType.DropDown += new EventHandler(cmbCustomerType_DropDown);
        }

        private void cmbCustomerType_DropDown(object sender, EventArgs e)
        {
            cmbCustomerType.Items.Clear();
            cmbCustomerType.Items.Add("Family");
            cmbCustomerType.Items.Add("Administrative agency");
            cmbCustomerType.Items.Add("Production unit");
            cmbCustomerType.Items.Add("Business service");

            // Select "Family" by default
            cmbCustomerType.SelectedIndex = 0;
        }

        private void cmbCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomerType.SelectedItem != null && cmbCustomerType.SelectedItem.ToString() == "Family")
            {
                txtNumberOfPeople.Enabled = true;
                txtNumberOfPeople.Clear();  // Allow user to input the number of people
            }
            else
            {
                txtNumberOfPeople.Enabled = false;
                txtNumberOfPeople.Text = "1";  // Automatically set to 1 for non-family types
            }
        }

        private void Calculate()
        {
            try
            {
                // Ensure all required fields are filled out
                if (string.IsNullOrEmpty(txtOldIndex.Text) || string.IsNullOrEmpty(txtNewIndex.Text) ||
                    cmbCustomerType.SelectedItem == null)
                {
                    MessageBox.Show("Please fill out all required fields.");
                    return;
                }

                // Validate input values
                if (!double.TryParse(txtOldIndex.Text, out double oldIndex) ||
                    !double.TryParse(txtNewIndex.Text, out double newIndex))
                {
                    MessageBox.Show("Please enter valid values for indexes.");
                    return;
                }

                if (oldIndex > newIndex)
                {
                    MessageBox.Show("Old index cannot be greater than new index. Please re-enter.");
                    return;
                }

                string customerType = cmbCustomerType.SelectedItem.ToString();
                double waterUsage = newIndex - oldIndex;
                txtWaterUsage.Text = waterUsage.ToString();

                int numberOfPeople = 1; // Default to 1 for all types except "Family"
                if (customerType == "Family")
                {
                    if (string.IsNullOrEmpty(txtNumberOfPeople.Text) || !int.TryParse(txtNumberOfPeople.Text, out numberOfPeople))
                    {
                        MessageBox.Show("Please enter a valid number of people for Family type.");
                        return;
                    }
                }

                double price = 0;
                switch (customerType)
                {
                    case "Family":
                        if (waterUsage <= 10) price = 5.973 * waterUsage;
                        else if (waterUsage <= 20) price = 7.052 * waterUsage;
                        else if (waterUsage <= 30) price = 8.699 * waterUsage;
                        else price = 15.929 * waterUsage;
                        break;
                    case "Administrative agency":
                        price = 9.955 * waterUsage;
                        break;
                    case "Production unit":
                        price = 11.615 * waterUsage;
                        break;
                    case "Business service":
                        price = 22.068 * waterUsage;
                        break;
                    default:
                        MessageBox.Show("Invalid customer type.");
                        return;
                }

                double environmentFee = 0.10 * price;
                double vat = 0.10 * price;

                txtEnvironmentFee.Text = environmentFee.ToString("N2") + " VND";
                txtVAT.Text = vat.ToString("N2") + " VND";

                double totalAmount = price + environmentFee + vat;
                txtTotalAmount.Text = totalAmount.ToString("N2") + " VND";

                isCalculationDone = true; // Set calculation flag

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!isCalculationDone)
            {
                // Perform the calculation first
                Calculate();
            }
            else
            {
                // Add to list if calculation is already done
                try
                {
                    string name = txtName.Text;
                    string address = txtAddress.Text;
                    string phone = txtPhone.Text;
                    string customerType = cmbCustomerType.SelectedItem?.ToString(); // Use null-conditional operator
                    if (customerType == null)
                    {
                        MessageBox.Show("Please select a customer type.");
                        return;
                    }
                    double oldIndex = double.Parse(txtOldIndex.Text);
                    double newIndex = double.Parse(txtNewIndex.Text);
                    double waterUsage = double.Parse(txtWaterUsage.Text);
                    int numberOfPeople = customerType == "Family" ? int.Parse(txtNumberOfPeople.Text) : 1;
                    double environmentFee = double.Parse(txtEnvironmentFee.Text.Replace(" VND", ""));
                    double vat = double.Parse(txtVAT.Text.Replace(" VND", ""));
                    double totalAmount = double.Parse(txtTotalAmount.Text.Replace(" VND", ""));

                    ListViewItem item = new ListViewItem(name);
                    item.SubItems.Add(address);
                    item.SubItems.Add(phone);
                    item.SubItems.Add(customerType);
                    item.SubItems.Add(oldIndex.ToString());
                    item.SubItems.Add(newIndex.ToString());
                    item.SubItems.Add(waterUsage.ToString());
                    item.SubItems.Add(customerType == "Family" ? numberOfPeople.ToString() : string.Empty);
                    item.SubItems.Add(environmentFee.ToString("N2") + " VND");
                    item.SubItems.Add(vat.ToString("N2") + " VND");
                    item.SubItems.Add(totalAmount.ToString("N2") + " VND");

                    lsvlclass.Items.Add(item);

                    // Clear input fields after adding
                    ClearForm();
                    isCalculationDone = false; // Reset calculation flag 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (lsvlclass.SelectedItems.Count > 0)
                {
                    // Perform the calculation first
                    Calculate();

                    ListViewItem selectedItem = lsvlclass.SelectedItems[0];

                    selectedItem.SubItems[0].Text = txtName.Text;
                    selectedItem.SubItems[1].Text = txtAddress.Text;
                    selectedItem.SubItems[2].Text = txtPhone.Text;
                    selectedItem.SubItems[3].Text = cmbCustomerType.SelectedItem?.ToString();                  // Use null-conditional operator
                    selectedItem.SubItems[4].Text = txtOldIndex.Text;
                    selectedItem.SubItems[5].Text = txtNewIndex.Text;
                    selectedItem.SubItems[6].Text = txtWaterUsage.Text;
                    selectedItem.SubItems[7].Text = cmbCustomerType.SelectedItem?.ToString() == "Family" ? 
                        txtNumberOfPeople.Text : string.Empty;
                    selectedItem.SubItems[8].Text = txtEnvironmentFee.Text;
                    selectedItem.SubItems[9].Text = txtVAT.Text;
                    selectedItem.SubItems[10].Text = txtTotalAmount.Text;

                    // Clear input fields after editing
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Please select a row to edit.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lsvlclass.SelectedItems.Count > 0)
                {
                    lsvlclass.Items.Remove(lsvlclass.SelectedItems[0]);
                }
                else
                {
                    MessageBox.Show("Please select a row to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            ClearForm();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void lsvlclass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvlclass.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lsvlclass.SelectedItems[0];

                txtName.Text = selectedItem.SubItems[0].Text;
                txtAddress.Text = selectedItem.SubItems[1].Text;
                txtPhone.Text = selectedItem.SubItems[2].Text;
                cmbCustomerType.SelectedItem = selectedItem.SubItems[3].Text;
                txtOldIndex.Text = selectedItem.SubItems[4].Text;
                txtNewIndex.Text = selectedItem.SubItems[5].Text;
                txtWaterUsage.Text = selectedItem.SubItems[6].Text;
                txtNumberOfPeople.Text = selectedItem.SubItems[7].Text;
                txtEnvironmentFee.Text = selectedItem.SubItems[8].Text;
                txtVAT.Text = selectedItem.SubItems[9].Text;
                txtTotalAmount.Text = selectedItem.SubItems[10].Text;

                // Enable or disable the number of people field based on customer type
                txtNumberOfPeople.Enabled = cmbCustomerType.SelectedItem?.ToString() == "Family";
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits and control characters
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Please enter numbers only for Phone Number field.");
            }
        }

        private void txtIndex_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits, decimal point, and control characters
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Please enter numbers only for the Water Index field.");
            }
        }

        private void txtNumberOfPeople_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits and control characters
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Please enter numbers only for the Population field.");
            }
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtOldIndex.Clear();
            txtNewIndex.Clear();
            txtNumberOfPeople.Clear();
            txtWaterUsage.Clear();
            txtVAT.Clear();
            txtEnvironmentFee.Clear();
            txtTotalAmount.Clear();
            cmbCustomerType.SelectedIndex = -1;
            txtNumberOfPeople.Enabled = false;
            txtNumberOfPeople.Text = "1";              // Default to 1 and disabled
            isCalculationDone = false;               // Reset calculation flag
        }

        private void btnInbill_Click(object sender, EventArgs e)
        {
            if (lsvlclass.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lsvlclass.SelectedItems[0];
                Form2 form2 = new Form2(this);
                form2.LoadBillData(selectedItem);
                form2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Please select a row to generate the bill.");
            }
        }
        // Other existing methods...
    }
}
