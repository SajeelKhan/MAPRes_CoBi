using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cobi_Cognitive_Architecture
{
    public class Protein
    {
        private String ProteinID;
        private String ProteinSequence;
        private int SequenceLength;
        private List<ModifiedResidue> ModifiedResidues = new List<ModifiedResidue>();

        public Protein(String ProteinID, String Sequence, int Length)
        {
            this.ProteinID = ProteinID;
            this.ProteinSequence = Sequence;
            this.SequenceLength = Length;
        }

        public void setProteinID(String ProteinID)
        {
            this.ProteinID = ProteinID;
        }

        public String getProteinID()
        {
            return this.ProteinID;
        }

        public void setProteinSequence(String ProteinSequence)
        {
            this.ProteinSequence = ProteinSequence;
        }

        public String getProteinSequence()
        {
            return this.ProteinSequence;
        }

        public void setSequenceLength(int SequenceLength)
        {
            this.SequenceLength = SequenceLength;
        }

        public int getSequenceLength()
        {
            return this.SequenceLength;
        }

        public void setModifiedResidues(List<ModifiedResidue> ModifiedResidues)
        {
            this.ModifiedResidues = ModifiedResidues;
        }

        public List<ModifiedResidue> getModifiedResidues()
        {
            return this.ModifiedResidues;
        }

        public void addModifiedResidue(ModifiedResidue ModifResidue)
        {
            this.ModifiedResidues.Add(ModifResidue);
        }
    }
}
