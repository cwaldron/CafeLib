using System;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Core.IoC
{
    public interface IPropertyService
    {
        /// <summary>
        /// Get property from property bag
        /// </summary>
        /// <typeparam name="T">property type</typeparam>
        /// <returns>property value</returns>
        T GetProperty<T>();

        /// <summary>
        /// Set a property in the property bag
        /// </summary>
        /// <typeparam name="T">property type</typeparam>
        /// <param name="value">property value</param>
        void SetProperty<T>(T value);

        /// <summary>
        /// Remove entry from property bag keyed by its type.
        /// </summary>
        /// <typeparam name="T">property type</typeparam>
        /// <returns>true if the property has been removed; otherwise false</returns>
        bool RemoveProperty<T>();

        /// <summary>
        /// Get property from property bag based on its key.
        /// </summary>
        /// <typeparam name="T">property type</typeparam>
        /// <param name="key">property key</param>
        /// <returns>property value</returns>
        T GetProperty<T>(string key);

        /// <summary>
        /// Set a property in the property bag using a key.
        /// </summary>
        /// <typeparam name="T">property type</typeparam>
        /// <param name="key">property key</param>
        /// <param name="value">property value</param>
        void SetProperty<T>(string key, T value);

        /// <summary>
        /// Remove entry from property bag based on its key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if the property has been removed; otherwise false</returns>
        bool RemoveProperty(string key);

        /// <summary>
        /// Get property from property bag based on its key.
        /// </summary>
        /// <typeparam name="T">property type</typeparam>
        /// <param name="guid">property guid</param>
        /// <returns>property value</returns>
        T GetProperty<T>(Guid guid);

        /// <summary>
        /// Set a property in the property bag using a guid.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guid">property guid</param>
        /// <param name="value">property value</param>
        void SetProperty<T>(Guid guid, T value);

        /// <summary>
        /// Remove entry from property bag keyed by a guid.
        /// </summary>
        /// <param name="guid">property guid</param>
        /// <returns>true if the property has been removed; otherwise false</returns>
        bool RemoveProperty(Guid guid);

        /// <summary>
        /// Convert the property bag to an object.
        /// </summary>
        T ToObject<T>();
    }
}
