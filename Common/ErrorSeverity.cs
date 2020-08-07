// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/08/19.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Agrupa y establece los niveles de severidad de error de una aplicación.
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

namespace AlbatrosSoft.Common
{
    /// <summary>
    /// Agrupa y establece los niveles de severidad de error de una aplicación.
    /// </summary>
    public struct ErrorSeverity
    {
        /// <summary>
        /// Nivel de gravedad de error menor.
        /// </summary>
        public static readonly int LOW = 1;

        /// <summary>
        /// Nivel de gravedad de error intermedio.
        /// </summary>
        public static readonly int MEDIUM = 2;

        /// <summary>
        /// Nivel de gravedad de error alto.
        /// </summary>
        public static readonly int HIGH = 3;

        /// <summary>
        /// Nivel de gravedad de error mayor.
        /// </summary>
        public static readonly int CRITICAL = 4;
    }
}
