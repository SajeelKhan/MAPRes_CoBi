namespace java org.cs.MAPResService

service MAPResService{
    bool loginToServer(
        1: string UserName,
        2: string passWord
    )
    
 void setProjectProfileWithStandardAminoAcids(
    1: string AnalysisTitle,
    2: string AnalystName,
    3: i32 PeptideWindowSize ,
    4: set<string> ModificationSites; 
 )
void setProjectProfileWithPseudoAminoAcids(
    1: string AnalysisTitle,
    2: string AnalystName,
    3: i32 PeptideWindowSize ,
    4: set<string> ModificationSites
    5: set<string> ListOfPseudoAminoAcids
    ); //path to primary dataset

 void SaveProject();
 void openDatasetLocationForAnalysis(1: string location);//Project Location
void runPreferenceEstimationProcess();
list<string> runAssociationRuleMiningProcess(1: i32 Confidence, 2: string Peptide);
list<string> getPrimaryDataset();
list<string> getProteinsDataset();
list<string> getPeptidesDataset();
list<string> getSitesDataset();
list<string> getObservedFrequency();
list<string> getObservedFrequencyAsPerc();
list<string> getObservedCount();
list<string> getExpectedCount();
list<string> getExpectedFrequency();
list<string> getExpectedFrequencyAsPerc()
list<string> getDOEC();
list<string> getSigma();
list<string> getDeviationParameter();
    /*
    Get Proper DataType for Matrix
    */
list<string> getPreferredSitesMatrix();
list<string> getSignificantlyPreferredSitesMatrix();
list<string> getSignificantlyPreferredSites_Positive_Negative_Both();
list<string> getAminoAcidsandPreferredPositions();
list<string> getAssociationRules();
void getModsite(1: string ModSite);
void getRuleForSingleProtein(1: string PID,
2: string sequence,
3:string AnalysisName,
4: string position;)
list<string> returnDatasetToClient(1: list<string> finalList);
}
