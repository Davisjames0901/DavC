
namespace DavCRevolution.Interfaces.ICodeObjects.ILine
{
    public interface IReturnLines : ILine
    {
        ILine ReturnBody { get; set; }
    }
}
