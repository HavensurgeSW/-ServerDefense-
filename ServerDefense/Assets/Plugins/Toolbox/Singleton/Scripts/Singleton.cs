using UnityEngine;

namespace ServerDefense.Tools.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (!AlreadyInitialized())
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    DontDestroyOnLoad(instance.gameObject);
                }

                return instance;
            }
        }

        public static bool AlreadyInitialized()
        {
            return instance != null;
        }
    }
}