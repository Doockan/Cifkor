using System;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.RequestQueue
{
    public interface IRequest
    {
        Task ExecuteAsync(Action<object> onComplete, Action onCancel);
    }
}