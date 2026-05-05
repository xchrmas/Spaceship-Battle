using UnityEngine;

namespace SpaceshipBattle.Services
{
    /// <summary>
    /// JSON serialization to PlayerPrefs
    /// </summary>
    public class StorageService : IStorageService
    {
        public void Save<T>(string key, T obj)
        {
            if (string.IsNullOrEmpty(key) || obj == null) return;

            PlayerPrefs.SetString(key, JsonUtility.ToJson(obj));
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Returns default if the key doesn't exist yet.
        /// </summary>
        public T Load<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) return default;
            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
        }
    }
}