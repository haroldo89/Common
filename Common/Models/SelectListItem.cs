// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/08/22.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Representacion de un item de un listado para una lista desplegable.
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

namespace AlbatrosSoft.Common.Models
{
    /// <summary>
    /// Representacion de un item de un listado para una lista desplegable.
    /// </summary>
    public class SelectListItem
    {
        /// <summary>
        /// Texto
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Valor
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Item seleccionado
        /// </summary>
        public bool Selected { get; set; }
    }
}
