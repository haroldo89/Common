// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo.
// Fecha de Creación		:   2014/01/15.
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Contiene operaciones para trabajar con expresiones de ordenamiento de datos para colecciones.
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AlbatrosSoft.Common.Web
{
    /// <summary>
    /// Contiene operaciones para trabajar con expresiones de ordenamiento de datos para colecciones.
    /// </summary>
    /// <typeparam name="T">Tipo de dato concreto de los elementos de la colección.</typeparam>
    public sealed class SortingHelper<T>
    {
        /// <summary>
        /// Dirección ascendente.
        /// </summary>
        private const string ASCENDING_DIRECTION = "ASC";

        /// <summary>
        /// Dirección descendente.
        /// </summary>
        private const string DESCENDING_DIRECTION = "DESC";

        /// <summary>
        /// Permite realizar el ordenamiento de los elementos de una colección de acuerdo a la expresión de ordenamiento y la dirección indicada (ASC, DESC).
        /// </summary>
        /// <param name="dataSource">Colección de elementos a ordenar.</param>
        /// <param name="sortExpressionString">Expresión o campo de ordenamiento.</param>
        /// <param name="sortDirection">Dirección del ordenamiento (ASC, DESC).</param>
        /// <returns>Colección de elementos ordenada de acuerdo a los criterios especificados.</returns>
        public static IEnumerable<T> SortBy(IEnumerable<T> dataSource, string sortExpressionString, string sortDirection)
        {
            IEnumerable<T> sortedData = null;
            if (dataSource != null)
            {
                var sortExpression = GetExpression<T>(sortExpressionString);
                switch (sortDirection)
                {
                    case ASCENDING_DIRECTION:
                        sortedData = dataSource.OrderBy(sortExpression);
                        break;
                    case DESCENDING_DIRECTION:
                        sortedData = dataSource.OrderByDescending(sortExpression);
                        break;
                }
            }
            return sortedData;
        }

        /// <summary>
        /// Obtiene la expresión de predicado que corresponde a un criterio o campo de ordenamiento.
        /// </summary>
        /// <typeparam name="TParam">Tipo concreto de los elementos de la colección a ordenar.</typeparam>
        /// <param name="sortExpressionString">Expresión o campo de ordenamiento.</param>
        /// <returns>Expresión de predicado de ordenamiento.</returns>
        private static Func<TParam, object> GetExpression<TParam>(string sortExpressionString)
        {
            var itemParameter = Expression.Parameter(typeof(TParam));
            var expressionBody = Expression.Convert(Expression.Property(itemParameter, sortExpressionString), typeof(object));
            var sortExpression = Expression.Lambda<Func<TParam, object>>(expressionBody, itemParameter);
            return sortExpression.Compile();
        }
    }
}


