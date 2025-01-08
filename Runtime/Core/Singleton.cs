namespace Utilities
{
    public abstract class Singleton<T> where T : new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (object.Equals(_instance, default(T)))
                {
                    _instance = new T();
                }

                return _instance;
            }
        }

        protected Singleton() { }
    }
}