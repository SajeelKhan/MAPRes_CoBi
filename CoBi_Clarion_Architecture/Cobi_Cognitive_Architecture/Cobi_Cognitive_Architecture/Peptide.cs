using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cobi_Cognitive_Architecture
{
    public class Peptide
    {
        private AminoAcid[] PeptAminoAcids = new AminoAcid[9];
        private List<Kinase> ModifKinases = new List<Kinase>();

        public Peptide()
        {
            this.setModifKinases();
        }

        public Peptide(AminoAcid[] PeptAminoAcids)
        {
            this.PeptAminoAcids = PeptAminoAcids;
            this.setModifKinases();
        }

        public void setPeptAminoAcids(AminoAcid[] PeptAminoAcids)
        {
            this.PeptAminoAcids = PeptAminoAcids;
        }

        public AminoAcid[] getPeptAminoAcids()
        {
            return this.PeptAminoAcids;
        }

        private void setModifKinases()
        {
            this.ModifKinases.Add(new Kinase("PKA", false));
            this.ModifKinases.Add(new Kinase("PKG", false));
            this.ModifKinases.Add(new Kinase("PKC", false));
            this.ModifKinases.Add(new Kinase("PKB", false));
            this.ModifKinases.Add(new Kinase("AKT1", false));
            this.ModifKinases.Add(new Kinase("AKT2", false));
            this.ModifKinases.Add(new Kinase("CK1", false));
            this.ModifKinases.Add(new Kinase("CK2", false));
        }

        public List<Kinase> getModifKinases()
        {
            return this.ModifKinases;
        }

        public void setKinaseStatus(String KinaseName)
        {
            for (int KinaseCount = 0; KinaseCount < this.ModifKinases.Count; KinaseCount++)
            {
                if (this.ModifKinases.ElementAt(KinaseCount).getKinaseName() == KinaseName)
                {
                    this.ModifKinases.ElementAt(KinaseCount).setStatus(true);
                }
            }
        }

        public String toString()
        {
            String PeptideSequence = null;

            for (int i = 0; i < this.PeptAminoAcids.Length; i++)
            {
                PeptideSequence = PeptideSequence + "" + this.PeptAminoAcids[i].getSymbol();
            }

            return PeptideSequence;
        }
    }
}
