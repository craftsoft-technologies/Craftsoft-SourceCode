using System;

namespace APP.Web.Common.Utilities
{
    public abstract class AppDataCacheUtility<T>
    {
        private readonly object syncRoot = new object();
        private T newCache;

        public T Cache
        {
            get { return newCache; }
        }

        #region IAppDataUtility Members
        public string Name { get; set; }
        public DateTime? NextRefreshTime { get; set; }
        public TimeSpan? RefreshInterval { get; set; }

        public void Initialize()
        {
            lock (syncRoot)
            {
                newCache = LoadFromServer();
                if (RefreshInterval != null)
                {
                    NextRefreshTime = DateTime.Now.Add(RefreshInterval.Value);
                }
            }
        }

        #endregion

        public abstract T LoadFromServer();

        protected AppDataCacheUtility()
        {
            Name = GetType().Name;
        }

        public T Get()
        {
            return newCache;
        }
    }
}
