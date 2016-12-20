
namespace DavCRevolution.Interfaces.ICodeObjects.ILine
{
    public interface IMethodCall : ILine
    {
        string[] Args { get; set; }
        string[] Name { get; set; }
    }
}
