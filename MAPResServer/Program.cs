using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Collections;
using Thrift.Transport;
using Thrift.Server;

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
                TServerTransport sT = new TServerSocket(4040);
                TThreadedServer server = new TThreadedServer(process, sT);
                Console.WriteLine("Server Started");
                server.Serve();
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
            throw new NotImplementedException();
        }

        public string getDOEC(string ModSite)
        {
            throw new NotImplementedException();
        }

        public string getExpectedCount(string ModSite)
        {
            throw new NotImplementedException();
        }

        public string getExpectedFrequency(string ModSite)
        {
            throw new NotImplementedException();
        }

        public string getExpectedFrequencyAsPerc(string ModSite)
        {
            throw new NotImplementedException();
        }

        public string getObservedCount(string ModSite)
        {
            throw new NotImplementedException();
        }

        public string getObservedFrequency(string ModSite)
        {
            throw new NotImplementedException();
        }

        public string getObservedFrequencyAsPerc(string ModSite)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        public void setProjectProfileWithPseudoAminoAcids(string AnalysisTitle, string AnalystName, int PeptideWindowSize, THashSet<string> ModificationSites, THashSet<string> ListOfPseudoAminoAcids, string path)
        {
            throw new NotImplementedException();
        }

        public void setProjectProfileWithStandardAminoAcids(string AnalysisTitle, string AnalystName, int PeptideWindowSize, THashSet<string> ModificationSites, string path)
        {
            throw new NotImplementedException();
        }
    }
}
