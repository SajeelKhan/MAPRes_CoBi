using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Collections;
using Thrift.Transport;
using Thrift.Server;
using System.Xml;
using System.Data;
using System.IO;
using ExcelDataReader;

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
                ms.loginToServer("Sajeel", "1234");
                var set = new THashSet<String>();
                set.Add("A");
                set.Add("B");
                set.Add("C");
                set.Add("D");
                set.Add("E");
                ms.setProjectProfileWithStandardAminoAcids("FistAnalysis", "Sajeel",10 ,set, "thisPath");
                ms.openProjectLocation(@"F:\Results\Acetylation\Dataset File\Acetylation (Refined).xlsx");
                ms.runPreferenceEstimationProcess();
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
        private DataTable dtCombinedResultTable;

        private DataTable dtObserveredFrequencyTable;
        private DataTable dtObserveredCountTable;
        private DataTable dtPercentageObserveredFrequencyTable;
        private DataTable dtPercentageExpectedFrequencyTable;
        private DataTable dtExpectedFrequencyTable;
        private DataTable dtDOECTable;
        private DataTable dtDeveiationParameterTable;
        private DataTable dtSigmaTable;
        private DataTable dtPreferredSitesTable;
        private DataTable dtS_PreferredSitesTable;
        private DataTable dtExpectedCountTable;
        private DataTable dtCountPerAminoAcidTable;
        private DataTable dtAminoAcidsAndPreferredPositions;

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

        DataTable dtProteins;
        DataTable dtPeptides;
        public void openProjectLocation(string location)
        {
            try
            {
                using (var stream = File.Open(location, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            while (reader.Read())
                            {
                                // reader.GetDouble(0);
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        var result = reader.AsDataSet();

                        // The result of each spreadsheet is in result.Tables
                        dtProteins = result.Tables[0];
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void runAssociationRuleMiningProcess()
        {
            throw new NotImplementedException();
        }

        string Modsite;
        public void getModsite(string ModSite)
        {
            this.Modsite = ModSite;
        }

        public void runPreferenceEstimationProcess()
        {
            //implement

            DataRow rowCombined;
            DataRow rowObservedCount;
            DataRow rowObservedFrequency;
            DataRow rowPercentageObservedFrequency;
            DataRow rowDeviationParameter;
            DataRow rowDOEC;
            DataRow rowSigma;
            DataRow rowPreferredSites;
            DataRow rowS_PreferredSites;
            DataRow rowExpectedCount;
            DataRow rowExpectedFrequency;
            DataRow rowPercentageExpectedFrequency;
            DataRow rowCountPerAminoAcid;
            DataRow rowAminoAcidsAndPreferredPosition;
            string listOfAminoAcidsPostivePositions = "";
            string listOfAminoAcidsNegativePositions = "";
            int numberOfPositivePositions = 0;
            int numberOfNegativePositions = 0;
            int positionInPeptide;
            dtPeptides = dtProteins;

            using (PreferenceResultSet prs = new PreferenceResultSet(firstPosition,lastPosition))
            {
                prepareForAnalysis();

                int aminoIndex, position;
                for (aminoIndex = 0; aminoIndex < TotalAminoAcids; aminoIndex++)
                {
                    listOfAminoAcidsPostivePositions = "";
                    listOfAminoAcidsNegativePositions = "";
                    numberOfPositivePositions = 0;
                    numberOfNegativePositions = 0;

                    rowObservedCount = prs.ObserveredCount.NewRow();
                    rowObservedFrequency = prs.ObserveredFrequency.NewRow();
                    rowPercentageObservedFrequency = prs.PercentageObserveredFrequency.NewRow();
                    rowDeviationParameter = prs.DeviationParameter.NewRow();
                    rowDOEC = prs.DOEC.NewRow();
                    rowSigma = prs.Sigma.NewRow();
                    rowPreferredSites = prs.PreferredSites.NewRow();
                    rowS_PreferredSites = prs.S_PreferredSites.NewRow();
                    rowExpectedCount = prs.ExpectedCount.NewRow();
                    rowExpectedFrequency = prs.ExpectedFrequency.NewRow();
                    rowPercentageExpectedFrequency = prs.PercentageExpectedFrequency.NewRow();
                    rowCountPerAminoAcid = prs.CountPerAminoAcid.NewRow();
                    rowAminoAcidsAndPreferredPosition = prs.AminoAcidsAndPreferredPositions.NewRow();
                    
                    rowObservedCount["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    rowObservedFrequency["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    rowPercentageObservedFrequency["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    rowDeviationParameter["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    rowDOEC["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    rowSigma["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    rowPreferredSites["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    rowS_PreferredSites["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    rowExpectedCount["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    rowExpectedFrequency["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    rowPercentageExpectedFrequency["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    rowCountPerAminoAcid["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    rowAminoAcidsAndPreferredPosition["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                    
                    positionInPeptide = firstPosition;
                    for (position = 0; position < totalPositions; position++)
                    {
                        rowObservedCount["P" + positionInPeptide.ToString()] = _ObservedCount[aminoIndex, position];
                        rowObservedFrequency["P" + positionInPeptide.ToString()] = _ObservedFrequency[aminoIndex, position];
                        rowPercentageObservedFrequency["P" + positionInPeptide.ToString()] = _ObservedFrequency[aminoIndex, position] * 100;
                        rowDeviationParameter["P" + positionInPeptide.ToString()] = _DeviationParameter[aminoIndex, position];
                        rowDOEC["P" + positionInPeptide.ToString()] = _DOEC[aminoIndex, position];
                        rowSigma["P" + positionInPeptide.ToString()] = _Sigma[aminoIndex, position];
                        rowPreferredSites["P" + positionInPeptide.ToString()] = _PreferredSites[aminoIndex, position];
                        rowS_PreferredSites["P" + positionInPeptide.ToString()] = _SPreferredSites[aminoIndex, position];

                        rowCombined = prs.CombinedResult.NewRow();
                        rowCombined["ModificationSite"] = this.Modsite;
                        rowCombined["AminoAcid"] = AminoAcidsSet.ToList()[aminoIndex];
                        rowCombined["CountPerAminoAcid"] = _CountPerAminoAcid[aminoIndex];
                        rowCombined["ExpectedFrequency"] = _ExpectedFrequency[aminoIndex];
                        rowCombined["PercentageExpectedFrequency"] = _ExpectedFrequency[aminoIndex] * 100;
                        rowCombined["ExpectedCount"] = _ExpectedCount[aminoIndex];
                        rowCombined["Position"] = positionInPeptide;
                        rowCombined["S-Preferred"] = _SPreferredSites[aminoIndex, position];
                        rowCombined["Preferred"] = _PreferredSites[aminoIndex, position];
                        rowCombined["PercentageObservedFrequency"] = _ObservedFrequency[aminoIndex, position] * 100;
                        rowCombined["ObservedFrequency"] = _ObservedFrequency[aminoIndex, position];
                        rowCombined["ObservedCount"] = _ObservedCount[aminoIndex, position];
                        rowCombined["DeviationParameter"] = _DeviationParameter[aminoIndex, position];
                        rowCombined["DOEC"] = _DOEC[aminoIndex, position];
                        rowCombined["Sigma"] = _Sigma[aminoIndex, position];
                        prs.CombinedResult.Rows.Add(rowCombined);
                        
                        positionInPeptide++;
                    }

                    rowExpectedCount["Value"] = _ExpectedCount[aminoIndex];
                    rowExpectedFrequency["Value"] = _ExpectedFrequency[aminoIndex];
                    rowPercentageExpectedFrequency["Value"] = _ExpectedFrequency[aminoIndex] * 100;
                    rowCountPerAminoAcid["Value"] = _CountPerAminoAcid[aminoIndex];
                    

                    rowAminoAcidsAndPreferredPosition["PositivePositions"] = listOfAminoAcidsPostivePositions;
                    rowAminoAcidsAndPreferredPosition["NegativePositions"] = listOfAminoAcidsNegativePositions;
                    rowAminoAcidsAndPreferredPosition["NumberOfPositivePositions"] = numberOfPositivePositions;
                    rowAminoAcidsAndPreferredPosition["NumberOfNegativePositions"] = numberOfNegativePositions;
                    
                    prs.CountPerAminoAcid.Rows.Add(rowCountPerAminoAcid);
                    prs.DeviationParameter.Rows.Add(rowDeviationParameter);
                    prs.ExpectedCount.Rows.Add(rowExpectedCount);
                    prs.DOEC.Rows.Add(rowDOEC);
                    prs.ExpectedFrequency.Rows.Add(rowExpectedFrequency);
                    prs.ObserveredCount.Rows.Add(rowObservedCount);
                    prs.ObserveredFrequency.Rows.Add(rowObservedFrequency);
                    prs.PercentageExpectedFrequency.Rows.Add(rowPercentageExpectedFrequency);
                    prs.PercentageObserveredFrequency.Rows.Add(rowPercentageObservedFrequency);
                    prs.PreferredSites.Rows.Add(rowPreferredSites);
                    prs.S_PreferredSites.Rows.Add(rowS_PreferredSites);
                    prs.Sigma.Rows.Add(rowSigma);
                    prs.AminoAcidsAndPreferredPositions.Rows.Add(rowAminoAcidsAndPreferredPosition);
                    
                }
            }
        }

        public void SaveProject()
        {
            //implement
            string root = @"D:\CoBi";
            string subdir = @"D:\CoBi\";
            subdir = String.Concat(subdir, userName,"\\");

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            if (!Directory.Exists(subdir))
            {
                Directory.CreateDirectory(subdir);
            }
            subdir = String.Concat(subdir,userName ,".xml");

            using (XmlTextWriter xml = new XmlTextWriter(subdir, null))
                {
                xml.WriteStartDocument();
                xml.WriteStartElement("User Name");
                xml.WriteElementString("", "\n");
                xml.WriteString(userName);
                xml.WriteElementString("", "\n");

                xml.WriteStartElement("Analysis Details");
                xml.WriteElementString("Analysis Title", AnalysisTitle);
                xml.WriteElementString("","\n");
                xml.WriteElementString("Analyst Name", AnalystName);
                xml.WriteElementString("", "\n");

                foreach (String str in ModificationSites)
                {
                    xml.WriteElementString("Modification Sites", str);
                    xml.WriteElementString("", "\n");
                }

                foreach (String str in AminoAcidsSet)
                {
                    xml.WriteElementString("Amino Acid Set", str);
                    xml.WriteElementString("", "\n");
                }

                xml.WriteEndElement();
                xml.WriteEndDocument();
            }
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

        Dictionary<String, String> users = new Dictionary<string, string>();
        String userName;
        public bool loginToServer(string UserName, string passWord)
        {
            String pWord;
            addValues();
            if (users.ContainsKey(UserName))
            {
                pWord = users[UserName];
                if (pWord == passWord)
                {
                    userName = UserName;
                    return true;
                }
            }
      
            return false;
        }
        public void addValues()
        {
            users.Add("Sajeel", "1234");
            users.Add("Adnan", "12345");
        }

        int firstPosition;
        int lastPosition;
        int totalPositions;

        double[,] _ObservedFrequency;
        double[,] _ObservedCount;
        double[,] _DeviationParameter;
        double[,] _DOEC;
        double[,] _Sigma;
        double[,] _PreferredSites;
        double[,] _SPreferredSites;

        double[] _ExpectedCount;
        double[] _ExpectedFrequency;
        double[] _CountPerAminoAcid;

        int TotalAminoAcids;

        public void prepareForAnalysis()
        {
            firstPosition = -1 * PeptideWindowSize;
            lastPosition = PeptideWindowSize;
            totalPositions = PeptideWindowSize + PeptideWindowSize + 1;
            TotalAminoAcids = AminoAcidsSet.Count();

            this._CountPerAminoAcid = new double[this.TotalAminoAcids];
            this._ExpectedCount = new double[this.TotalAminoAcids];
            this._ExpectedFrequency = new double[this.TotalAminoAcids];

            this._ObservedCount = new double[this.TotalAminoAcids, this.totalPositions];
            this._ObservedFrequency = new double[this.TotalAminoAcids, this.totalPositions];
            this._DeviationParameter = new double[this.TotalAminoAcids, this.totalPositions];
            this._DOEC = new double[this.TotalAminoAcids, this.totalPositions];
            this._Sigma = new double[this.TotalAminoAcids, this.totalPositions];
            this._PreferredSites = new double[this.TotalAminoAcids, this.totalPositions];
            this._SPreferredSites = new double[this.TotalAminoAcids, this.totalPositions];

            ComputeObservedCount();
            ComputeObservedFrequencyandSigma();
            ComputeCountPerAminoAcid();
            ComputeExpectedFrequencyAndExpectedCount();
            Compute_DeviationParameter_EstimatePreferredSites_DOEC_EstimateSPreferredSites();
        }

        public void ComputeObservedCount()
        {
            string aminoacid;

            for(int i=0; i< dtPeptides.Rows.Count; i++)
            {
                for(int j=0; j< totalPositions;j++)
                {
                    aminoacid = dtPeptides.Rows[i][j + 3].ToString();
                    _ObservedCount[aminoacid.IndexOf(aminoacid), j]++;
                }
            }
        }

        public void ComputeObservedFrequencyandSigma()
        {
            for(int i=0; i< TotalAminoAcids; i++)
            {
                for(int j=0; j< totalPositions; j++)
                {
                    _ObservedFrequency[i, j] = _ObservedCount[i, j] / dtPeptides.Rows.Count;
                    _Sigma[i, j] = Math.Sqrt(_ObservedCount[i, j]);
                }
            }
        }

        int _TotalNumberOfAminoAcidsInProteinDataSet;

        public void ComputeCountPerAminoAcid()
        {
            string[] ExtendedSequence;
            _TotalNumberOfAminoAcidsInProteinDataSet = 0;
            char[] sep = { ',' };
            int colIndex;
            int totalCols;

            for (int rowIndex = 0; rowIndex < dtProteins.Rows.Count; rowIndex++)
            {
                ExtendedSequence = dtProteins.Rows[rowIndex]["IMSB_Sequence"].ToString().Split(sep);
                totalCols = ExtendedSequence.Length;
                _TotalNumberOfAminoAcidsInProteinDataSet = _TotalNumberOfAminoAcidsInProteinDataSet + totalCols; //ExtendedSequence.Length
                for (colIndex = 0; colIndex < totalCols; colIndex++)
                {
                    this._CountPerAminoAcid[AminoAcidsSet.ToList<string>().IndexOf(ExtendedSequence[colIndex])]++;
                }
            }
        }

        public void ComputeExpectedFrequencyAndExpectedCount()
        {
            for(int i=0; i<TotalAminoAcids;i++)
            {
                _ExpectedFrequency[i] = (_CountPerAminoAcid[i]) / _TotalNumberOfAminoAcidsInProteinDataSet;
                _ExpectedCount[i] = dtPeptides.Rows.Count * _ExpectedFrequency[i];
            }
        }

        public void Compute_DeviationParameter_EstimatePreferredSites_DOEC_EstimateSPreferredSites()
        {
            double expectedFrequency, expectedCount;
            double dp, doec, sigma;
            int mark;

            for(int i=0; i< TotalAminoAcids;i++)
            {
                expectedFrequency = _ExpectedFrequency[i];
                expectedCount = _ExpectedCount[i];
                for(int j=0; j< totalPositions;j++)
                {
                    dp = ((_ObservedFrequency[i, j] - expectedFrequency) / expectedFrequency) * 100;

                    if(double.IsNaN(dp) == true)
                    {
                        mark = 0;
                    }
                    else
                    {
                        mark = Math.Sign(dp);
                        _PreferredSites[i, j] = mark;
                    }

                    doec = _ObservedCount[i, j] - expectedCount;
                    _DOEC[i, j] = doec;

                    sigma = _Sigma[i, j];
                    if(doec >= (2 * sigma))
                    {
                        _SPreferredSites[i, j] = mark;
                    }
                }
            }
        }

        
        public MAPResServer() { }
    }

    class PreferenceResultSet : IDisposable
    {
        private DataTable dtCombinedResultTable;

        private DataTable dtObserveredFrequencyTable;
        private DataTable dtObserveredCountTable;
        private DataTable dtPercentageObserveredFrequencyTable;
        private DataTable dtPercentageExpectedFrequencyTable;
        private DataTable dtExpectedFrequencyTable;
        private DataTable dtDOECTable;
        private DataTable dtDeveiationParameterTable;
        private DataTable dtSigmaTable;
        private DataTable dtPreferredSitesTable;
        private DataTable dtS_PreferredSitesTable;
        private DataTable dtExpectedCountTable;
        private DataTable dtCountPerAminoAcidTable;
        private DataTable dtAminoAcidsAndPreferredPositions;

        
        //private long _totalNumberOfAminoAcidsInProteinDataSet;

        [NonSerialized]
        private bool isDisposed = false;
        [NonSerialized]
        private int _firstPosition;
        [NonSerialized]
        private int _lastPosition;

        public PreferenceResultSet(int firstPosition, int lastPosition)
        {
            _firstPosition = firstPosition;
            _lastPosition = lastPosition;

            dtCombinedResultTable = new DataTable("CombinedResults");
            dtCombinedResultTable.Columns.Add("ModificationSite");
            dtCombinedResultTable.Columns.Add("AminoAcid");
            dtCombinedResultTable.Columns.Add("CountPerAminoAcid");
            dtCombinedResultTable.Columns.Add("ExpectedFrequency");
            dtCombinedResultTable.Columns.Add("PercentageExpectedFrequency");
            dtCombinedResultTable.Columns.Add("ExpectedCount");
            dtCombinedResultTable.Columns.Add("Position");
            dtCombinedResultTable.Columns.Add("Site");
            dtCombinedResultTable.Columns.Add("S-Preferred");
            dtCombinedResultTable.Columns.Add("Preferred");
            dtCombinedResultTable.Columns.Add("PercentageObservedFrequency");
            dtCombinedResultTable.Columns.Add("ObservedFrequency");
            dtCombinedResultTable.Columns.Add("ObservedCount");
            dtCombinedResultTable.Columns.Add("DeviationParameter");
            dtCombinedResultTable.Columns.Add("DOEC");
            dtCombinedResultTable.Columns.Add("Sigma");

            dtObserveredCountTable = new DataTable("ObservedCount");
            dtObserveredFrequencyTable = new DataTable("ObservedFrequency");
            dtDeveiationParameterTable = new DataTable("DeviationParameter");
            dtDOECTable = new DataTable("DOEC");
            dtSigmaTable = new DataTable("Sigma");
            dtPreferredSitesTable = new DataTable("PreferredSites");
            dtS_PreferredSitesTable = new DataTable("SPreferredSites");
            dtPercentageObserveredFrequencyTable = new DataTable("PercentageObservedFrequency");
            dtPercentageExpectedFrequencyTable = new DataTable("PercentageExpectedFrequency");
            dtExpectedFrequencyTable = new DataTable("ExpectedFrequency");
            dtCountPerAminoAcidTable = new DataTable("CountPerAminoAcidInProteinDataSet");
            dtExpectedCountTable = new DataTable("ExpectedCount");

            dtObserveredCountTable.Columns.Add("AminoAcid");
            dtObserveredFrequencyTable.Columns.Add("AminoAcid");
            dtDeveiationParameterTable.Columns.Add("AminoAcid");
            dtDOECTable.Columns.Add("AminoAcid");
            dtSigmaTable.Columns.Add("AminoAcid");
            dtPreferredSitesTable.Columns.Add("AminoAcid");
            dtS_PreferredSitesTable.Columns.Add("AminoAcid");
            dtPercentageObserveredFrequencyTable.Columns.Add("AminoAcid");
            dtPercentageExpectedFrequencyTable.Columns.Add("AminoAcid");
            dtExpectedFrequencyTable.Columns.Add("AminoAcid");
            dtCountPerAminoAcidTable.Columns.Add("AminoAcid");
            dtExpectedCountTable.Columns.Add("AminoAcid");

            dtPercentageExpectedFrequencyTable.Columns.Add("Value");
            dtExpectedFrequencyTable.Columns.Add("Value");
            dtCountPerAminoAcidTable.Columns.Add("Value");
            dtExpectedCountTable.Columns.Add("Value");

            for (int position = _firstPosition; position <= _lastPosition; position++)
            {
                dtDeveiationParameterTable.Columns.Add("P" + position.ToString());
                dtDOECTable.Columns.Add("P" + position.ToString());
                dtObserveredCountTable.Columns.Add("P" + position.ToString());
                dtObserveredFrequencyTable.Columns.Add("P" + position.ToString());
                dtPercentageExpectedFrequencyTable.Columns.Add("P" + position.ToString());
                dtPercentageObserveredFrequencyTable.Columns.Add("P" + position.ToString());
                dtPreferredSitesTable.Columns.Add("P" + position.ToString());
                dtS_PreferredSitesTable.Columns.Add("P" + position.ToString());
                dtSigmaTable.Columns.Add("P" + position.ToString());
            }

            
            #region Memory Allocation Block for:-> dtAminoAcidsAndPreferredPositions DataTable
            dtAminoAcidsAndPreferredPositions = new DataTable("AminoAcidsAndNegativelyPreferredPositions");
            dtAminoAcidsAndPreferredPositions.Columns.Add("AminoAcid");
            dtAminoAcidsAndPreferredPositions.Columns.Add("NumberOfPositivePositions", System.Type.GetType("System.Int32"));
            dtAminoAcidsAndPreferredPositions.Columns.Add("PositivePositions");
            dtAminoAcidsAndPreferredPositions.Columns.Add("NumberOfNegativePositions", System.Type.GetType("System.Int32"));
            dtAminoAcidsAndPreferredPositions.Columns.Add("NegativePositions");
            #endregion
        }



        ~PreferenceResultSet()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (isDisposed == false)
            {
                if (disposing)
                {
                    dtCombinedResultTable.Dispose();
                    dtObserveredFrequencyTable.Dispose();
                    dtPercentageObserveredFrequencyTable.Dispose();
                    dtPercentageExpectedFrequencyTable.Dispose();
                    dtExpectedFrequencyTable.Dispose();
                    dtDOECTable.Dispose();
                    dtDeveiationParameterTable.Dispose();
                    dtSigmaTable.Dispose();
                    dtPreferredSitesTable.Dispose();
                    dtS_PreferredSitesTable.Dispose();
                    dtExpectedCountTable.Dispose();
                    dtExpectedFrequencyTable.Dispose();
                    dtCountPerAminoAcidTable.Dispose();
                    dtAminoAcidsAndPreferredPositions.Dispose();

                }
            }
        }

        public DataTable CombinedResult
        {
            get
            {
                return dtCombinedResultTable;
            }
        }

        public DataTable AminoAcidsAndPreferredPositions
        {
            get
            {
                return dtAminoAcidsAndPreferredPositions;
            }
        }


        public DataTable ObserveredFrequency
        {
            get
            {
                return dtObserveredFrequencyTable;
            }
        }

        public DataTable ObserveredCount
        {
            get
            {
                return dtObserveredCountTable;
            }
        }

        public DataTable PercentageObserveredFrequency
        {
            get
            {
                return dtPercentageObserveredFrequencyTable;
            }
        }

        public DataTable PercentageExpectedFrequency
        {
            get
            {
                return dtPercentageExpectedFrequencyTable;
            }
        }

        public DataTable ExpectedFrequency
        {
            get
            {
                return dtExpectedFrequencyTable;
            }
        }

        public DataTable DOEC
        {
            get
            {
                return dtDOECTable;
            }
        }

        public DataTable DeviationParameter
        {
            get
            {
                return dtDeveiationParameterTable;
            }
        }

        public DataTable Sigma
        {
            get
            {
                return dtSigmaTable;
            }
        }

        public DataTable PreferredSites
        {
            get
            {
                return dtPreferredSitesTable;
            }
        }

        public DataTable S_PreferredSites
        {
            get
            {
                return dtS_PreferredSitesTable;
            }
        }

        public DataTable ExpectedCount
        {
            get
            {
                return dtExpectedCountTable;
            }
        }

        public DataTable CountPerAminoAcid
        {
            get
            {
                return dtCountPerAminoAcidTable;
            }
        }

        
        
    }
}
