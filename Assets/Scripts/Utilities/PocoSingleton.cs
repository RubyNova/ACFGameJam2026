using UnityEngine;

namespace Utilities
{
    public class PocoSingleton<T> where T : new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new();
                }

                return _instance;
            }
        }
    }
}