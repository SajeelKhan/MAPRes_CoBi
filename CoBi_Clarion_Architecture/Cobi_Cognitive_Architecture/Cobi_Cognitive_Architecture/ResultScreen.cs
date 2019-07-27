using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cobi_Cognitive_Architecture
{
    public partial class ResultScreen : Form
    {
        public ResultScreen()
        {
            InitializeComponent();
        }

        private void ResultScreen_Load(object sender, EventArgs e)
        {
            DataTable ResultTable = new DataTable();

            ResultTable.Columns.Add("Peptide");
            ResultTable.Columns.Add("Kinase");
            ResultTable.Columns.Add("Status");

            

        }
    }
}
