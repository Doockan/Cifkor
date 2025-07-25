using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Assets.Scripts.Core.RequestQueue
{
  public class RequestQueue : IRequestQueue, IDisposable
  {
    // Храним очереди и токены отмены по типу запроса
    private readonly Queue<RequestItem> _queue = new Queue<RequestItem>();
    private bool _isProcessing = false;
    private RequestItem _currentRequest;

    private class RequestItem
    {
      public Func<CancellationToken, UniTask> RequestFunc;
      public Type RequestType;
      public CancellationToken CancellationToken;
    }

    public void Enqueue(Func<CancellationToken, UniTask> requestFunc, Type requestType,
      CancellationToken cancellationToken)
    {
      _queue.Enqueue(new RequestItem
      {
        RequestFunc = requestFunc,
        RequestType = requestType,
        CancellationToken = cancellationToken
      });
      ProcessNext().Forget();
    }

    public void RemoveRequestsOfType<T>()
    {
      var type = typeof(T);
      var newQueue = new Queue<RequestItem>();
      while (_queue.Count > 0)
      {
        var item = _queue.Dequeue();
        if (item.RequestType != type)
          newQueue.Enqueue(item);
      }

      _queue.Clear();
      foreach (var item in newQueue)
        _queue.Enqueue(item);
    }

    public void Dispose()
    {
      _queue.Clear();
      _isProcessing = false;
      _currentRequest = null;
    }

    private async UniTaskVoid ProcessNext()
    {
      if (_isProcessing || _queue.Count == 0) return;

      _isProcessing = true;
      _currentRequest = _queue.Dequeue();

      try
      {
        await _currentRequest.RequestFunc(_currentRequest.CancellationToken);
      }
      catch (OperationCanceledException)
      {
        Debug.Log("Request canceled");
      }
      catch (Exception e)
      {
        Debug.LogError($"RequestQueue error: {e.Message}");
      }

      _isProcessing = false;
      _currentRequest = null;
      if (_queue.Count > 0)
        ProcessNext().Forget();
    }
  }
}