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
    public partial class MainScreen : Form
    {
        public MainScreen()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            String ProteinSequence = ProteinSequenceTextBox.Text;
            
            Boolean is_Checked_Serine = SerineRadioBtn.Checked;
            Boolean is_Checked_Threonine = ThreonineRadioBtn.Checked;
            Boolean is_Checked_Tyrosine = TyrosineRadioBtn.Checked;
            Boolean is_Checked_All = AllRadioBtn.Checked;

            HeaderLabel.Text = "CoBi Prediction Results";
            ProgressBarPanel.Visible = true;

            AminoAcid[] StandardProteinSequence = MainClass.getStandardProteinSequence(ProteinSequence);
            ProgressBar.Increment(10);
            Percentage.Text = "10%";

            List<Peptide> Peptides = MainClass.getPeptideList(StandardProteinSequence, is_Checked_Serine, is_Checked_Threonine, is_Checked_Tyrosine, is_Checked_All);
            ProgressBar.Increment(20);
            Percentage.Text = "23%";

            Peptides = MainClass.Train_Clarion_Model_AGC_Group(Peptides);
            ProgressBar.Increment(40);
            Percentage.Text = "67%";

            Peptides = MainClass.Train_Clarion_Model_PKB_Family(Peptides);
            ProgressBar.Increment(70);
            Percentage.Text = "81%";

            Peptides = MainClass.Train_Clarion_Model_CaseinKinase_Family(Peptides);
            ProgressBar.Increment(100);
            Percentage.Text = "100%";

            DataTable ResultTable = new DataTable();

            ResultTable.Columns.Add("Peptides");
            ResultTable.Columns.Add("Kinase Families");
            ResultTable.Columns.Add("Status");

            for (int PeptideCount = 0; PeptideCount < Peptides.Count; PeptideCount++)
            {
                for (int KinaseCount = 0; KinaseCount < Peptides.ElementAt(PeptideCount).getModifKinases().Count; KinaseCount++)
                {
                    if (Peptides.ElementAt(PeptideCount).getModifKinases().ElementAt(KinaseCount).getStatus() == true)
                    {
                        ResultTable.Rows.Add(Peptides.ElementAt(PeptideCount).toString(), Peptides.ElementAt(PeptideCount).getModifKinases().ElementAt(KinaseCount).getKinaseName(), "YES");
                    }
                    else
                    {
                        ResultTable.Rows.Add(Peptides.ElementAt(PeptideCount).toString(), Peptides.ElementAt(PeptideCount).getModifKinases().ElementAt(KinaseCount).getKinaseName(), "---");
                    }

                    ResultGrid.DataSource = ResultTable;
                }

            }

            
        }

        
    }
}
