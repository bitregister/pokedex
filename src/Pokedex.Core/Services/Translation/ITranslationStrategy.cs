using System.Threading.Tasks;

namespace Pokedex.Core.Services.Translation
{
    public interface ITranslationStrategy
    {
        Task<string> Translate(string description);
    }
}