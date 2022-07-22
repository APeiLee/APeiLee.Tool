using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APeiLee.Tool
{
    /// <summary>
    /// 延时任务处理帮助类
    /// </summary>
    public class DelayProcessHelper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="delayTimespan">延时任务的延时时间</param>
        public DelayProcessHelper(int delayTimespan = 500)
        {
            _delayTimespan = delayTimespan;
        }

        /// <summary>
        /// 任务的GUID
        /// </summary>
        private string _taskGuid = String.Empty;

        /// <summary>
        /// 任务延迟时间
        /// </summary>
        private readonly int _delayTimespan;

        /// <summary>
        /// 开启延时任务
        /// </summary>
        /// <param name="taskAction">需要执行的任务</param>
        public void StartDelayTask(Action taskAction)
        {
            string tempTaskGuid;

            //为了锁住_taskGuid
            lock (this)
            {
                tempTaskGuid = _taskGuid = Guid.NewGuid().ToString();
            }

            DelayTaskProcess(tempTaskGuid, taskAction);
        }

        /// <summary>
        /// 延迟任务处理
        /// </summary>
        /// <param name="tempTaskGuid"></param>
        /// <param name="taskAction"></param>
        private void DelayTaskProcess(string tempTaskGuid, Action taskAction)
        {
            Task.Run(() =>
            {
                Thread.Sleep(_delayTimespan);

                //为了锁住_taskGuid
                lock (this)
                {
                    if (_taskGuid != tempTaskGuid)
                    {
                        return;
                    }
                }

                taskAction();
            });
        }
    }
}
