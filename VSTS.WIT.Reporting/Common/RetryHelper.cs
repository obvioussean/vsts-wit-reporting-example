using System;
using System.Threading.Tasks;

namespace VSTS.WIT.Reporting.Common
{
    public static class RetryHelper
    {
        public static async Task<T> Retry<T>(Func<Task<T>> function, int retryCount)
        {
            Exception exception = null;
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    return await function();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            throw new Exception("Retry count exhausted.", exception);
        }
    }
}
