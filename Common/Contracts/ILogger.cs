// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/08/15.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Expone la funcionalidad necesaria para realizar instrumentación de eventos y registro
//                  de errores de una aplicación.
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================
using System;

namespace AlbatrosSoft.Common.Contracts
{
    /// <summary>
    /// Expone la funcionalidad necesaria para realizar instrumentación de eventos y registro
    /// de errores de una aplicación.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Permite escribir una entrada de registro de evento de aplicación.
        /// </summary>
        /// <param name="message">Mensaje con el detalle del evento.</param>
        /// <param name="category">Categoría del evento.</param>
        void WriteLog(string message, string category);

        /// <summary>
        /// Permite escribir una entrada de registro de error de aplicación.
        /// </summary>
        /// <param name="errorMessage">Mensaje con el detalle del error.</param>
        /// <param name="category">Categoría del error.</param>
        /// <param name="severiryLevel">Nivel de severidad del error.</param>
        void WriteErrorLog(string errorMessage, string category, int severiryLevel);

        /// <summary>
        /// Permite escribir una entrada de registro de error de aplicación.
        /// </summary>
        /// <param name="errorMessage">Mensaje con el detalle del error.</param>
        /// <param name="category">Categoría del error.</param>
        /// <param name="severiryLevel">Nivel de severidad del error.</param>
        /// <param name="innerException">Instancia del error interno generado.</param>
        /// <param name="includeErrorDetail">Indica si se incluye el detalle de error en el contenido del mensaje de error.</param>
        void WriteErrorLog(string errorMessage, string category, int severiryLevel, Exception innerException, bool includeErrorDetail);
    }
}
