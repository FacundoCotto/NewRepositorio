//------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------


using System.Collections.ObjectModel;

namespace Ucu.Poo.Repositories
{
    public interface IRepository<T>
    {
        ReadOnlyCollection<T> Items { get; }
        void Add(T item);
        void Remove(T item);
        T Find(string field, string value);
        string ConvertToJson();
        void LoadFromJson(string content);
        void SaveToFile(string filePath);
        bool LoadFromFile(string filePath);

    }
}