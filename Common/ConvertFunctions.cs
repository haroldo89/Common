// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/08/22.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Contiene funciones para conversiones.
//             
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
    /// Funciones de conversion.
    /// </summary>
    public static class ConvertFunctions
    {
        /// <summary>
        /// Deveolver vacio si es nulo
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ReturnEmptyIfNull(object value)
        {
            string objectValue = string.Empty;
            if (value != null)
            {
                return value;
            }
            return objectValue;
        }

        /// <summary>
        /// Devolver cero si es nulo
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ReturnZeroIfNull(object value)
        {
            int objectValue = 0;
            if (value != null)
            {
                return value;
            }
            return objectValue;
        }

        /// <summary>
        /// Devolver fecha minima si es nulo
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ReturnDateTimeMinIfNull(object value)
        {
            DateTime objectValue = DateTime.MinValue;
            if (value != null)
            {
                return value;
            }
            return objectValue;
        }
    }
}
