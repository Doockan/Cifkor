using System.Threading;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Core.Utils;

namespace Assets.Scripts.API
{
    public interface IDogApiService
    {
        UniTask<DogBreedDataListModel> GetBreedsAsync(CancellationToken cancellationToken);
        UniTask<DogBreedDataModel> GetBreedAsync(string breedId, CancellationToken cancellationToken);
    }
}