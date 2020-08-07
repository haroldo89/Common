// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/01/15.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Contiene operaciones para trabajar con expresiones de filtrado de datos para colecciones.
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

using System;

namespace AlbatrosSoft.Common
{
    /// <summary>
    /// Contiene operaciones para trabajar con expresiones de filtrado de datos para colecciones.
    /// </summary>
    /// <typeparam name="T">Tipo de dato concreto de los elementos de la colección.</typeparam>
    public class FilterHelper<T> where T : new()
    {
        /// <summary>
        /// Permite agregar un criterio de búsqueda de datos a una expresion de filtrado existente.
        /// </summary>
        /// <param name="currentFilterExpression">Expresion lambda que representa una condicion de filtrado.</param>
        /// <param name="newFilterExpression">Expresion lambda que representa un criterio de filtrado.</param>
        /// <returns>Expresion lambda resultante con los criterios de filtrado.</returns>
        public Func<T, bool> AddFilterExpression(Func<T, bool> currentFilterExpression, Func<T, bool> newFilterExpression)
        {
            if (currentFilterExpression == null)
            {
                currentFilterExpression = t => true;
            }
            return t => currentFilterExpression(t) && newFilterExpression(t);
        }
    }
}
