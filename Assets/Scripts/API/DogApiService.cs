using System.Threading;
using Assets.Scripts.Core.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace Assets.Scripts.API
{
    public class DogApiService : IDogApiService
    {
        private const string BreedsUrl = "https://dogapi.dog/api/v2/breeds";
        private const string BreedFactUrl = "https://dogapi.dog/api/v2/breeds/{0}";

        public async UniTask<DogBreedDataListModel> GetBreedsAsync(CancellationToken cancellationToken)
        {
            using (var req = UnityWebRequest.Get(BreedsUrl))
            {
                await req.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

                if (req.result != UnityWebRequest.Result.Success)
                    throw new System.Exception(req.error);

                return DogBreedDataListModel.FromJson(req.downloadHandler.text);
            }
        }

        public async UniTask<DogBreedDataModel> GetBreedAsync(string breedId, CancellationToken cancellationToken)
        {
            var url = string.Format(BreedFactUrl, breedId);
            using (var req = UnityWebRequest.Get(url))
            {
                await req.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

                if (req.result != UnityWebRequest.Result.Success)
                    throw new System.Exception(req.error);

                return DogBreedDataModel.FromJson(req.downloadHandler.text);
            }
        }
    }
}