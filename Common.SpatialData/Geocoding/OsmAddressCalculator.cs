// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo.
// Fecha de Creación		:   2014/12/29.
// Producto o sistema	    :   Wizenz.Common
// Empresa			        :   Wizenz Technologies
// Proyecto			        :   Wizenz.Common

// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Expone la funcionalidad necesaria para determinar el valor de una dirección utilizando
//                  los servicios de Open Street Maps.
//                  
//                  La implementación fue realizada teniendo en cuenta la especificación definida en:
//                  http://wiki.openstreetmap.org/wiki/Nominatim#Reverse_Geocoding_.2F_Address_lookup
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

using AlbatrosSoft.Common.SpatialData.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;

namespace AlbatrosSoft.Common.SpatialData.Geocoding
{
    /// <summary>
    /// Expone la funcionalidad necesaria para determinar el valor de una dirección utilizando los servicios de Open Street Maps.
    /// </summary>
    public class OsmAddressCalculator : IAddressCalculator
    {
        #region Constantes
        /// <summary>
        /// Dirección base del servicio
        /// </summary>
        public const string OSM_SERVICE_ROOT_URL = "http://nominatim.openstreetmap.org/reverse";

        /// <summary>
        /// Tipo de formato de salida por defecto
        /// </summary>
        public readonly string DEFAULT_OUTPUT_FORMAT = ResponseOutputFormat.Xml;

        /// <summary>
        /// Valor por defecto de dirección
        /// </summary>
        private readonly string DEFAULT_ADDRESS_VALUE = string.Empty;
        #endregion Constantes

        #region Propiedades
        /// <summary>
        /// Llave de autenticación del servicio Bing Maps
        /// </summary>
        public string MapServiceKey { get; set; }

        /// <summary>
        /// Formato de salida de la respuesta de la solicitud.
        /// </summary>
        public string OutputFormat { get; set; }
        #endregion Propiedades

        #region Métodos
        /// <summary>
        /// Inicializa una nueva instancia de la clase OsmAddressCalculator para el cálculo de direcciones a través de los servicios de Open Street Maps.
        /// </summary>
        public OsmAddressCalculator()
            : this(ResponseOutputFormat.Xml)
        {

        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase OsmAddressCalculator para el cálculo de direcciones a través de los servicios de Open Street Maps.
        /// </summary>
        /// <param name="outputFormat">Formato de salida de la respuesta de las solicitudes realizadas a los servicios de Open Street Maps.</param>
        public OsmAddressCalculator(string outputFormat)
        {
            this.OutputFormat = DEFAULT_OUTPUT_FORMAT;
            if (!string.IsNullOrEmpty(outputFormat))
            {
                this.OutputFormat = outputFormat;
            }
        }

        /// <summary>
        /// Construye la Url de solicitud para invocar el servicio de Geocoding de Open Street Maps.
        /// </summary>
        /// <param name="latitude">Valor de latitud.</param>
        /// <param name="longitude">Valor de longitud.</param>
        /// <returns>Url de solicitud del servicio.</returns>
        private string GetRequestUrl(double latitude, double longitude)
        {
            string requestUrl = string.Empty;
            string requestUrlTemplate = string.Concat(OSM_SERVICE_ROOT_URL, "/?{0}");
            IDictionary<string, string> requestParameters = new Dictionary<string, string>();
            //Tipo de formato de salida
            requestParameters["format"] = this.OutputFormat;
            //Coordenadas
            requestParameters["lat"] = StringUtils.ToDoubleString(latitude);
            requestParameters["lon"] = StringUtils.ToDoubleString(longitude);
            //Nivel de zoom
            requestParameters["zoom"] = "18";
            //Incluir detalles de dirección
            requestParameters["addressdetails"] = "1";
            var queryString = string.Join("&", requestParameters.Select(p => string.Format("{0}={1}", p.Key, p.Value)));
            requestUrl = string.Format(requestUrlTemplate, queryString);
            return requestUrl;
        }

        #region Miembros de IAddressCalculator
        /// <summary>
        /// Determina el valor de una dirección a partir de un par de coordenadas de latitud y longitud.
        /// </summary>
        /// <param name="latitude">Valor de latitud.</param>
        /// <param name="longitude">Valor de longitud.</param>
        /// <returns>Valor de dirección correspondiente a las coordenadas especificadas.</returns>
        public string ComputeAddress(float latitude, float longitude)
        {
            string resultAddress = string.Empty;
            if (latitude != default(float) && longitude != default(float))
            {
                resultAddress = this.ComputeAddress(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
            }
            return resultAddress;
        }

        /// <summary>
        /// Determina el valor de una dirección a partir de un par de coordenadas de latitud y longitud en formato flotante de doble precisión.
        /// </summary>
        /// <param name="latitude">Valor de latitud.</param>
        /// <param name="longitude">Valor de longitud.</param>
        /// <returns>Valor de dirección correspondiente a las coordenadas especificadas.</returns>
        public string ComputeAddress(double latitude, double longitude)
        {
            string resultAddress = string.Empty;
            if (latitude != default(float) && longitude != default(float))
            {
                try
                {
                    //Armar url de la solicitud
                    string requestUrl = this.GetRequestUrl(latitude, longitude);
                    //Realizar solicitud al servicio
                    var addressRequest = WebRequest.Create(requestUrl);
                    //Obtener respuesta del servicio
                    using (WebResponse response = addressRequest.GetResponse())
                    {
                        if (response != null)
                        {
                            //Obtener información de la respuesta del servicio
                            var responseInfo = (response as HttpWebResponse).GetResponseStream();
                            if (responseInfo != null)
                            {
                                ////Cargar información de la respuesta
                                XmlDocument xmlResponse = new XmlDocument();
                                xmlResponse.Load(responseInfo);
                                //Leer dirección
                                XmlNode addressNode = xmlResponse.SelectSingleNode("//result");
                                resultAddress = addressNode.InnerText;
                            }
                        }
                    }
                }
                catch
                {
                    resultAddress = DEFAULT_ADDRESS_VALUE;
                }
            }
            return resultAddress;
        }
        #endregion Miembros de IAddressCalculator

        #endregion Métodos
    }
}
