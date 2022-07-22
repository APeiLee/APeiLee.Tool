using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace APeiLee.Tool
{
    public class TaskWaitHelper
    {
        /// <summary>
        /// 任务并发帮助类
        /// 设置一个任务的最大并发数量，使用TaskNumAddOne添加任务数量（_currentTaskNum+=1），当_currentTaskNum达到最大任务数量时将阻塞
        /// 使用TaskNumSubOne以减少一个数量
        /// 最后可以使用WaitAllComplete方法等待所有任务完成
        /// </summary>
        /// <param name="maxTaskNum">缓冲任务数量</param>
        /// <param name="taskDetectionInterval">任务检测间歇时间</param>
        public TaskWaitHelper(int maxTaskNum, int taskDetectionInterval = 200)
        {
            if (maxTaskNum < 1)
            {
                throw new ArgumentException("maxTaskNum must > 1");
            }

            if (taskDetectionInterval < 0)
            {
                throw new ArgumentException("taskDetectionInterval must > 0");
            }

            _maxChanelTaskNum = maxTaskNum;
            _taskDetectionIntervalTime = taskDetectionInterval;
        }

        /// <summary>
        /// 任务检测间歇时间
        /// </summary>
        private readonly int _taskDetectionIntervalTime = 200;

        /// <summary>
        /// 最大任务数量
        /// </summary>
        private readonly int _maxChanelTaskNum;

        /// <summary>
        /// 当前任务数量
        /// </summary>
        private int _currentTaskNum;

        /// <summary>
        /// 新增一个任务，如果任务达到最大任务数量，那么将阻塞，直至运行中的任务完成一个后方可添加
        /// </summary>
        /// <returns></returns>
        public async Task AddOne()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    lock (this)
                    {
                        if (_currentTaskNum < _maxChanelTaskNum)
                        {
                            _currentTaskNum++;
                            Debug.WriteLine($"TaskWaitHelper.TaskNum AddOne, CurrentTaskNum = {_currentTaskNum}");
                            break;
                        }
                    }

                    Thread.Sleep(_taskDetectionIntervalTime);
                }
            });
        }

        /// <summary>
        /// 减少一个任务
        /// </summary>
        /// <returns></returns>
        public async Task SubOne()
        {
            await Task.Run(() =>
            {
                lock (this)
                {
                    _currentTaskNum--;
                    Debug.WriteLine($"TaskWaitHelper.TaskNum SubOne, CurrentTaskNum = {_currentTaskNum}");
                }
            });
        }

        /// <summary>
        /// 等待所有任务完成
        /// </summary>
        /// <returns></returns>
        public async Task WaitAllComplete()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    lock (this)
                    {
                        if (_currentTaskNum == 0)
                        {
                            Debug.WriteLine("TaskWaitHelper.WaitAllComplete Done.");
                            break;
                        }
                    }

                    Thread.Sleep(_taskDetectionIntervalTime);
                }
            });
        }
    }
}
