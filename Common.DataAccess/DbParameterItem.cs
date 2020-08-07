// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/01/15.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Representa un parámetro de entrada o salida utilizado por una función o procedimiento almacenado 
//                  alojado en una base de datos.
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

using System.Data;

namespace AlbatrosSoft.Common.DataAccess
{
    /// <summary>
    /// Representa un parámetro de entrada o salida utilizado por una función o procedimiento almacenado 
    /// alojado en una base de datos.
    /// </summary>
    public class DbParameterItem
    {
        #region Propiedades
        /// <summary>
        /// Nombre
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Tipo
        /// </summary>
        public DbType Type { get; set; }
        /// <summary>
        /// Direccion de parametro
        /// </summary>
        public ParameterDirection Direction { get; set; }
        /// <summary>
        /// Valor
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Tamaño.
        /// </summary>
        public int Size { get; set; }
        #endregion
    }
}
