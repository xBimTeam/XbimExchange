namespace Xbim.DPoW.Interfaces
{
    public class Project 
    {
        public string ProjectCode { get; private set; }
        public LinearUnits LinearUnits { get; set; }
        public AreaUnits AreaUnits { get; set; }
        public string ProjectName { get; set; }
        public VolumeUnits VolumeUnits { get; set; }
        public string ProjectURI { get; set; }
        public string ProjectDescription { get; set; }
        public CurrencyUnits CurrencyUnits { get; set; }
        public ProjectStage CurrentProjectStage { get; set; }
    }
}
