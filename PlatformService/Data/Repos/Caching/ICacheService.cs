namespace PlatformService.Data.Repos.Caching
{
     public interface ICacheService
	 { 
	    public T GetData<T>(string key);
		public bool SetData<T>(string key, T value, TimeSpan expirationTime);       
		public object RemoveData(string key);
		public bool UpdateCacheIfExists<T>(string key, T value, TimeSpan expirationTime);


	 }
}
