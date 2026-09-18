//------------------------------------------------------------------------------
// <copyright file="Repository.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace Ucu.Poo.Repositories
{
    /// <summary>
    /// Esta clase representa un catálogo de películas.
    /// </summary>
    public class Repository<T> : IRepository<T> where T : ISpecificValue
    {
        private List<T> items = new List<T>();

        /// <summary>
        /// Obtiene la lista de T en la base de datos.
        /// </summary>
        public ReadOnlyCollection<T> Items
        {
            get { return this.items.AsReadOnly(); }
        }

        public void Add(T item)
        {
            if (item != null)
            {
                this.items.Add(item);
            }
        }
        public void Remove(T item)
        {
            this.items.Remove(item);
        }

        /// <summary>
        /// Busca un item que cumpla con un criterio
        /// específico.
        /// </summary>
        /// <param name="field">El nombre del atributo.</param>
        /// <param name="value">El valor del atributo.</param>
        /// <returns>El item encontrado que cumple el criterio especificado
        /// o null si no se encuentra ningun item.</returns>
        public T Find(string field, string value)
        {
            foreach (T item in this.items)
            {
                if (item.HasValue(field, value))
                {
                    return item;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Convierte el almacenamiento de los items a una representación en formato
        /// JSON.
        /// </summary>
        /// <returns>Una representación del almacenamiento de los items en formato
        /// JSON.</returns>
        public string ConvertToJson()
        {
            return JsonSerializer.Serialize(this.items);
        }

        /// <summary>
        /// Carga el almacenamiento de los items desde una representación en formato
        /// JSON.
        /// </summary>
        /// <param name="content">La representación en formato JSON desde la
        /// cual cargar el almacenamiento.</param>
        public void LoadFromJson(string content)
        {
            List<T> items = JsonSerializer.Deserialize<List<T>>(content);
            if (items != null)
            {
                this.items = items;
            }
            else
            {
                this.items = new List<T>();
            }
        }

        /// <summary>
        /// Guarda el almacenamiento de los items en un archivo en formato JSON.
        /// </summary>
        /// <param name="filePath">El nombre del archivo, incluyendo
        /// opcionalmente la ruta.</param>
        public void SaveToFile(string filePath)
        {
            string content = this.ConvertToJson();
            File.WriteAllText(filePath, content);
        }

        /// <summary>
        /// Carga el almacenamiento de los items desde un archivo en formato JSON.
        /// </summary>
        /// <param name="filePath">El nombre del archivo, incluyendo
        /// opcionalmente la ruta.</param>
        /// <returns>Retorna <c>true</c> si se cargó la base de datos y
        /// <c>false</c> en caso contrario.</returns>
        public bool LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                this.LoadFromJson(content);
                return true;
            }

            return false;
        }
    }
}
