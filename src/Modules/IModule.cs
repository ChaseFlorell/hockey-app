using System.Threading.Tasks;
using HockeyApp.Models;

namespace HockeyApp.Modules
{
    public interface IModule
    {
        IModule Successor { get; set; }
        bool CanHandle(string command, string[] args);
        Task<IResponseObject> Handle();
        Task Run(string[] args);
        IModule SetSuccessor(IModule successor);
    }
}