using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Proaxis
{
    public partial class frmMain : Form
    {
        private decimal orderTotal = 0;

        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            orderTotal = 0;
        }

        private void addToOrder(decimal price) {
            orderTotal += price;
        }

        private void updateCurrentOrderTextBox() {
            this.textBox1.Text=string.Format("{0:#,##0.00}", orderTotal);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = DateTime.Now.ToString();
            updateCurrentOrderTextBox();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.timer1.Interval = 100;
            this.timer1.Start();
            this.pnlControls.Width = this.Width/3;
            this.pnlItems.Width = this.Width - pnlControls.Width - 8;
            this.pnlEntry.Visible=false;
            using (var db = new ProaxisDBEntities())
            {
                foreach(item i in db.items)
                {
                    var button = new Button();
                    button.Text = i.itemDescription;
                    button.Click += itembtn_Click;
                    button.Parent = this.pnlItems;
                    button.Height = 120;
                    button.Width = 120;
                }
            }
            this.pnlItems.Refresh();
        }

        private void itembtn_Click(object sender, EventArgs e)
        {
            using (var db = new ProaxisDBEntities())
            {
                Button clicked = (Button)sender;
                var i = db.items.Where(c => c.itemDescription == clicked.Text);
                foreach(item it in i)
                {
                    orderTotal += it.itemPrice;
                }
            }
            this.txtBadgeReader.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pnlEntry.Visible)
            {
                this.pnlEntry.Visible = false;
                this.txtPriceEntry.Text = "";
                this.button2.Text = "Enter Custom Amount";
            } else
            {
                this.button2.Text = "Cancel";
                this.pnlEntry.Visible = true;
                this.txtBadgeReader.Focus();
            }
        }

        private void txtBadgeReader_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                using (var db = new ProaxisDBEntities())
                {
                    if (db.employees.Find(txtBadgeReader.Text) != null)
                    {
                        orderTotal = Math.Round(orderTotal * (decimal).75, 2);
                        order curOrder = db.orders.Create();
                        curOrder.orderID = db.orders.Count();
                        curOrder.badgeID = this.txtBadgeReader.Text.Trim();
                        curOrder.orderTotal = orderTotal;
                        curOrder.orderTimestamp = DateTime.Now;
                        curOrder.employee = db.employees.Find(curOrder.badgeID);
                        this.richTextBox1.AppendText("Order:\t" + curOrder.orderID +
                            "\t Name: " + curOrder.employee.lastName + ", " + curOrder.employee.firstName +
                            "\nTotal: " + string.Format("{0:#,##0.00}", curOrder.orderTotal.ToString()) + "\n\n");
                        this.richTextBox1.ScrollToCaret();
                        db.orders.Add(curOrder);
                        db.SaveChanges();
                        orderTotal = 0;
                        this.txtBadgeReader.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("INVALID BADGE #");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtPriceEntry.Text += 1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtPriceEntry.Text += 2;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtPriceEntry.Text += 3;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtPriceEntry.Text += 4;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txtPriceEntry.Text += 5;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txtPriceEntry.Text += 6;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txtPriceEntry.Text += 7;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            txtPriceEntry.Text += 8;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            txtPriceEntry.Text += 9;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            txtPriceEntry.Text += 0;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            txtPriceEntry.Text += ".";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                orderTotal += decimal.Parse(this.txtPriceEntry.Text);
            }
            catch (Exception except)
            {
                MessageBox.Show(
                    "Please report this error to your IT department:\n" +
                    "Error Parsing Custom Price Entry\n" +
                    except.Message
                    );
            }
            this.txtPriceEntry.Text = "";
            this.pnlEntry.Visible = false;
            this.button2.Text = "Enter Custom Amount";
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.pnlControls.Width = this.Width / 3;
            this.pnlItems.Width = this.Width - pnlControls.Width - 8;
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReport reporter = new frmReport();
            reporter.Show();
        }
    }
}
