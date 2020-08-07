// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo.
// Fecha de Creación		:   2014/11/21.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   

// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Contiene una serie de utilidades para realizar conversion de unidades de medida
//                  entre diferentes magnitudes.
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
    /// Contiene una serie de utilidades para realizar conversion de unidades de medida
    /// entre diferentes magnitudes.
    /// </summary>
    public static class UnitConverter
    {
        /// <summary>
        /// Radio de la tierra (km)
        /// </summary>
        public const double EARTH_RADIUS_IN_KILOMETERS = 6367.0;

        /// <summary>
        /// Radio de la tierra (mi)
        /// </summary>
        public const double EARTH_RADIUS_IN_MILES = 3956.0;

        /// <summary>
        /// Convierte un valor dado de grados a radianes.
        /// </summary>
        /// <param name="degreesVaule">Valor a convertir en grados.</param>
        /// <returns>Valor equivalente en radianes.</returns>
        public static double ToRadian(double degreesVaule)
        {
            return degreesVaule * (Math.PI / 180);
        }

        /// <summary>
        /// Convierte un valor dado de radianes a grados.
        /// </summary>
        /// <param name="radiansValue">Valor a convertir en radianes.</param>
        /// <returns>Valor equivalente en grados.</returns>
        public static double ToDegrees(double radiansValue)
        {
            return radiansValue * (180 / Math.PI);
        }

        /// <summary>
        /// Convierte a Kilómetros una cantidad dada en la unidad de medida especificada.
        /// </summary>
        /// <param name="value">Cantidad a convertir</param>
        /// <param name="sourceUnit">Unidad de medida de al cantidad dada.</param>
        /// <returns>Valor equivalente en Kilómetros.</returns>
        public static double ToKilometers(double value, DistanceUnit sourceUnit)
        {
            double conversionValue = default(double);
            switch (sourceUnit)
            {
                case DistanceUnit.Meters:
                    conversionValue = value / 1000;
                    break;
                case DistanceUnit.Miles:
                    conversionValue = value * 1.609344;
                    break;
                case DistanceUnit.Knots:
                    conversionValue = value * 1.852;
                    break;
                default:
                    conversionValue = value;
                    break;
            }
            return conversionValue;
        }
    }
}
