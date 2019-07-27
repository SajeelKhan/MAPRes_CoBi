using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using Clarion;
using Clarion.Framework;
using Clarion.Framework.Core;
using Clarion.Plugins;
using Clarion.Framework.Extensions;
using Clarion.Framework.Templates;

namespace Cobi_Cognitive_Architecture
{
    public class MainClass
    {
        private static String Dataset_File_Path_AGC_Group = "C:\\Users\\M. Anwaar Khalid\\Documents\\Visual Studio 2013\\Projects\\CoBi_Project\\Kinase Dataset Cyclic-Nucleotide-Dependent family (PKA & PKG) and Protein Kinase C (PKC).xlsx";
        private static String Dataset_File_Path_PKB_Family = "C:\\Users\\M. Anwaar Khalid\\Documents\\Visual Studio 2013\\Projects\\CoBi_Project\\Kinase Dataset Protein Kinase Family (PKB).xlsx";
        private static String Dataset_File_Path_CaseinKinase_Family = "C:\\Users\\M. Anwaar Khalid\\Documents\\Visual Studio 2013\\Projects\\CoBi_Project\\Kinase Dataset Casein Kinase Family (CK1 & CK2).xlsx";
        /*
        public static void main(String [] args)
        {
            String ProteinSequence = "MENEREKQVYLAKLSEQTERYDEMVEAMKKVAQLDVELTVEERNLVSVGYKNVIGARRASWRILSSIEQKEESKGNDENVKRLKNYRKRVEDELAKVCNDILSVIDKHLIPSSNAVESTVFFYKMKGDYYRYLAEFSSGAERKEAADQSLEAYKAAVAAAENGLAPTHPVRLGLALNFSVFYYEILNSPESACQLAKQAFDDAIAELDSLNEESYKDSTLIMQLLRDNLTLWTSDLNEEGDERTKGADEPQDEN";
            
            AminoAcid[] StandardProteinSequence = getStandardProteinSequence(ProteinSequence);
            List<Peptide> Peptides = getPeptideList(StandardProteinSequence);
            Peptides = Train_Clarion_Model_AGC_Group(Peptides);
            
            Console.WriteLine("");
            Peptides = Train_Clarion_Model_PKB_Family(Peptides);

            Console.WriteLine("");
            Peptides = Train_Clarion_Model_CaseinKinase_Family(Peptides);

            Console.WriteLine("");

            for (int PeptideCount = 0; PeptideCount < Peptides.Count; PeptideCount++)
            {
                for (int KinaseCount = 0; KinaseCount < Peptides.ElementAt(PeptideCount).getModifKinases().Count; KinaseCount++)
                {
                    if (Peptides.ElementAt(PeptideCount).getModifKinases().ElementAt(KinaseCount).getStatus() == true)
                    {
                        Console.WriteLine("" + Peptides.ElementAt(PeptideCount).toString() + "\t" + Peptides.ElementAt(PeptideCount).getModifKinases().ElementAt(KinaseCount).getKinaseName()+"\tYES");
                    }
                    else
                    {
                        Console.WriteLine("" + Peptides.ElementAt(PeptideCount).toString() + "\t" + Peptides.ElementAt(PeptideCount).getModifKinases().ElementAt(KinaseCount).getKinaseName() + "\t---");
                    }
                }

                Console.WriteLine("");
            }
        }

        */
        public static List<Peptide> Train_Clarion_Model_CaseinKinase_Family(List<Peptide> Peptides)
        {

            int CorrectCount = 0, Accuracy = 0;

            List<Protein> TrainingProteins = getProteinRecordsFromTrainingDataset(Dataset_File_Path_CaseinKinase_Family);
            List<Protein> TestingProteins = getProteinRecordsFromTestingDataset(Dataset_File_Path_CaseinKinase_Family);
            List<Protein> CompleteDataset = new List<Protein>();
            CompleteDataset.AddRange(TrainingProteins);
            CompleteDataset.AddRange(TestingProteins);
            CompleteDataset.ShuffleDatasetRecords();

            String ActualKineaseName = "";
            int NumberOfInputs = CompleteDataset.ElementAt(0).getModifiedResidues().ElementAt(0).getModificationPeptide().getPeptAminoAcids().Length;
            Peptide PeptideGivenToAgent;

            Clarion_CK_Model ClarionTask = new Clarion_CK_Model();
            ClarionTask.SetNeuralNetworkInputNodes(NumberOfInputs);
            ClarionTask.SetNeuralNetworkOutputNodes();
            ClarionTask.SetAgentParameters();

            while (Accuracy < 40)
            {

                CorrectCount = 0;
                
                for (int ProteinCount = 0; ProteinCount < CompleteDataset.Count; ProteinCount++)
                {
                    ClarionTask.RefreshSensoryInformation();
                    PeptideGivenToAgent = CompleteDataset.ElementAt(ProteinCount).getModifiedResidues().ElementAt(0).getModificationPeptide();
                    ActualKineaseName = CompleteDataset.ElementAt(ProteinCount).getModifiedResidues().ElementAt(0).getKineaseName();

                    ClarionTask.SetValuesOfNeuralNetworkInputLayer(PeptideGivenToAgent);

                    ClarionTask.PerceiveSensoryInformation();

                    ExternalActionChunk ActionChunkChoosen = ClarionTask.GetChoosenActionChunk();

                    if ((ClarionTask.GetPredictionResultStatus(ActionChunkChoosen, ActualKineaseName)))
                    {
                        CorrectCount = CorrectCount + 1;
                    }

                }

                Accuracy = (int)(((double)(CorrectCount) / (double)(CompleteDataset.Count)) * (100));
            }

            String PredictedKinase = "";

            for (int PeptideCount = 0; PeptideCount < Peptides.Count; PeptideCount++)
            {
                ClarionTask.RefreshSensoryInformation();
                PeptideGivenToAgent = Peptides.ElementAt(PeptideCount);

                ClarionTask.SetValuesOfNeuralNetworkInputLayer(PeptideGivenToAgent);

                ClarionTask.PerceiveSensoryInformation();

                ExternalActionChunk ActionChunkChoosen = ClarionTask.GetChoosenActionChunk();

                if (ActionChunkChoosen == World.GetActionChunk("CK1"))
                {
                    PredictedKinase = "CK1";
                    Peptides.ElementAt(PeptideCount).setKinaseStatus(PredictedKinase);
                }
                else if (ActionChunkChoosen == World.GetActionChunk("CK2"))
                {
                    PredictedKinase = "CK2";
                    Peptides.ElementAt(PeptideCount).setKinaseStatus(PredictedKinase);
                }
               
            }

            return Peptides;
        }

        public static List<Peptide> Train_Clarion_Model_PKB_Family(List<Peptide> Peptides)
        {

            int CorrectCount = 0, Accuracy = 0;

            List<Protein> TrainingProteins = getProteinRecordsFromTrainingDataset(Dataset_File_Path_PKB_Family);
            List<Protein> TestingProteins = getProteinRecordsFromTestingDataset(Dataset_File_Path_PKB_Family);
            List<Protein> CompleteDataset = new List<Protein>();
            CompleteDataset.AddRange(TrainingProteins);
            CompleteDataset.AddRange(TestingProteins);
            CompleteDataset.ShuffleDatasetRecords();

            String ActualKineaseName = "";
            int NumberOfInputs = CompleteDataset.ElementAt(0).getModifiedResidues().ElementAt(0).getModificationPeptide().getPeptAminoAcids().Length;
            Peptide PeptideGivenToAgent;

            Clarion_PKB_Model ClarionTask = new Clarion_PKB_Model();
            ClarionTask.SetNeuralNetworkInputNodes(NumberOfInputs);
            ClarionTask.SetNeuralNetworkOutputNodes();
            ClarionTask.SetAgentParameters();

            while (Accuracy < 40)
            {

                CorrectCount = 0;

                for (int ProteinCount = 0; ProteinCount < CompleteDataset.Count; ProteinCount++)
                {
                    ClarionTask.RefreshSensoryInformation();
                    PeptideGivenToAgent = CompleteDataset.ElementAt(ProteinCount).getModifiedResidues().ElementAt(0).getModificationPeptide();
                    ActualKineaseName = CompleteDataset.ElementAt(ProteinCount).getModifiedResidues().ElementAt(0).getKineaseName();

                    ClarionTask.SetValuesOfNeuralNetworkInputLayer(PeptideGivenToAgent);

                    ClarionTask.PerceiveSensoryInformation();

                    ExternalActionChunk ActionChunkChoosen = ClarionTask.GetChoosenActionChunk();

                    if ((ClarionTask.GetPredictionResultStatus(ActionChunkChoosen, ActualKineaseName)))
                    {
                        CorrectCount = CorrectCount + 1;
                    }

                }

                Accuracy = (int)(((double)(CorrectCount) / (double)(CompleteDataset.Count)) * (100));

            }

            String PredictedKinase = "";

            for (int PeptideCount = 0; PeptideCount < Peptides.Count; PeptideCount++)
            {
                ClarionTask.RefreshSensoryInformation();
                PeptideGivenToAgent = Peptides.ElementAt(PeptideCount);

                ClarionTask.SetValuesOfNeuralNetworkInputLayer(PeptideGivenToAgent);

                ClarionTask.PerceiveSensoryInformation();

                ExternalActionChunk ActionChunkChoosen = ClarionTask.GetChoosenActionChunk();

                if (ActionChunkChoosen == World.GetActionChunk("PKB"))
                {
                    PredictedKinase = "PKB";
                    Peptides.ElementAt(PeptideCount).setKinaseStatus(PredictedKinase);
                }
                else if (ActionChunkChoosen == World.GetActionChunk("AKT1"))
                {
                    PredictedKinase = "PKB";
                    Peptides.ElementAt(PeptideCount).setKinaseStatus(PredictedKinase);
                    PredictedKinase = "AKT1";
                    Peptides.ElementAt(PeptideCount).setKinaseStatus(PredictedKinase);
                }
                else if (ActionChunkChoosen == World.GetActionChunk("AKT2"))
                {
                    PredictedKinase = "PKB";
                    Peptides.ElementAt(PeptideCount).setKinaseStatus(PredictedKinase);
                    PredictedKinase = "AKT2";
                    Peptides.ElementAt(PeptideCount).setKinaseStatus(PredictedKinase);
                }

            }

            return Peptides;
        }
        
        public static List<Peptide> Train_Clarion_Model_AGC_Group(List<Peptide> Peptides)
        {

            int CorrectCount = 0, Accuracy = 0;

            List<Protein> TrainingProteins = getProteinRecordsFromTrainingDataset(Dataset_File_Path_AGC_Group);
            List<Protein> TestingProteins = getProteinRecordsFromTestingDataset(Dataset_File_Path_AGC_Group);
            List<Protein> CompleteDataset = new List<Protein>();
            CompleteDataset.AddRange(TrainingProteins);
            CompleteDataset.AddRange(TestingProteins);
            CompleteDataset.ShuffleDatasetRecords();

            String ActualKineaseName = "";
            int NumberOfInputs = CompleteDataset.ElementAt(0).getModifiedResidues().ElementAt(0).getModificationPeptide().getPeptAminoAcids().Length;
            Peptide PeptideGivenToAgent;

            Clarion_AGC_Model ClarionTask = new Clarion_AGC_Model();
            ClarionTask.SetNeuralNetworkInputNodes(NumberOfInputs);
            ClarionTask.SetNeuralNetworkOutputNodes();
            ClarionTask.SetAgentParameters();

            while (Accuracy < 50)
            {

                CorrectCount = 0;

                for (int ProteinCount = 0; ProteinCount < CompleteDataset.Count; ProteinCount++)
                {
                    ClarionTask.RefreshSensoryInformation();
                    PeptideGivenToAgent = CompleteDataset.ElementAt(ProteinCount).getModifiedResidues().ElementAt(0).getModificationPeptide();
                    ActualKineaseName = CompleteDataset.ElementAt(ProteinCount).getModifiedResidues().ElementAt(0).getKineaseName();

                    ClarionTask.SetValuesOfNeuralNetworkInputLayer(PeptideGivenToAgent);

                    ClarionTask.PerceiveSensoryInformation();

                    ExternalActionChunk ActionChunkChoosen = ClarionTask.GetChoosenActionChunk();

                    if ((ClarionTask.GetPredictionResultStatus(ActionChunkChoosen, ActualKineaseName)))
                    {
                        CorrectCount = CorrectCount + 1;
                    }

                }

                Accuracy = (int)(((double)(CorrectCount) / (double)(CompleteDataset.Count)) * (100));

            }

            String PredictedKinase = "";

            for (int PeptideCount = 0; PeptideCount < Peptides.Count; PeptideCount++)
            {
                ClarionTask.RefreshSensoryInformation();
                PeptideGivenToAgent = Peptides.ElementAt(PeptideCount);

                ClarionTask.SetValuesOfNeuralNetworkInputLayer(PeptideGivenToAgent);

                ClarionTask.PerceiveSensoryInformation();

                ExternalActionChunk ActionChunkChoosen = ClarionTask.GetChoosenActionChunk();

                if (ActionChunkChoosen == World.GetActionChunk("PKA"))
                {
                    PredictedKinase = "PKA";
                    Peptides.ElementAt(PeptideCount).setKinaseStatus(PredictedKinase);
                }
                else if (ActionChunkChoosen == World.GetActionChunk("PKG"))
                {
                    PredictedKinase = "PKG";
                    Peptides.ElementAt(PeptideCount).setKinaseStatus(PredictedKinase);
                }
                else if (ActionChunkChoosen == World.GetActionChunk("PKC"))
                {
                    PredictedKinase = "PKC";
                    Peptides.ElementAt(PeptideCount).setKinaseStatus(PredictedKinase);
                }
                
            }

            return Peptides;
        }

        public static List<Protein> getProteinRecordsFromTrainingDataset(String Dataset_File_Path)
        {
            Excel.Application ExcelApplication = new Excel.Application();
            Excel.Workbook ExcelWorkbook = ExcelApplication.Workbooks.Open(Dataset_File_Path);
            Excel.Worksheet ExcelWorksheet = ExcelWorkbook.Sheets[1];
            Excel.Range ExcelSheetRange = ExcelWorksheet.UsedRange;

            int NumberOfRows = ExcelSheetRange.Rows.Count;
            int NumberOfColumn = ExcelSheetRange.Columns.Count;

            String ProteinID = "", ModifiedResidueName = "", AminoAcidName = "", KineaseName = "", ProteinSequence = "";
            int ModificationSite = 0, SequenceLength = 0;
            char Symbol = '\0';
            List<Protein> Proteins = new List<Protein>();

            for (int RowCount = 2; RowCount <= NumberOfRows; RowCount++)
            {
                for (int ColumnCount = 1; ColumnCount <= NumberOfColumn; ColumnCount++)
                {
                    if (ExcelSheetRange.Cells[RowCount, ColumnCount] != null && ExcelSheetRange.Cells[RowCount, ColumnCount].Value2 != null)
                    {

                        switch (ColumnCount)
                        {

                            case 1:
                                {
                                    ProteinID = ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString();
                                    break;
                                }
                            case 2:
                                {
                                    ModificationSite = Int32.Parse(ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString());
                                    break;
                                }
                            case 3:
                                {
                                    ModifiedResidueName = ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString();
                                    break;
                                }
                            case 4:
                                {
                                    AminoAcidName = ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString();
                                    break;
                                }
                            case 5:
                                {
                                    Symbol = char.Parse(ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString());
                                    break;
                                }
                            case 6:
                                {
                                    KineaseName = ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString();
                                    break;
                                }
                            case 7:
                                {
                                    ProteinSequence = ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString();
                                    break;
                                }
                            case 8:
                                {
                                    SequenceLength = Int32.Parse(ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString());
                                    break;
                                }
                        }
                    }
                }

                AminoAcid ModifAminoAcid = new AminoAcid(AminoAcidName, Symbol);
                AminoAcid[] StandardProteinSequence = getStandardProteinSequence(ProteinSequence);
                Peptide ModifPeptide = getPeptide(StandardProteinSequence, ModificationSite);
                ModifiedResidue ModifResidue = new ModifiedResidue(ModificationSite, ModifiedResidueName, ModifAminoAcid, KineaseName, ModifPeptide);
                Protein protein = new Protein(ProteinID, ProteinSequence, SequenceLength);
                protein.addModifiedResidue(ModifResidue);
                Proteins.Add(protein);

            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(ExcelSheetRange);
            Marshal.ReleaseComObject(ExcelWorksheet);

            ExcelWorkbook.Close();
            Marshal.ReleaseComObject(ExcelWorkbook);

            ExcelApplication.Quit();
            Marshal.ReleaseComObject(ExcelApplication);

            return Proteins;
        }

        public static List<Protein> getProteinRecordsFromTestingDataset(String Dataset_File_Path)
        {
            Excel.Application ExcelApplication = new Excel.Application();
            Excel.Workbook ExcelWorkbook = ExcelApplication.Workbooks.Open(Dataset_File_Path);
            Excel.Worksheet ExcelWorksheet = ExcelWorkbook.Sheets[2];
            Excel.Range ExcelSheetRange = ExcelWorksheet.UsedRange;

            int NumberOfRows = ExcelSheetRange.Rows.Count;
            int NumberOfColumn = ExcelSheetRange.Columns.Count;

            String ProteinID = "", ModifiedResidueName = "", AminoAcidName = "", KineaseName = "", ProteinSequence = "";
            int ModificationSite = 0, SequenceLength = 0;
            char Symbol = '\0';
            List<Protein> Proteins = new List<Protein>();

            for (int RowCount = 2; RowCount <= NumberOfRows; RowCount++)
            {
                for (int ColumnCount = 1; ColumnCount <= NumberOfColumn; ColumnCount++)
                {
                    if (ExcelSheetRange.Cells[RowCount, ColumnCount] != null && ExcelSheetRange.Cells[RowCount, ColumnCount].Value2 != null)
                    {

                        switch (ColumnCount)
                        {

                            case 1:
                                {
                                    ProteinID = ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString();
                                    break;
                                }
                            case 2:
                                {
                                    ModificationSite = Int32.Parse(ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString());
                                    break;
                                }
                            case 3:
                                {
                                    ModifiedResidueName = ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString();
                                    break;
                                }
                            case 4:
                                {
                                    AminoAcidName = ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString();
                                    break;
                                }
                            case 5:
                                {
                                    Symbol = char.Parse(ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString());
                                    break;
                                }
                            case 6:
                                {
                                    KineaseName = ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString();
                                    break;
                                }
                            case 7:
                                {
                                    ProteinSequence = ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString();
                                    break;
                                }
                            case 8:
                                {
                                    SequenceLength = Int32.Parse(ExcelSheetRange.Cells[RowCount, ColumnCount].Value2.ToString());
                                    break;
                                }
                        }
                    }
                }

                AminoAcid ModifAminoAcid = new AminoAcid(AminoAcidName, Symbol);
                AminoAcid[] StandardProteinSequence = getStandardProteinSequence(ProteinSequence);
                Peptide ModifPeptide = getPeptide(StandardProteinSequence, ModificationSite);
                ModifiedResidue ModifResidue = new ModifiedResidue(ModificationSite, ModifiedResidueName, ModifAminoAcid, KineaseName, ModifPeptide);
                Protein protein = new Protein(ProteinID, ProteinSequence, SequenceLength);
                protein.addModifiedResidue(ModifResidue);
                Proteins.Add(protein);

            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(ExcelSheetRange);
            Marshal.ReleaseComObject(ExcelWorksheet);

            ExcelWorkbook.Close();
            Marshal.ReleaseComObject(ExcelWorkbook);

            ExcelApplication.Quit();
            Marshal.ReleaseComObject(ExcelApplication);

            return Proteins;
        }

        public static AminoAcid[] getStandardProteinSequence(String ProteinSequence)
        {
            #region Amino Acid List

                List<AminoAcid> AminoAcidList = new List<AminoAcid>();
                AminoAcidList.Add(new AminoAcid("Alanine", 'A'));
                AminoAcidList.Add(new AminoAcid("Arginine", 'R'));
                AminoAcidList.Add(new AminoAcid("Asparagine", 'N'));
                AminoAcidList.Add(new AminoAcid("Aspartic Acid", 'D'));
                AminoAcidList.Add(new AminoAcid("Cystein", 'C'));
                AminoAcidList.Add(new AminoAcid("Glutamine", 'Q'));
                AminoAcidList.Add(new AminoAcid("Glutamic Acid", 'E'));
                AminoAcidList.Add(new AminoAcid("Glycine", 'G'));
                AminoAcidList.Add(new AminoAcid("Histidine", 'H'));
                AminoAcidList.Add(new AminoAcid("Isoleucine", 'I'));
                AminoAcidList.Add(new AminoAcid("Leucine", 'L'));
                AminoAcidList.Add(new AminoAcid("Lysine", 'K'));
                AminoAcidList.Add(new AminoAcid("Methionine", 'M'));
                AminoAcidList.Add(new AminoAcid("Phenylalanine", 'F'));
                AminoAcidList.Add(new AminoAcid("Proline", 'P'));
                AminoAcidList.Add(new AminoAcid("Serine", 'S'));
                AminoAcidList.Add(new AminoAcid("Threonine", 'T'));
                AminoAcidList.Add(new AminoAcid("Tryptophan", 'W'));
                AminoAcidList.Add(new AminoAcid("Tyrosine", 'Y'));
                AminoAcidList.Add(new AminoAcid("Valine", 'V'));

            #endregion

            AminoAcid[] StandardProteinSequence = new AminoAcid[ProteinSequence.Length];
            char SequenceChar;

            for (int i = 0; i < ProteinSequence.Length; i++)
            {
                SequenceChar = ProteinSequence.ElementAt(i);

                for (int j = 0; j < AminoAcidList.Count; j++)
                {
                    if (AminoAcidList.ElementAt(j).getSymbol().Equals(SequenceChar))
                    {
                        StandardProteinSequence[i] = AminoAcidList.ElementAt(j);
                        break;
                    }

                }

            }

            return StandardProteinSequence;
        }

        public static Peptide getPeptide(AminoAcid[] StandardProteinSequence, int ModificationSite)
        {
            AminoAcid[] PeptAminoAcids = new AminoAcid[9];
            int PeptIndex, SeqncIndex;

            for (int i = 0; i < StandardProteinSequence.Length; i++)
            {
                if ((i + 1) == ModificationSite)
                {

                    PeptAminoAcids[4] = StandardProteinSequence.ElementAt(i);

                    PeptIndex = 3;
                    SeqncIndex = (i - 1);

                    while (PeptIndex != -1)
                    {
                        if (SeqncIndex < 0)
                        {
                            PeptAminoAcids[PeptIndex] = new AminoAcid("Null", 'X');
                        }
                        else
                        {
                            PeptAminoAcids[PeptIndex] = StandardProteinSequence[SeqncIndex];
                        }

                        PeptIndex--;
                        SeqncIndex--;
                    }

                    PeptIndex = 5;
                    SeqncIndex = (i + 1);

                    while (PeptIndex != 9)
                    {
                        if (SeqncIndex > (StandardProteinSequence.Length - 1))
                        {
                            PeptAminoAcids[PeptIndex] = new AminoAcid("Null", 'X');
                        }
                        else
                        {
                            PeptAminoAcids[PeptIndex] = StandardProteinSequence[SeqncIndex];
                        }

                        PeptIndex++;
                        SeqncIndex++;
                    }

                    break;
                }
            }

            return new Peptide(PeptAminoAcids);
        }

        public static List<Peptide> getPeptideList(AminoAcid[] StandardProteinSequence, Boolean SerineChecked, Boolean ThreonineChecked, Boolean TyrosineChecked, Boolean AllChecked)
        {
            List<Peptide> peptides = new List<Peptide>();
            int PeptIndex, SeqncIndex;

            for (int i = 0; i < StandardProteinSequence.Length; i++)
            {
                AminoAcid[] PeptAminoAcids = new AminoAcid[9];

                if(SerineChecked == true){

                    if (StandardProteinSequence.ElementAt(i).getSymbol().Equals('S')) {

                        PeptAminoAcids[4] = StandardProteinSequence.ElementAt(i);

                        PeptIndex = 3;
                        SeqncIndex = (i - 1);

                        while (PeptIndex != -1)
                        {
                            if (SeqncIndex < 0)
                            {
                                PeptAminoAcids[PeptIndex] = new AminoAcid("Null", 'X');
                            }
                            else
                            {
                                PeptAminoAcids[PeptIndex] = StandardProteinSequence[SeqncIndex];
                            }

                            PeptIndex--;
                            SeqncIndex--;
                        }

                        PeptIndex = 5;
                        SeqncIndex = (i + 1);

                        while (PeptIndex != 9)
                        {
                            if (SeqncIndex > (StandardProteinSequence.Length - 1))
                            {
                                PeptAminoAcids[PeptIndex] = new AminoAcid("Null", 'X');
                            }
                            else
                            {
                                PeptAminoAcids[PeptIndex] = StandardProteinSequence[SeqncIndex];
                            }

                            PeptIndex++;
                            SeqncIndex++;
                        }

                        Peptide peptide = new Peptide(PeptAminoAcids);
                        peptides.Add(peptide);
                    }
                }
                else if (ThreonineChecked == true)
                {

                    if (StandardProteinSequence.ElementAt(i).getSymbol().Equals('T'))
                    {

                        PeptAminoAcids[4] = StandardProteinSequence.ElementAt(i);

                        PeptIndex = 3;
                        SeqncIndex = (i - 1);

                        while (PeptIndex != -1)
                        {
                            if (SeqncIndex < 0)
                            {
                                PeptAminoAcids[PeptIndex] = new AminoAcid("Null", 'X');
                            }
                            else
                            {
                                PeptAminoAcids[PeptIndex] = StandardProteinSequence[SeqncIndex];
                            }

                            PeptIndex--;
                            SeqncIndex--;
                        }

                        PeptIndex = 5;
                        SeqncIndex = (i + 1);

                        while (PeptIndex != 9)
                        {
                            if (SeqncIndex > (StandardProteinSequence.Length - 1))
                            {
                                PeptAminoAcids[PeptIndex] = new AminoAcid("Null", 'X');
                            }
                            else
                            {
                                PeptAminoAcids[PeptIndex] = StandardProteinSequence[SeqncIndex];
                            }

                            PeptIndex++;
                            SeqncIndex++;
                        }

                        Peptide peptide = new Peptide(PeptAminoAcids);
                        peptides.Add(peptide);
                    }
                }
                else if (TyrosineChecked == true)
                {

                    if (StandardProteinSequence.ElementAt(i).getSymbol().Equals('Y'))
                    {

                        PeptAminoAcids[4] = StandardProteinSequence.ElementAt(i);

                        PeptIndex = 3;
                        SeqncIndex = (i - 1);

                        while (PeptIndex != -1)
                        {
                            if (SeqncIndex < 0)
                            {
                                PeptAminoAcids[PeptIndex] = new AminoAcid("Null", 'X');
                            }
                            else
                            {
                                PeptAminoAcids[PeptIndex] = StandardProteinSequence[SeqncIndex];
                            }

                            PeptIndex--;
                            SeqncIndex--;
                        }

                        PeptIndex = 5;
                        SeqncIndex = (i + 1);

                        while (PeptIndex != 9)
                        {
                            if (SeqncIndex > (StandardProteinSequence.Length - 1))
                            {
                                PeptAminoAcids[PeptIndex] = new AminoAcid("Null", 'X');
                            }
                            else
                            {
                                PeptAminoAcids[PeptIndex] = StandardProteinSequence[SeqncIndex];
                            }

                            PeptIndex++;
                            SeqncIndex++;
                        }

                        Peptide peptide = new Peptide(PeptAminoAcids);
                        peptides.Add(peptide);
                    }
                }
                else if (AllChecked == true)
                {

                    if (StandardProteinSequence.ElementAt(i).getSymbol().Equals('S') || StandardProteinSequence.ElementAt(i).getSymbol().Equals('T') || StandardProteinSequence.ElementAt(i).getSymbol().Equals('Y'))
                    {

                        PeptAminoAcids[4] = StandardProteinSequence.ElementAt(i);

                        PeptIndex = 3;
                        SeqncIndex = (i - 1);

                        while (PeptIndex != -1)
                        {
                            if (SeqncIndex < 0)
                            {
                                PeptAminoAcids[PeptIndex] = new AminoAcid("Null", 'X');
                            }
                            else
                            {
                                PeptAminoAcids[PeptIndex] = StandardProteinSequence[SeqncIndex];
                            }

                            PeptIndex--;
                            SeqncIndex--;
                        }

                        PeptIndex = 5;
                        SeqncIndex = (i + 1);

                        while (PeptIndex != 9)
                        {
                            if (SeqncIndex > (StandardProteinSequence.Length - 1))
                            {
                                PeptAminoAcids[PeptIndex] = new AminoAcid("Null", 'X');
                            }
                            else
                            {
                                PeptAminoAcids[PeptIndex] = StandardProteinSequence[SeqncIndex];
                            }

                            PeptIndex++;
                            SeqncIndex++;
                        }

                        Peptide peptide = new Peptide(PeptAminoAcids);
                        peptides.Add(peptide);
                    }
                }

            }

            return peptides;
        }
    }


    public static class IListShuffle
    {
        public static void ShuffleDatasetRecords<T>(this IList<T> ProteinRecords)
        {
            Random rand = new Random();
            int NumberOfRecords = ProteinRecords.Count;

            while (NumberOfRecords > 1)
            {
                NumberOfRecords = NumberOfRecords - 1;

                int GeneratedRandomValue = rand.Next(NumberOfRecords + 1);
                T value = ProteinRecords[GeneratedRandomValue];
                ProteinRecords[GeneratedRandomValue] = ProteinRecords[NumberOfRecords];
                ProteinRecords[NumberOfRecords] = value;

            }
        }
    }
}

