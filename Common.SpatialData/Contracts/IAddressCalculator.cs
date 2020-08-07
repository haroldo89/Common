// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo.
// Fecha de Creación		:   2014/12/29.
// Producto o sistema	    :   Wizenz.Common
// Empresa			        :   Wizenz Technologies
// Proyecto			        :   Wizenz.Common

// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Establece el comportamiento base de un servicio de cálculo de direcciones.
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

namespace AlbatrosSoft.Common.SpatialData.Contracts
{
    /// <summary>
    /// Establece el comportamiento base de un servicio de cálculo de direcciones.
    /// </summary>
    public interface IAddressCalculator
    {
        /// <summary>
        /// Formato de salida del resultado devuelto por el servicio de Geocoding.
        /// </summary>
        string OutputFormat { get; set; }

        /// <summary>
        /// Determina el valor de una dirección a partir de un par de coordenadas de latitud y longitud.
        /// </summary>
        /// <param name="latitude">Valor de latitud.</param>
        /// <param name="longitude">Valor de longitud.</param>
        /// <returns>Valor de dirección correspondiente a las coordenadas especificadas.</returns>
        string ComputeAddress(float latitude, float longitude);

        /// <summary>
        /// Determina el valor de una dirección a partir de un par de coordenadas de latitud y longitud en formato flotante de doble precisión.
        /// </summary>
        /// <param name="latitude">Valor de latitud.</param>
        /// <param name="longitude">Valor de longitud.</param>
        /// <returns>Valor de dirección correspondiente a las coordenadas especificadas.</returns>
        string ComputeAddress(double latitude, double longitude);
    }
}
