using TreeMiner;

public class XstArtifact : ITreeArtifact
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public int Level { get; set; }
    public object Info { get; set; }
}
