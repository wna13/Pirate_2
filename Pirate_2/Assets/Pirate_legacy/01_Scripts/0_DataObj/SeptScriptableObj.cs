using UnityEngine;

namespace Sept.Data
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    string path = $"Data/{typeof(T).Name}";
                    instance = Resources.Load<T>(path);
                }

                return instance;
            }
        }
    }
}
