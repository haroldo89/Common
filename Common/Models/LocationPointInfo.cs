// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2016/03/31.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Representacion de un punto en un mapa.
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

namespace Common.Models
{
    /// <summary>
    /// Representacion de un punto en el mapa.
    /// </summary>
    public class LocationPointInfo
    {
        /// <summary>
        /// Latitud
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitud
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Constructor que instancia un nuevo objeto de LocationPointInfo
        /// </summary>
        public LocationPointInfo()
        {

        }

        /// <summary>
        /// Constructor que recibe latitud y longitud para crear un nuevo 
        /// objeto de LocationPointInfo
        /// </summary>
        /// <param name="latitude">Latitud</param>
        /// <param name="longitude">Longitud</param>
        public LocationPointInfo(double longitude, double latitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
