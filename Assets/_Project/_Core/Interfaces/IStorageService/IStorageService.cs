
namespace SpaceshipBattle.Services
{
    /// <summary>
    ///     Provides basic save / load functionality.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        ///     Saves an object to the device.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        void Save<T>(string key, T obj);


        /// <summary>
        ///     Loads an object from the device.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        T Load<T>(string key);
    }
}