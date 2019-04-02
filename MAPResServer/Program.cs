using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Collections;
using Thrift.Transport;
using Thrift.Server;
using System.Collections;

namespace MAPResServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MAPResServer ms = new MAPResServer();
                MAPResService.Processor process = new MAPResService.Processor(ms);
                TServerTransport sT = new TServerSocket(9091);
                TThreadedServer server = new TThreadedServer(process, sT);
                Console.WriteLine("Server Started");
                server.Serve();
                ms.getStandardAminoAcidSet();
                ms.counter();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }

    class MAPResServer : MAPResService.Iface
    {
        public List<string> getAminoAcidsandPreferredPositions()
        {
            throw new NotImplementedException();
        }

        public List<string> getAssociationRules()
        {
            throw new NotImplementedException();
        }

        public string getDeviationParameter(string ModSite)
        {
            return ModSite + "From Server";
        }

        public string getDOEC(string ModSite)
        {
            return ModSite + "From Server";
        }

        public string getExpectedCount(string ModSite)
        {
            return ModSite + "From Server";
        }

        public string getExpectedFrequency(string ModSite)
        {
            return ModSite + "From Server";
        }

        public string getExpectedFrequencyAsPerc(string ModSite)
        {
            return ModSite + "From Server";
        }

        public string getObservedCount(string ModSite)
        {
            return ModSite + "From Server";
        }

        public string getObservedFrequency(string ModSite)
        {
            return ModSite + "From Server";
        }

        public string getObservedFrequencyAsPerc(string ModSite)
        {
            return ModSite + "From Server";
        }

        public List<string> getPeptidesDataset()
        {
            throw new NotImplementedException();
        }

        public List<string> getPreferredSitesMatrix()
        {
            throw new NotImplementedException();
        }

        public List<string> getPrimaryDataset()
        {
            throw new NotImplementedException();
        }

        public List<string> getProteinsDataset()
        {
            throw new NotImplementedException();
        }

        public string getSigma(string ModSite)
        {
            return ModSite + "From Server";
        }

        public List<string> getSignificantlyPreferredSitesMatrix()
        {
            throw new NotImplementedException();
        }

        public List<string> getSignificantlyPreferredSites_Positive_Negative_Both()
        {
            throw new NotImplementedException();
        }

        public List<string> getSitesDataset()
        {
            throw new NotImplementedException();
        }

        public void openProjectLocation(string location)
        {
            throw new NotImplementedException();
        }

        public void runAssociationRuleMiningProcess()
        {
            throw new NotImplementedException();
        }

        public void runPreferenceEstimationProcess()
        {
            throw new NotImplementedException();
        }

        public void SaveProject()
        {
            throw new NotImplementedException();
        }

        string AnalysisTitle;
        string AnalystName;
        int PeptideWindowSize;
        THashSet<string> ModificationSites;
        THashSet<string> AminoAcidsSet = new THashSet<string>();
        string path;
        public void setProjectProfileWithPseudoAminoAcids(string AnalysisTitle, string AnalystName, int PeptideWindowSize, THashSet<string> ModificationSites, THashSet<string> ListOfPseudoAminoAcids, string path)
        {
            this.AnalysisTitle = AnalysisTitle;
            this.AnalystName = AnalystName;
            this.PeptideWindowSize = PeptideWindowSize;
            this.ModificationSites = ModificationSites;
            AminoAcidsSet = ListOfPseudoAminoAcids;
            this.path = path;
        }

        public void setProjectProfileWithStandardAminoAcids(string AnalysisTitle, string AnalystName, int PeptideWindowSize, THashSet<string> ModificationSites, string path)
        {
            this.AnalysisTitle = AnalysisTitle;
            this.AnalystName = AnalystName;
            this.PeptideWindowSize = PeptideWindowSize;
            this.ModificationSites = ModificationSites;
            this.path = path;
            getStandardAminoAcidSet();
        }
        public void getStandardAminoAcidSet()
        {
            AminoAcidsSet.Add("A");
            AminoAcidsSet.Add("C");
            AminoAcidsSet.Add("D");
            AminoAcidsSet.Add("E");
            AminoAcidsSet.Add("F");
            AminoAcidsSet.Add("G");
            AminoAcidsSet.Add("H");
            AminoAcidsSet.Add("I");
            AminoAcidsSet.Add("K");
            AminoAcidsSet.Add("L");
            AminoAcidsSet.Add("M");
            AminoAcidsSet.Add("N");
            AminoAcidsSet.Add("P");
            AminoAcidsSet.Add("Q");
            AminoAcidsSet.Add("R");
            AminoAcidsSet.Add("S");
            AminoAcidsSet.Add("T");
            AminoAcidsSet.Add("V");
            AminoAcidsSet.Add("W");
            AminoAcidsSet.Add("Y");
            AminoAcidsSet.Add("-");
        }
        
    }
}
