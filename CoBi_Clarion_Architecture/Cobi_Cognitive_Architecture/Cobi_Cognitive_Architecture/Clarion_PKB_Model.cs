using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Clarion;
using Clarion.Framework;
using Clarion.Plugins;
using Clarion.Framework.Extensions;
using Clarion.Framework.Templates;

namespace Cobi_Cognitive_Architecture
{
    public class Clarion_PKB_Model
    {
        private Agent MAKaey;
        private SimplifiedQBPNetwork NeuralNetwork;
        private SensoryInformation sensInfo;

        public Clarion_PKB_Model()
        {
            World.LoggingLevel = System.Diagnostics.TraceLevel.Off;

            this.MAKaey = World.NewAgent("MAKaey");
            NeuralNetwork = AgentInitializer.InitializeImplicitDecisionNetwork(MAKaey, SimplifiedQBPNetwork.Factory);

        }

        public Clarion_PKB_Model(Agent MAKaey)
        {
            World.LoggingLevel = System.Diagnostics.TraceLevel.Off;

            this.MAKaey = MAKaey;
            NeuralNetwork = AgentInitializer.InitializeImplicitDecisionNetwork(MAKaey, SimplifiedQBPNetwork.Factory);
        }

        public void setAgent(Agent MAKaey)
        {
            this.MAKaey = MAKaey;
        }

        public Agent getAgent()
        {
            return this.MAKaey;
        }

        public void SetNeuralNetworkInputNodes(int NumberOfInputNodes)
        {
            #region Amino Acid List

                List<AminoAcid> AminoAcidList = new List<AminoAcid>();

                #region List Entries
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
                    AminoAcidList.Add(new AminoAcid("Default", 'X'));
                #endregion

            #endregion

            String NodeID = null;
            for (int NetworkNodeCount = 1; NetworkNodeCount <= NumberOfInputNodes; NetworkNodeCount++)
            {
                NodeID = "NetworkNode_" + NetworkNodeCount;

                for (int AminoAcidCount = 0; AminoAcidCount < AminoAcidList.Count; AminoAcidCount++)
                {
                    DimensionValuePair DimValuePair = World.NewDimensionValuePair(NodeID, AminoAcidList.ElementAt(AminoAcidCount).getSymbol());
                    NeuralNetwork.Input.Add(DimValuePair);
                }
            }

        }

        public void SetNeuralNetworkOutputNodes()
        {
            ExternalActionChunk Predict_PKB = World.NewExternalActionChunk("PKB");
            ExternalActionChunk Predict_AKT1 = World.NewExternalActionChunk("AKT1");
            ExternalActionChunk Predict_AKT2 = World.NewExternalActionChunk("AKT2");

            NeuralNetwork.Output.Add(Predict_PKB);
            NeuralNetwork.Output.Add(Predict_AKT1);
            NeuralNetwork.Output.Add(Predict_AKT2);
        }

        public void SetAgentParameters()
        {
            MAKaey.ACS.Parameters.PERFORM_RER_REFINEMENT = false;
            MAKaey.ACS.Parameters.SELECTION_TEMPERATURE = 0.001;
            MAKaey.ACS.Parameters.FIXED_BL_LEVEL_SELECTION_MEASURE = 0.5;
            MAKaey.ACS.Parameters.FIXED_FR_LEVEL_SELECTION_MEASURE = 0;
            MAKaey.ACS.Parameters.FIXED_IRL_LEVEL_SELECTION_MEASURE = 0.5;
            MAKaey.ACS.Parameters.FIXED_RER_LEVEL_SELECTION_MEASURE = 0;
            MAKaey.ACS.Parameters.LEVEL_SELECTION_OPTION = ActionCenteredSubsystem.LevelSelectionOptions.FIXED;

            NeuralNetwork.Parameters.LEARNING_RATE = 1;
            NeuralNetwork.Parameters.MOMENTUM = 0.09;

            MAKaey.Commit(NeuralNetwork);

        }

        public void SetValuesOfNeuralNetworkInputLayer(Peptide PeptideGivenToAgent)
        {
            #region Amino Acid List

                List<AminoAcid> AminoAcidList = new List<AminoAcid>();

                #region List Entries
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
                    AminoAcidList.Add(new AminoAcid("Default", 'X'));
                #endregion

            #endregion

            String NodeID = null;
            for (int NetworkNodeCount = 0; NetworkNodeCount < PeptideGivenToAgent.getPeptAminoAcids().Length; NetworkNodeCount++)
            {
                NodeID = "NetworkNode_" + (NetworkNodeCount + 1);

                for (int AminoAcidCount = 0; AminoAcidCount < AminoAcidList.Count; AminoAcidCount++)
                {
                    if (PeptideGivenToAgent.getPeptAminoAcids().ElementAt(NetworkNodeCount).getSymbol().Equals(AminoAcidList.ElementAt(AminoAcidCount).getSymbol()))
                    {

                        sensInfo.Add(World.GetDimensionValuePair(NodeID, AminoAcidList.ElementAt(AminoAcidCount).getSymbol()), MAKaey.Parameters.MAX_ACTIVATION);
                    }
                    else
                    {

                        sensInfo.Add(World.GetDimensionValuePair(NodeID, AminoAcidList.ElementAt(AminoAcidCount).getSymbol()), MAKaey.Parameters.MIN_ACTIVATION);
                    }
                }
            }
        }

        public Boolean GetPredictionResultStatus(ExternalActionChunk ActionChunkChoosen, String ActualKinease)
        {
            Boolean ResultFlag = false;

            if (ActionChunkChoosen == World.GetActionChunk("PKB"))
            {
                if (ActualKinease.Equals("PKB"))
                {
                    Trace.WriteLineIf(World.LoggingSwitch.TraceWarning, "MAKaey Responded Correctly");
                    MAKaey.ReceiveFeedback(sensInfo, 1.0);
                    ResultFlag = true;
                }
                else
                {
                    Trace.WriteLineIf(World.LoggingSwitch.TraceWarning, "MAKaey Responded In-correctly");
                    MAKaey.ReceiveFeedback(sensInfo, 0.0);
                    ResultFlag = false;
                }
            }
            else if (ActionChunkChoosen == World.GetActionChunk("AKT1"))
            {
                if (ActualKinease.Equals("AKT1"))
                {
                    Trace.WriteLineIf(World.LoggingSwitch.TraceWarning, "MAKaey Responded Correctly");
                    MAKaey.ReceiveFeedback(sensInfo, 1.0);
                    ResultFlag = true;
                }
                else
                {
                    Trace.WriteLineIf(World.LoggingSwitch.TraceWarning, "MAKaey Responded In-correctly");
                    MAKaey.ReceiveFeedback(sensInfo, 0.0);
                    ResultFlag = false;

                }
            }
            else if (ActionChunkChoosen == World.GetActionChunk("AKT2"))
            {
                if (ActualKinease.Equals("AKT2"))
                {
                    Trace.WriteLineIf(World.LoggingSwitch.TraceWarning, "MAKaey Responded Correctly");
                    MAKaey.ReceiveFeedback(sensInfo, 1.0);
                    ResultFlag = true;
                }
                else
                {
                    Trace.WriteLineIf(World.LoggingSwitch.TraceWarning, "MAKaey Responded In-correctly");
                    MAKaey.ReceiveFeedback(sensInfo, 0.0);
                    ResultFlag = false;
                }
            }

            return ResultFlag;
        }

        public void setSensoryInformation(SensoryInformation sensoryInfo)
        {
            this.sensInfo = sensoryInfo;
        }

        public void RefreshSensoryInformation()
        {
            this.sensInfo = World.NewSensoryInformation(this.MAKaey);
        }

        public void PerceiveSensoryInformation()
        {
            this.MAKaey.Perceive(this.sensInfo);
        }

        public ExternalActionChunk GetChoosenActionChunk()
        {
            return this.MAKaey.GetChosenExternalAction(this.sensInfo);
        }
    }
}
