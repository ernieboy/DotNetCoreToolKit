using System.Threading.Tasks;

namespace DotNetCoreToolKit.Library.Abstractions
{
    public interface ICommand
    {
        Task Execute();
    }
}
