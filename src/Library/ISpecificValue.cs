//------------------------------------------------------------------------------
// <copyright file="ISpecificValue.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//------------------------------------------------------------------------------

namespace Ucu.Poo.Repositories
{
    public interface ISpecificValue
    {
        bool HasValue(string field, string value);
    }
}