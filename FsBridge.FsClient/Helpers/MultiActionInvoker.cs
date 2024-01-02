﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Helpers
{
    public class MultiActionInvoker
    {
        int _threadsCount;
        object _popLock = new object();
        System.Collections.Concurrent.ConcurrentDictionary<int, BlockingCollection<Action>> _actions;
        CancellationTokenSource _wantStop;
        public MultiActionInvoker()
        {
            _threadsCount = Math.Max(1, Environment.ProcessorCount / 2);
            _wantStop = new CancellationTokenSource();
            _actions = new ConcurrentDictionary<int, BlockingCollection<Action>>();
            for (int i = 0; i < _threadsCount; i++) _actions[i] = new BlockingCollection<Action>();
            for (int workerId = 0; workerId < _threadsCount; workerId++)
            {
                var localCopy = workerId;
                new System.Threading.Thread(() => StartWorker(localCopy)).Start();
            }
        }
        private void StartWorker(int workerId)
        {
            try
            {

                while (!_wantStop.Token.WaitHandle.WaitOne(10))
                {
                    while (_actions[workerId].TryTake(out var item, 1000, _wantStop.Token))
                    {
                        try
                        {
                            item?.Invoke();
                        }
                        catch (Exception x)
                        {
                        }
                    }
                }
            }
            catch (Exception c)
            {
                // operationc cancelled 
            }
        }

        public void Stop()
        {
            _wantStop.Cancel();
        }
        public void Invoke(Guid? key, Action action)
        {
            var threadId = key.HasValue ? Math.Min(_threadsCount - 1, Convert.ToInt32(((float)key.Value.ToByteArray()[15] / byte.MaxValue) * _threadsCount)) : Random.Shared.Next (_threadsCount);
            _actions[threadId].Add(action);
        }
    }
}
