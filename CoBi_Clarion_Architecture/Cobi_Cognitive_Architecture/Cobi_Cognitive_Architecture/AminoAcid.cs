using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cobi_Cognitive_Architecture
{
    public class AminoAcid
    {
        private String AminoAcidName;
        private char Symbol;

        public AminoAcid(String AminoAcidName, char Symbol)
        {
            this.AminoAcidName = AminoAcidName;
            this.Symbol = Symbol;
        }

        public void setAminoAcidName(String AminoAcidName)
        {
            this.AminoAcidName = AminoAcidName;
        }

        public String getAminoAcidName()
        {
            return this.AminoAcidName;
        }

        public void setSymbol(char Symbol)
        {
            this.Symbol = Symbol;
        }

        public char getSymbol()
        {
            return this.Symbol;
        }
    }
}
