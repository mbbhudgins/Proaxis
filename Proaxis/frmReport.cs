using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Proaxis
{
    public partial class frmReport : Form
    {
        public frmReport()
        {
            InitializeComponent();
        }

        private void updateGrid()
        {

            using (var db = new ProaxisDBEntities())
            {
                var orderSet = from or in db.orders
                    where dateTimePicker1.Value >= or.orderTimestamp &&
                          or.orderTimestamp <= dateTimePicker2.Value
                    group or by or.badgeID
                    into g
                    select new
                    {
                        badgeID = g.Key,
                        entriesSum = g.Sum(or => or.orderTotal)
                    };


                var reportSet = from em in db.employees
                    join or in orderSet on em.badgeID equals or.badgeID
                    orderby em.lastName, em.firstName
                    select new
                    {
                        em.lastName,
                        em.firstName,
                        em.employeeID,
                        em.badgeID,
                        sumExpense = or.entriesSum
                    };
                dataGridView1.DataSource = reportSet.ToList();
            }
            dataGridView1.Refresh();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            updateGrid();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            updateGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileDialog dlgFileDialog = new SaveFileDialog();
            dlgFileDialog.AddExtension = true;
            dlgFileDialog.Filter = "Comma Seperated Values|*.csv";
            if (dlgFileDialog.ShowDialog() == DialogResult.OK)
            {
                var filename = dlgFileDialog.FileName;
                FileStream file = new FileStream(filename,FileMode.Create);
                StreamWriter strWriter = new StreamWriter(file);
                using (var db = new ProaxisDBEntities())
                {
                    var orderSet = from or in db.orders
                        where dateTimePicker1.Value >= or.orderTimestamp &&
                              or.orderTimestamp <= dateTimePicker2.Value
                        group or by or.badgeID
                        into g
                        select new
                        {
                            badgeID = g.Key,
                            entriesSum = g.Sum(or => or.orderTotal)
                        };


                    var reportSet = from em in db.employees
                        join or in orderSet on em.badgeID equals or.badgeID
                        orderby em.lastName,em.firstName
                        select new
                        {
                            em.lastName,
                            em.firstName,
                            em.employeeID,
                            em.badgeID,
                            sumExpense = or.entriesSum
                        };
                    foreach (var record in reportSet.ToList())
                    {
                        strWriter.WriteLine("{0},{1},{2},{3},{4:C}",record.lastName,record.firstName,record.employeeID,record.badgeID,record.sumExpense);
                    }
                }
                strWriter.Flush();
                strWriter.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}