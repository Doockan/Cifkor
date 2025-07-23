using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Assets.Scripts.Core.RequestQueue
{
  public interface IRequest
  {
    Task ExecuteAsync(Action<object> onComplete, Action onCancel);
  }

  public interface IRequestQueue
  {
    void Enqueue(IRequest request);
    void CancelRequest(IRequest request);
    void RemoveRequest(IRequest request);
  }

  public class RequestQueue : IRequestQueue
  {
    private Queue<IRequest> _queue = new Queue<IRequest>();
    private bool _isProcessing = false;
    private IRequest _currentRequest;

    public void Enqueue(IRequest request)
    {
      _queue.Enqueue(request);
      ProcessNext();
    }

    public void CancelRequest(IRequest request)
    {
      if (_currentRequest == request)
      {
        // Cancel current request logic
      }
      else
      {
        RemoveRequest(request);
      }
    }

    public void RemoveRequest(IRequest request)
    {
      var newQueue = new Queue<IRequest>();
      while (_queue.Count > 0)
      {
        var r = _queue.Dequeue();
        if (r != request)
          newQueue.Enqueue(r);
      }

      _queue = newQueue;
    }

    private async void ProcessNext()
    {
      if (_isProcessing || _queue.Count == 0) return;
      _isProcessing = true;
      _currentRequest = _queue.Dequeue();
      await _currentRequest.ExecuteAsync(OnRequestComplete, OnRequestCancel);
    }

    private void OnRequestComplete(object result)
    {
      _isProcessing = false;
      _currentRequest = null;
      ProcessNext();
    }

    private void OnRequestCancel()
    {
      _isProcessing = false;
      _currentRequest = null;
      ProcessNext();
    }
  }
}