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
using System.Data.OleDb;

namespace MAPResServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
                MAPResServer ms =  new MAPResServer();
                MAPResService.Processor process = new MAPResService.Processor(ms);
                TServerTransport sT = new TServerSocket(9091);
                TThreadedServer server = new TThreadedServer(process, sT);
                Console.WriteLine("Server Started");
                //ms.loginToServer("Sajeel", "1234");
                //var set = new THashSet<String>();
                //set.Add("S");
                //set.Add("T");
                //set.Add("Y");
                //ms.setProjectProfileWithStandardAminoAcids("FistAnalysis", "Sajeel",10 ,set, "thisPath");
                //ms.openProjectLocation(@"F:\Results\Carboxylation\XML Files\CarboxylEncoded.XML");
              //  ms.getModsite("S");
            
            //ms.runPreferenceEstimationProcess();
            //ms.SaveProject();
            //ms.getDeviationParameter("S");
                server.Serve();
                
            //}
            //catch (Exception e)
            //{
              //  Console.WriteLine(e.StackTrace);
            //}
        }
    }

    class MAPResServer : MAPResService.Iface
    {
        private DataTable dtCombinedResultTable;



        DataTable dtProteins;
        DataTable dtPeptides;

        PreferenceResultSet prs;

        private Dictionary<string, Subject> _subjectsHash;

        public Dictionary<string, Subject> SubjectsHash
        {
            set
            {
                _subjectsHash = value;
            }
            get
            {
                return this._subjectsHash;
            }
        }

        public DataTable GetProteinsDataTable()
        {

            return SubjectsHash[Modsite].ProteinDataTable;
        }

        public DataTable GetPeptidesDataTable()
        {

            return SubjectsHash[Modsite].PeptideDataTable;
        }

        

        public List<string> getAssociationRules()
        {
            throw new NotImplementedException();
        }

        public string getDeviationParameter(string ModSite)
        {
            foreach (DataRow row in prs.DeviationParameter.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    return item.ToString();
                }
            }
            return "Nothing";
        }

        public string getDOEC(string ModSite)
        {

            foreach (DataRow row in prs.DOEC.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    return item.ToString();
                }
            }
            return "Nothing";
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

        DataTable usd;
        public void openProjectLocation(string location)
        {
            var result = new DataSet();
            result.ReadXml(location);
            usd = result.Tables[0].Copy();
            prepareData();
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

        

        public void SaveProject()
        {
            #region Write to XML File
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

            using (XmlTextWriter xml = new XmlTextWriter(subdir, Encoding.UTF8))
                {
                xml.WriteStartDocument();
                xml.WriteStartElement("User Name");
                xml.WriteString(userName);

                xml.WriteStartElement("Analysis Details");
                xml.WriteElementString("Analysis Title", AnalysisTitle);
                xml.WriteElementString("Analyst Name", AnalystName);
                
                foreach (String str in ModificationSites)
                {
                    xml.WriteElementString("Modification Sites", str);
                }

                foreach (String str in AminoAcidsSet)
                {
                    xml.WriteElementString("Amino Acid Set", str);
                }

                xml.WriteEndElement();
                xml.WriteEndDocument();
            }
            #endregion
            
            prs.CombinedResult.WriteXml(@"D:\CoBi\Sajeel\Results.xml");
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
            #region Standard Set Defination
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
            #endregion
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
        
        //Step 1: Generate Relevant Datasets
        private List<Subject> _lstSubjects;
        public void prepareData()
        {
            _lstSubjects = new List<Subject>();
            for(int i=0; i < ModificationSites.Count; i++)
            {
                _lstSubjects.Add(new Subject(ModificationSites.ElementAt(i)));
            }
            ApplyTransformation();
        }

        bool _isUSDNative;
        //Step 2: Apply Transformation Rules
        public void ApplyTransformation()
        {
            _isUSDNative = usd.Columns.Contains("IMSB_Sequence"); //This means that if the _usd contains "IMSB_Sequence field already, then it means that the sequence is native sequence and there is no need to add "IMSB_Sequence". Therefore next condition will be false if the _usd contains the "IMSB_Sequence" field and no sequence transformation will be performed as it already is".

            if (_isUSDNative == false)
            {
                //Do Transformation
                usd.Columns.Add("IMSB_Sequence");
                string sequence;
                SequenceTransformation sequenceTransformation = new SequenceTransformation();

                foreach (DataRow row in usd.Rows)
                {
                    sequence = row["Sequence"].ToString();
                    row["IMSB_Sequence"] = sequenceTransformation.ToIMSBSequence(sequence);
                }
                sequenceTransformation = null;
            }
            GenerateSubjectOritentedSiteDatasets();
        }

        //Step 3: Generate Subject Oritented Site Datasets
        public void GenerateSubjectOritentedSiteDatasets()
        {
            DataRow[] rows;
            List<string> pids = new List<string>();
            string pid;
            for (int subjectIndex = 0; subjectIndex < ModificationSites.Count; subjectIndex++)
            {
                rows = usd.Select("[ModificationSite] = '" + ModificationSites.ElementAt(subjectIndex) + "'");

                //_lstSubjects[subjectIndex].SitesDataTable = new DataTable(subjects[subjectIndex]);
                //_lstSubjects[subjectIndex].ProteinDataTable  = new DataTable(subjects[subjectIndex]);
                _lstSubjects[subjectIndex].SitesDataTable = usd.Clone();
                _lstSubjects[subjectIndex].ProteinDataTable = usd.Clone();

                foreach (DataRow row in rows)
                {
                    _lstSubjects[subjectIndex].SitesDataTable.ImportRow(row);
                    pid = row["PID"].ToString();
                    if (pids.Contains(pid) == false)
                    {
                        pids.Add(pid);
                        _lstSubjects[subjectIndex].ProteinDataTable.ImportRow(row);
                    }

                }
                pids.Clear();
            }
            GenerateSubjectOritentedProteinDatasets();
        }

        //Step 4: Generate Subject Oritented Protein Datasets
        public void GenerateSubjectOritentedProteinDatasets()
        {
            int colIndex;
            DataColumn col;
            for (int subjectIndex = 0; subjectIndex < ModificationSites.Count; subjectIndex++)
            {

                for (colIndex = 0; colIndex < _lstSubjects[subjectIndex].ProteinDataTable.Columns.Count; colIndex++)
                {
                    col = _lstSubjects[subjectIndex].ProteinDataTable.Columns[colIndex];
                    if (col.ColumnName == "PID" || col.ColumnName == "Sequence" || col.ColumnName == "IMSB_Sequence")
                    { /*DoNothing. Just Ignore*/}
                    else
                    {
                        _lstSubjects[subjectIndex].ProteinDataTable.Columns.Remove(col);
                    }
                }
            }
            GenerateSubjectOritentedPeptideDatasets();
        }

        //Step 5: Generate Subject Oritented Peptide Datasets
        public void GenerateSubjectOritentedPeptideDatasets()
        {
            PeptideGenerator peptideGenerator = new PeptideGenerator(true, this.PeptideWindowSize);
            int subjectIndex;
            for (subjectIndex = 0; subjectIndex < ModificationSites.Count; subjectIndex++)
            {
                _lstSubjects[subjectIndex].PeptideDataTable = peptideGenerator.ToPeptide(_lstSubjects[subjectIndex].SitesDataTable);
                _lstSubjects[subjectIndex].PeptideDataTable.TableName = ModificationSites.ElementAt(subjectIndex);
            }
            addtoDictionary();
        }

        public void addtoDictionary()
        {
            SubjectsHash = new Dictionary<string, Subject>();
            for(int i = 0; i < ModificationSites.Count; i++)
            {
                SubjectsHash.Add(ModificationSites.ElementAt(i), _lstSubjects[i]);
            }            
        }

        public void runPreferenceEstimationProcess()
        {
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

            prepareForAnalysis();

            using ( prs = new PreferenceResultSet(firstPosition, lastPosition))
            {

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
            dtProteins = GetProteinsDataTable();
            dtPeptides = GetPeptidesDataTable();

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

            for (int i = 0; i < dtPeptides.Rows.Count; i++)
            {
                for (int j = 0; j < totalPositions; j++)
                {
                    aminoacid = dtPeptides.Rows[i][j + 3].ToString();
                    _ObservedCount[aminoacid.IndexOf(aminoacid), j]++;
                }
            }
        }

        public void ComputeObservedFrequencyandSigma()
        {
            for (int i = 0; i < TotalAminoAcids; i++)
            {
                for (int j = 0; j < totalPositions; j++)
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
            for (int i = 0; i < TotalAminoAcids; i++)
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

            for (int i = 0; i < TotalAminoAcids; i++)
            {
                expectedFrequency = _ExpectedFrequency[i];
                expectedCount = _ExpectedCount[i];
                for (int j = 0; j < totalPositions; j++)
                {
                    dp = ((_ObservedFrequency[i, j] - expectedFrequency) / expectedFrequency) * 100;

                    if (double.IsNaN(dp) == true)
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
                    if (doec >= (2 * sigma))
                    {
                        _SPreferredSites[i, j] = mark;
                    }
                }
            }
        }

        public List<string> getAminoAcidsandPreferredPositions()
        {
            throw new NotImplementedException();
        }
    }

    class SequenceTransformation
    {
        public string ToIMSBSequence(string sequence)
        {
            char aminoacid;
            StringBuilder imsbSequence = new StringBuilder();
            for (int aminoIndex = 0; aminoIndex < sequence.Length; aminoIndex++)
            {
                aminoacid = sequence[aminoIndex];
                imsbSequence.Append(aminoacid);
                if (aminoIndex < sequence.Length - 1)
                    imsbSequence.Append(',');
            }
            return imsbSequence.ToString();
        }
    }

    [Serializable]
    class Subject : IDisposable
    {
        private string _subjectName;
        private DataTable _dtSites;
        private DataTable _dtProtein;
        private DataTable _dtPeptide;
        private PreferenceResultSet _preferrenceResult;


        public Subject(string subjectName)
        {
            _subjectName = subjectName;

        }

        public string SubjectName
        {
            set
            {
                _subjectName = value;
            }
            get
            {
                return _subjectName;
            }
        }

        public DataTable SitesDataTable
        {
            set
            {
                _dtSites = value;
            }
            get
            {
                return _dtSites;
            }
        }

        public DataTable ProteinDataTable
        {
            set
            {
                _dtProtein = value;
            }
            get
            {
                return _dtProtein;
            }
        }

        public DataTable PeptideDataTable
        {
            set
            {
                _dtPeptide = value;
            }
            get
            {
                return _dtPeptide;
            }
        }

        public PreferenceResultSet PreferredAnalysis
        {
            set
            {
                this._preferrenceResult = value;
            }
            get
            {
                return this._preferrenceResult;
            }
        }

        public PreferenceResultSet PreferredSitesDataTable
        {
            set
            {
                this._preferrenceResult = value;
            }
            get
            {
                return _preferrenceResult;
            }


        }



        public void Dispose()
        {

        }

        
        
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

    class PeptideGenerator
    {
        private bool _useIMSBSequence;
        private int _sizeOfOneSide;
        private string _peptideSequence;
        private readonly char[] sep = { ',' };
        private int start, end;
        private int _sizeOfPeptides;
        private const string dash = "-";
        private const string comma = ",";
        private string amino;
        private DataRow newRow = null;

        public PeptideGenerator(bool useIMSBSequence, int sizeOfOneSide)
        {
            _sizeOfOneSide = sizeOfOneSide;
            _sizeOfPeptides = sizeOfOneSide + sizeOfOneSide + 1;
            _useIMSBSequence = useIMSBSequence;
        }

        public bool UsingIMSBSequence
        {
            set
            {
                _useIMSBSequence = value;
            }
            get
            {
                return _useIMSBSequence;
            }
        }

        public int SizeOfOneSide
        {
            set
            {
                _sizeOfOneSide = value;
            }
            get
            {
                return _sizeOfOneSide;
            }
        }

        public string ToPeptide(string sequence, int position)
        {
            if (_useIMSBSequence == true)
            {
                GeneratePeptideFromIMSBSequence(sequence, position);
            }
            else
            {
                GeneratePeptideFromStandardSequence(sequence, position);
            }

            return this._peptideSequence;
        }

        public DataTable ToPeptide(DataTable dtSites)
        {
            using (DataTable dtPeptide = new DataTable("Peptide"))
            {
                CreatePeptideStructureIn(dtPeptide);

                int position;
                foreach (DataRow row in dtSites.Rows)
                {
                    newRow = dtPeptide.NewRow();
                    position = int.Parse(row["Position"].ToString());
                    newRow["Position"] = row["Position"];
                    newRow["PID"] = row["PID"];
                    if (_useIMSBSequence == true)
                        newRow["PeptideSequence"] = ToPeptide(row["IMSB_Sequence"].ToString(), position);
                    else
                        newRow["PeptideSequence"] = ToPeptide(row["Sequence"].ToString(), position);
                    dtPeptide.Rows.Add(newRow);
                }
                return dtPeptide;
            }//end using
        }

        private void CreatePeptideStructureIn(DataTable dtPeptide)
        {
            dtPeptide.Columns.Add("PID");
            dtPeptide.Columns.Add("Position");
            dtPeptide.Columns.Add("PeptideSequence");

            int i;
            for (i = (-1 * this._sizeOfOneSide); i <= this._sizeOfOneSide; i++)
            {
                dtPeptide.Columns.Add("P" + i.ToString());
            }
        }

        private void GeneratePeptide(string[] sequenceArray, int position)
        {
            position--; //Convert position number to index. Here after variable position will refer to the index in sequence
            start = position - this._sizeOfOneSide;
            end = position + this._sizeOfOneSide;
            _peptideSequence = "";
            int indexInPeptide = -1 * this._sizeOfOneSide;

            for (int indexInSequence = start; indexInSequence <= end; indexInSequence++)
            {
                // transverse in sequence to get peptide
                if (indexInSequence < 0)
                    amino = dash;
                else if (indexInSequence >= sequenceArray.Length)
                    amino = dash;
                else
                    amino = sequenceArray[indexInSequence];

                _peptideSequence = _peptideSequence + amino;

                if (indexInSequence < end && _useIMSBSequence == true)
                    _peptideSequence = _peptideSequence + ",";


                if (newRow != null)
                {
                    newRow["P" + indexInPeptide.ToString()] = amino;
                    indexInPeptide++;
                }
            }
        }

        private void GeneratePeptideFromIMSBSequence(string sequence, int position)
        {
            string[] sequenceArray;
            sequenceArray = sequence.Split(sep);
            GeneratePeptide(sequenceArray, position);
        }

        private void GeneratePeptideFromStandardSequence(string sequence, int position)
        {
            string[] sequenceArray;
            sequenceArray = new string[sequence.Length];

            for (int index = 0; index < sequence.Length; index++)
            {
                sequenceArray[index] = sequence[index].ToString();
            }

            GeneratePeptide(sequenceArray, position);
        }

    }

    

}
