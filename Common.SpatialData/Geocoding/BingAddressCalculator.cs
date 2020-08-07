// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo.
// Fecha de Creación		:   2014/12/29.
// Producto o sistema	    :   Wizenz.Common
// Empresa			        :   Wizenz Technologies
// Proyecto			        :   Wizenz.Common

// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Expone la funcionalidad necesaria para determinar el valor de una dirección utilizando
//                  los servicios de Bing Maps.
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
    /// Expone la funcionalidad necesaria para determinar el valor de una dirección utilizando los servicios de Bing Maps.
    /// </summary>
    public sealed class BingAddressCalculator : IAddressCalculator
    {
        #region Constantes
        /// <summary>
        /// Dirección base del servicio
        /// </summary>
        public const string BING_SERVICE_ROOT_URL = "http://dev.virtualearth.net/REST/v1/Locations";

        /// <summary>
        /// Tipo de formato de salida por defecto
        /// </summary>
        public static readonly string DEFAULT_OUTPUT_FORMAT = ResponseOutputFormat.Xml;

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
        /// Inicializa una nueva instancia de la clase BingAddressCalculator con la información de acceso a los servicios de Bing Maps.
        /// </summary>
        /// <param name="mapServiceKey">Llave de autenticación del servicio Bing Maps.</param>
        public BingAddressCalculator(string mapServiceKey)
            : this(mapServiceKey, DEFAULT_OUTPUT_FORMAT)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase BingAddressCalculator con la información de acceso a los servicios de Bing Maps.
        /// </summary>
        /// <param name="mapServiceKey">Llave de autenticación del servicio Bing Maps.</param>
        /// <param name="outputFormat">Formato de salida de la respuesta de las solicitudes realizadas a los servicios de Bing Maps.</param>
        public BingAddressCalculator(string mapServiceKey, string outputFormat)
        {
            if (string.IsNullOrEmpty(mapServiceKey))
            {
                throw new ArgumentException("Clave del servicio de mapa no puede ser un valor nulo.", "mapServiceKey");
            }
            this.MapServiceKey = mapServiceKey;
            this.OutputFormat = DEFAULT_OUTPUT_FORMAT;
            if (!string.IsNullOrEmpty(outputFormat))
            {
                this.OutputFormat = outputFormat;
            }
        }

        /// <summary>
        /// Construye la Url de solicitud para invocar el servicio de Geocoding de Bing Maps.
        /// </summary>
        /// <param name="latitude">Valor de latitud.</param>
        /// <param name="longitude">Valor de longitud.</param>
        /// <returns>Url de solicitud del servicio.</returns>
        private string GetRequestUrl(double latitude, double longitude)
        {
            string requestUrl = string.Empty;
            string requestUrlTemplate = string.Concat(BING_SERVICE_ROOT_URL, "/{0}?{1}");
            string locationParameter = string.Format("{0},{1}", StringUtils.ToDoubleString(latitude), StringUtils.ToDoubleString(longitude));
            IDictionary<string, string> requestParameters = new Dictionary<string, string>();
            //Tipo de formato de salida
            requestParameters["o"] = this.OutputFormat;
            //Llave de autenticacion del servicio del mapa
            requestParameters["key"] = this.MapServiceKey;
            var queryString = string.Join("&", requestParameters.Select(p => string.Format("{0}={1}", p.Key, p.Value)));
            requestUrl = string.Format(requestUrlTemplate, locationParameter, queryString);
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
            //Coordenadas son válidas
            if (latitude != default(double) && longitude != default(double))
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
                                if (this.OutputFormat.Equals(ResponseOutputFormat.Xml))
                                {
                                    //Cargar información de la respuesta
                                    XmlDocument xmlResponse = new XmlDocument();
                                    xmlResponse.Load(responseInfo);
                                    //Leer dirección
                                    XmlNamespaceManager xmlManager = new XmlNamespaceManager(xmlResponse.NameTable);
                                    xmlManager.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");
                                    XmlNodeList locationElements = xmlResponse.SelectNodes("//rest:Location", xmlManager);
                                    XmlNode locationNode = xmlResponse.SelectSingleNode("//rest:Location", xmlManager);
                                    if (locationNode != null)
                                    {
                                        XmlNode addressNode = locationNode.SelectSingleNode(".//rest:FormattedAddress", xmlManager);
                                        if (addressNode != null)
                                        {
                                            resultAddress = addressNode.InnerText;
                                        }
                                        ////Obtener pais
                                        //XmlNode countryNode = locationNode.SelectSingleNode(".//rest:CountryRegion", xmlManager);
                                        ////Obtener departamento
                                        //XmlNode provinceNode = locationNode.SelectSingleNode(".//rest:AdminDistrict", xmlManager);
                                        ////Obtener ciudad
                                        //XmlNode cityNode = locationNode.SelectSingleNode(".//rest:Locality", xmlManager);
                                        //StringBuilder sbResult = new StringBuilder();
                                        //sbResult.AppendLine("----Direccion:");
                                        //sbResult.AppendLine(addressNode.InnerText);
                                        //sbResult.AppendLine();
                                        //sbResult.AppendLine("----Pais:");
                                        //sbResult.AppendLine(countryNode.InnerText);
                                        //sbResult.AppendLine("----Departamento:");
                                        //sbResult.AppendLine(provinceNode.InnerText);
                                        //sbResult.AppendLine("----Ciudad:");
                                        //sbResult.AppendLine(cityNode.InnerText);
                                        //resultAddress = sbResult.ToString(); 
                                    }
                                }
                                //TODO: Implementar lectura de respuesta en formato Json
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
