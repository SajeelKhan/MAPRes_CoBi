using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cobi_Cognitive_Architecture
{
    public class Kinase
    {
        private String KinaseName;
        private Boolean Status;

        public Kinase()
        {

        }

        public Kinase(String KinaseName, Boolean Status)
        {
            this.KinaseName = KinaseName;
            this.Status = Status;
        }

        public void setKinaseName(String KinaseName)
        {
            this.KinaseName = KinaseName;
        }

        public String getKinaseName()
        {
            return this.KinaseName;
        }

        public void setStatus(Boolean Status)
        {
            this.Status = Status;
        }

        public Boolean getStatus()
        {
            return this.Status;
        }
    }
}
