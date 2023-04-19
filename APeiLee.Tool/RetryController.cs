using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace APeiLee.Tool
{
    /// <summary>
    /// 重试控制器
    /// </summary>
    public static class RetryController
    {
        /// <summary>
        /// 重试指定次数（maxRetries），中途出现异常则重试，直到重试次数用完
        /// </summary>
        /// <param name="maxRetries">重试次数；如果重试次数小于等于0，那么一直循环，直到正确执行方法</param>
        /// <param name="sleepTimeMs">每次尝试间隔时间</param>
        /// <param name="action"></param>
        public static void Retry(int maxRetries, int sleepTimeMs, Action action)
        {
            int retries = 0;//当前重试次数
            //如果maxRetries小于等于0，那么一直循环
            if (maxRetries <= 0)
            {
                retries = maxRetries - 1;
            }

            while (retries < maxRetries)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception)
                {
                    if (sleepTimeMs > 0)//如果sleepTimeMs小于等于0，则不休眠
                    {
                        Thread.Sleep(sleepTimeMs);
                    }

                    if (maxRetries > 0)
                    {
                        retries++;
                    }
                }
            }
        }

        /// <summary>
        /// 重试指定次数（maxRetries），中途出现异常则重试，直到重试次数用完；如果重试次数用完仍有异常，则抛出该异常
        /// </summary>
        /// <param name="maxRetries">重试次数；如果重试次数小于等于0，那么一直循环，直到正确执行方法</param>
        /// <param name="sleepTimeMs">每次尝试间隔时间</param>
        /// <param name="action"></param>
        /// <exception cref="Exception">重试次数用完后，仍然出现异常，将抛出该异常</exception>
        public static void RetryWithException(int maxRetries, int sleepTimeMs, Action action)
        {
            int retries = 0;//当前重试次数
            //如果maxRetries小于等于0，那么一直循环
            if (maxRetries <= 0)
            {
                retries = maxRetries - 1;
            }

            while (retries < maxRetries)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception ex)
                {
                    if (sleepTimeMs > 0)//如果sleepTimeMs小于等于0，则不休眠
                    {
                        Thread.Sleep(sleepTimeMs);
                    }

                    if (maxRetries > 0)
                    {
                        retries++;
                    }

                    if (retries >= maxRetries)//如果达到指定的重试次数，则抛出异常
                    {
                        throw ex;
                    }
                }
            }
        }


        /// <summary>
        /// 重试指定次数（maxRetries），直到条件func满足，中途出现异常则重试，直到重试次数用完；如果重试次数用完仍有异常，则抛出该异常
        /// </summary>
        /// <param name="maxRetries"></param>
        /// <param name="sleepTimeMs"></param>
        /// <param name="action"></param>
        /// <param name="func">退出条件</param>
        /// <param name="funcErrorMsg">如果不满足条件时的错误信息</param>
        public static void RetryWithExceptionUtil(int maxRetries, int sleepTimeMs, Action action, Func<bool> func, string funcErrorMsg = "")
        {
            int retries = 0;//当前重试次数
            //如果maxRetries小于等于0，那么一直循环
            if (maxRetries <= 0)
            {
                retries = maxRetries - 1;
            }

            while (retries < maxRetries)
            {
                try
                {
                    action();

                    if (func())//如果条件满足，那么退出，否则继续循环
                    {
                        break;
                    }
                    else
                    {
                        throw new Exception(string.IsNullOrEmpty(funcErrorMsg) ? "未满足指定结果条件" : funcErrorMsg);
                    }
                }
                catch (Exception ex)
                {
                    if (sleepTimeMs > 0)//如果sleepTimeMs小于等于0，则不休眠
                    {
                        Thread.Sleep(sleepTimeMs);
                    }

                    if (maxRetries > 0)
                    {
                        retries++;
                    }

                    if (retries >= maxRetries)//如果达到指定的重试次数，则抛出异常
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 重试指定次数（maxRetries），中途出现异常则重试，直到重试次数用完
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="maxRetries">重试次数；如果重试次数小于等于0，那么一直循环，直到正确执行方法</param>
        /// <param name="sleepTimeMs">每次尝试间隔时间</param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static T Retry<T>(int maxRetries, int sleepTimeMs, Func<T> function)
        {
            int retries = 0;//当前重试次数
            //如果maxRetries小于等于0，那么一直循环
            if (maxRetries <= 0)
            {
                retries = maxRetries - 1;
            }

            while (retries < maxRetries)
            {
                try
                {
                    return function();
                }
                catch (Exception ex)
                {
                    if (sleepTimeMs > 0)//如果sleepTimeMs小于等于0，则不休眠
                    {
                        Thread.Sleep(sleepTimeMs);
                    }

                    if (maxRetries > 0)//如果maxRetries才计数，如果maxRetries小于零，那么将一直循环
                    {
                        retries++;
                    }

                    Debug.WriteLine(ex.Message);
                }
            }
            return default;
        }

        /// <summary>
        /// 重试指定次数（maxRetries），中途出现异常则重试，直到重试次数用完
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="maxRetries">重试次数；如果重试次数小于等于0，那么一直循环，直到正确执行方法</param>
        /// <param name="sleepTimeMs">每次尝试间隔时间</param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static T RetryWithException<T>(int maxRetries, int sleepTimeMs, Func<T> function)
        {
            int retries = 0;//当前重试次数
            //如果maxRetries小于等于0，那么一直循环
            if (maxRetries <= 0)
            {
                retries = maxRetries - 1;
            }

            while (retries < maxRetries)
            {
                try
                {
                    return function();
                }
                catch (Exception ex)
                {
                    if (sleepTimeMs > 0)//如果sleepTimeMs小于等于0，则不休眠
                    {
                        Thread.Sleep(sleepTimeMs);
                    }

                    if (maxRetries > 0)
                    {
                        retries++;
                    }

                    if (retries >= maxRetries)//如果超过重试次数，那么抛出异常
                    {
                        throw ex;
                    }

                    Debug.WriteLine(ex.Message);
                }
            }
            return default;
        }

        /// <summary>
        /// 执行Action方法，当超时时抛出异常
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="action"></param>
        /// <exception cref="TimeoutException">超时报警</exception>
        public static void ActionWithTimeout(int timeout, Action action)
        {
            var task = Task.Run(action);
            if (task.Wait(timeout))
            {
                return;
            }
            
            throw new TimeoutException("方法执行超时");
        }
    }
}