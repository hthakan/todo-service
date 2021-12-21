using CacheManager;
using ErrorHandler.Models;
using System.Threading.Tasks;

namespace ErrorHandler
{
    public interface IErrorOperations
    {
        Task<ErrorItem> Get(string code);
        Task Add(ErrorItem error);
        Task<string> GetErrorDescription(string code, string extraparameter = null);
        Task<string> PrepareErrorText(string code);
        Task<string> GetErrorDescriptionFromCache(string code, ICacheClient cache, string extraparameter = null);
        Task<string> PrepareErrorTextFormCache(string code, ICacheClient cache);
    }
}
