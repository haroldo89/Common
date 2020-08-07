// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo.
// Fecha de Creación		:   2014/12/29.
// Producto o sistema	    :   Wizenz.Common
// Empresa			        :   Wizenz Technologies
// Proyecto			        :   Wizenz.Common

// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Establece los formatos de salida soportados por los servicios de Geocoding.
//             
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
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.SpatialData.Geocoding
{
    /// <summary>
    /// Establece los formatos de salida soportados por los servicios de Geocoding.
    /// </summary>
    public struct ResponseOutputFormat
    {
        /// <summary>
        /// Formato de salida en notación Xml.
        /// </summary>
        public static readonly string Xml = "xml";

        /// <summary>
        /// Formato de salida en notación Json.
        /// </summary>
        public static readonly string Json = "json";
    }
}
