using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cobi_Cognitive_Architecture
{
    public class ModifiedResidue
    {
        private int ModificationSite;
        private String ModifResidueName;
        private AminoAcid ModifAminoAcid;
        private String KineaseName;
        private Peptide ModificationPeptide;

        public ModifiedResidue(int ModificationSite, String ModifResidueName, AminoAcid ModifAminoAcid, String KineaseName, Peptide ModificationPeptide)
        {
            this.ModificationSite = ModificationSite;
            this.ModifResidueName = ModifResidueName;
            this.ModifAminoAcid = ModifAminoAcid;
            this.KineaseName = KineaseName;
            this.ModificationPeptide = ModificationPeptide;
        }

        public void setModifResidueName(String ModifResidueName)
        {
            this.ModifResidueName = ModifResidueName;
        }

        public String getModifResidueName()
        {
            return this.ModifResidueName;
        }

        public void setModifAminoAcid(AminoAcid ModifAminoAcid)
        {
            this.ModifAminoAcid = ModifAminoAcid;
        }

        public AminoAcid getModifAminoAcid()
        {
            return this.ModifAminoAcid;
        }

        public void setModificationSite(int ModificationSite)
        {
            this.ModificationSite = ModificationSite;
        }

        public int getModificationSite()
        {
            return this.ModificationSite;
        }

        public void setKineaseName(String KineaseName)
        {
            this.KineaseName = KineaseName;
        }

        public String getKineaseName()
        {
            return this.KineaseName;
        }

        public void setModificationPeptide(Peptide ModificationPeptide)
        {
            this.ModificationPeptide = ModificationPeptide;
        }

        public Peptide getModificationPeptide()
        {
            return this.ModificationPeptide;
        }
    }
}
