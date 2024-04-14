namespace Botanist;

class BioPlanetDetail
{
    public string BodyName { get; set; }
    public int BioTotal { get; set; }
    public Dictionary<string, BioSampleDetail> SpeciesFound { get; set; }
}