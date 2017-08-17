using System;
using System.Linq;
using System.Windows.Forms;


namespace Proaxis
{
    public partial class frmReport : Form
    {
        ProaxisDBEntities db;
        
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            db = new ProaxisDBEntities();
            var orderSet = from or in db.orders
                           where this.dateTimePicker1.Value >= or.orderTimestamp && or.orderTimestamp <= this.dateTimePicker2.Value
                           group or by or.badgeID into g
                           select new
                           {
                               badgeID = g.Key,
                               entriesSum = g.Sum(or=>or.orderTotal)
                           };


            var reportSet = from em in db.employees
                            join or in orderSet on em.badgeID equals or.badgeID
                            select new
                            {
                                lastName = em.lastName,
                                firstName = em.firstName,
                                employeeID = em.employeeID,
                                badgeID = em.badgeID,
                                sumExpense = or.entriesSum
                            };

            this.dataGridView1.DataSource = reportSet.ToList();
        }

    }
}
