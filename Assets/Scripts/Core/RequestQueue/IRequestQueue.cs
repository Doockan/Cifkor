using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Core.RequestQueue
{
    public interface IRequestQueue
    {
        void Enqueue(Func<CancellationToken, UniTask> requestFunc, Type requestType,
            CancellationToken cancellationToken);

        void RemoveRequestsOfType<T>();
    }
}