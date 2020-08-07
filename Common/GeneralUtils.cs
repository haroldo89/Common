// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/01/15.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Contiene operaciones para Utilidades generales.
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

using AlbatrosSoft.Common.Models;
using System;
using System.Globalization;
using System.Text;

namespace AlbatrosSoft.Common
{
    /// <summary>
    /// Utilidades generales
    /// </summary>
    public static class GeneralUtils
    {

        private const string DEFAULT_ITEM_VALUE = "0";
        private const string DEFAULT_ITEM_TEXT = "";
        private const int APPLICATION_DATETIME_OFFSET_SETTING_ID = 2;

        /// <summary>
        /// Crear item por defecto
        /// </summary>
        /// <returns></returns>
        public static SelectListItem CreateDefaultItem()
        {
            return new SelectListItem { Text = DEFAULT_ITEM_TEXT, Value = DEFAULT_ITEM_VALUE, Selected = true };
        }

        /// <summary>
        /// Crear item por defecto con texto establecido.
        /// </summary>
        /// <param name="displayText"></param>
        /// <returns></returns>
        public static SelectListItem CreateDefaultItem(string displayText)
        {
            return new SelectListItem { Text = displayText, Value = DEFAULT_ITEM_VALUE, Selected = true };
        }

        /// <summary>
        /// Devuelve un valor entero o un valor nulo si la cadena de caracteres es vacia
        /// </summary>
        /// <param name="numberText">cadena de caracteres</param>
        /// <returns></returns>
        public static int? ToNullableInt32(string numberText)
        {
            int resultValue;
            if (int.TryParse(numberText, out resultValue))
            {
                return resultValue;
            }
            return null;
        }

        /// <summary>
        /// Une los valores de una fecha y hora separados en un solo elemento.
        /// </summary>
        /// <param name="date">Valor de fecha</param>
        /// <param name="hour">Valor de hora</param>
        /// <returns></returns>
        public static DateTime? JoinDateTime(string date, string hour)
        {
            try
            {
                if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(hour))
                {
                    string selectedDate = string.Format(CultureInfo.InvariantCulture, "{0} {1}", date, hour);
                    DateTime queryDate = Convert.ToDateTime(selectedDate, CultureInfo.InvariantCulture);
                    DateTime dtDate = new DateTime(((DateTime)queryDate).Year, ((DateTime)queryDate).Month, ((DateTime)queryDate).Day, ((DateTime)queryDate).Hour, ((DateTime)queryDate).Minute, 0);
                    return dtDate;
                }
                return null;

            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener fecha actual en formato texto.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTimestampString()
        {
            string timestamp = string.Empty;
            timestamp = CommonUtils.GetCurrentDate().ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
            return timestamp;
        }

        /// <summary>
        /// Obtener puntos de cerca apartir de un texto geografico.
        /// </summary>
        /// <param name="geoText"></param>
        /// <returns></returns>
        //public static IEnumerable<LocationPoint> GetGeofencePoints(string geoText)
        //{
        //    NumberStyles numberStyle = NumberStyles.Any;
        //    List<LocationPoint> geofencePoints = new List<LocationPoint>();
        //    const string POINT_SEPARATOR = ",";
        //    const string COORDINATE_SEPARATOR = " ";
        //    if (!string.IsNullOrEmpty(geoText))
        //    {
        //        //Obtener texto con las coordenadas de los puntos
        //        var pointsText = StringUtils.GetStringInBetween("((", "))", geoText);
        //        var points = pointsText.Split(new char[] { Convert.ToChar(POINT_SEPARATOR, CultureInfo.InvariantCulture) }, StringSplitOptions.RemoveEmptyEntries);
        //        if (points.Any())
        //        {
        //            foreach (var pointInfo in points)
        //            {
        //                //Obtener coordenadas del punto
        //                var pointCoordinates = pointInfo.Split(new char[] { Convert.ToChar(COORDINATE_SEPARATOR, CultureInfo.InvariantCulture) }, StringSplitOptions.RemoveEmptyEntries);
        //                if (pointCoordinates.Any())
        //                {
        //                    //Construir punto
        //                    double latitude = 0, longitude = 0;
        //                    var latitudeResult = double.TryParse(pointCoordinates.LastOrDefault(), numberStyle, CultureInfo.InvariantCulture, out latitude);
        //                    var longitudeResult = double.TryParse(pointCoordinates.FirstOrDefault(), numberStyle, CultureInfo.InvariantCulture, out longitude);
        //                    if (latitudeResult && longitudeResult)
        //                    {
        //                        LocationPoint vertex = new LocationPoint(latitude, longitude);
        //                        if (vertex != null)
        //                        {
        //                            //Agregar punto a los vertices de la geocerca
        //                            geofencePoints.Add(vertex);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return geofencePoints;
        //}

        ///// <summary>
        ///// Obtiene el conjunto de puntos que forman una linea que representa una vía.
        ///// </summary>
        ///// <param name = "geoText" > Texto geográfico que representa la vía(LINESTRING).</param>
        ///// <returns>Conjunto de puntos o vertices que forman la vía.</returns>
        //public static IEnumerable<LocationPoint> GetRoutePoints(string geoText)
        //{
        //    NumberStyles numberStyle = NumberStyles.Any;
        //    List<LocationPoint> routePoints = null;
        //    const string POINT_SEPARATOR = ",";
        //    const string COORDINATE_SEPARATOR = " ";
        //    if (!string.IsNullOrEmpty(geoText))
        //    {
        //        //Obtener texto con las coordenadas de los puntos
        //        var pointsText = StringUtils.GetStringInBetween("(", ")", geoText);
        //        var points = pointsText.Split(new char[] { Convert.ToChar(POINT_SEPARATOR, CultureInfo.InvariantCulture) }, StringSplitOptions.RemoveEmptyEntries);
        //        if (points.Any())
        //        {
        //            //Obtener los puntos de la vía.
        //            routePoints = new List<LocationPoint>();
        //            foreach (var pointInfo in points)
        //            {
        //                //Obtener coordenadas del punto
        //                var pointCoordinates = pointInfo.Split(new char[] { Convert.ToChar(COORDINATE_SEPARATOR, CultureInfo.InvariantCulture) }, StringSplitOptions.RemoveEmptyEntries);
        //                if (pointCoordinates.Any())
        //                {
        //                    //Construir punto
        //                    double latitude = 0, longitude = 0;
        //                    var latitudeResult = double.TryParse(pointCoordinates.LastOrDefault(), numberStyle, CultureInfo.InvariantCulture, out latitude);
        //                    var longitudeResult = double.TryParse(pointCoordinates.FirstOrDefault(), numberStyle, CultureInfo.InvariantCulture, out longitude);
        //                    if (latitudeResult && longitudeResult)
        //                    {
        //                        LocationPoint pointLocation = new LocationPoint(latitude, longitude);
        //                        if (pointLocation != null)
        //                        {
        //                            //Agregar punto a la vía.
        //                            routePoints.Add(pointLocation);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return routePoints;
        //}

        /// <summary>
        /// Obtener puntos de cerca apartir de un texto geografico.
        /// </summary>
        /// <param name="locationPointsText"></param>
        /// <returns></returns>
        //public static IEnumerable<LocationPoint> GetLocationPointsFromText(string locationPointsText)
        //{
        //    NumberStyles numberStyle = NumberStyles.Any;
        //    List<LocationPoint> geofencePoints = new List<LocationPoint>();
        //    const string POINT_SEPARATOR = ",";
        //    const string COORDINATE_SEPARATOR = ",";
        //    if (!string.IsNullOrEmpty(locationPointsText))
        //    {
        //        //Obtener texto con las coordenadas de los puntos
        //        var pointsText = locationPointsText.Split(new char[] { Convert.ToChar(POINT_SEPARATOR, CultureInfo.InvariantCulture) }, StringSplitOptions.RemoveEmptyEntries);
        //        if (pointsText.Any())
        //        {
        //            foreach (var pointInfo in pointsText)
        //            {
        //                //Obtener texto con las coordenadas de los puntos
        //                var pointInfoCoordintes = StringUtils.GetStringInBetween(" ", "", pointInfo);
        //                //Obtener coordenadas del punto
        //                var pointCoordinates = pointInfoCoordintes.Split(new char[] { Convert.ToChar(COORDINATE_SEPARATOR, CultureInfo.InvariantCulture) }, StringSplitOptions.RemoveEmptyEntries);
        //                if (pointCoordinates.Any())
        //                {
        //                    //Construir punto
        //                    double latitude = 0, longitude = 0;
        //                    var latitudeResult = double.TryParse(pointCoordinates.LastOrDefault(), numberStyle, CultureInfo.InvariantCulture, out latitude);
        //                    var longitudeResult = double.TryParse(pointCoordinates.FirstOrDefault(), numberStyle, CultureInfo.InvariantCulture, out longitude);
        //                    if (latitudeResult && longitudeResult)
        //                    {
        //                        LocationPoint vertex = new LocationPoint(latitude, longitude);
        //                        if (vertex != null)
        //                        {
        //                            //Agregar punto a los vertices de la geocerca
        //                            geofencePoints.Add(vertex);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return geofencePoints;
        //}

        /// <summary>
        /// Permite obtener la definición de una polilinea de geocerca en formato de texto geográfico de SQL Server Spatial Data a partir de
        /// los vertices que la forman.
        /// </summary>
        /// <param name="polygonPoints">Conjunto de vértices que forman el polígono de la geocerca.</param>
        /// <returns>Texto geográfico que representa a la geocerca.</returns>
        //public static string ToPolylineGeoText(IEnumerable<LocationPoint> polylinePoints)
        //{
        //    string geoText = string.Empty;
        //    const string POINT_SEPARATOR = ",";
        //    if (polylinePoints != null && polylinePoints.Any())
        //    {
        //        StringBuilder sbGeoText = new StringBuilder();
        //        foreach (LocationPoint point in polylinePoints)
        //        {
        //            sbGeoText.AppendFormat("{0} {1}{2} ", point.Longitude.ToString("G", CultureInfo.InvariantCulture), point.Latitude.ToString("G", CultureInfo.InvariantCulture), POINT_SEPARATOR);
        //        }
        //        geoText = string.Format(CultureInfo.InvariantCulture, "LINESTRING({0})", sbGeoText.ToString());
        //        geoText = geoText.Replace(", )", ")");
        //    }
        //    return geoText;
        //}

        ///// <summary>
        ///// Permite obtener la definición de un polígono de geocerca en formato de texto geográfico de SQL Server Spatial Data a partir de
        ///// los vertices que la forman.
        ///// </summary>
        ///// <param name="polygonPoints">Conjunto de vértices que forman el polígono de la geocerca.</param>
        ///// <returns>Texto geográfico que representa a la geocerca.</returns>
        //public static string ToPolygonGeoText(IEnumerable<LocationPoint> polygonPoints)
        //{
        //    string geoText = string.Empty;
        //    const string POINT_SEPARATOR = ",";
        //    if (polygonPoints != null && polygonPoints.Any())
        //    {
        //        StringBuilder sbGeoText = new StringBuilder();
        //        foreach (LocationPoint point in polygonPoints)
        //        {
        //            sbGeoText.AppendFormat("{0} {1}{2} ", point.Longitude.ToString("G", CultureInfo.InvariantCulture), point.Latitude.ToString("G", CultureInfo.InvariantCulture), POINT_SEPARATOR);
        //        }
        //        //Agregar el vertice inicial para cerrar el polígono
        //        var firstVertexPoint = polygonPoints.FirstOrDefault();
        //        sbGeoText.AppendFormat("{0} {1}", firstVertexPoint.Longitude.ToString("G", CultureInfo.InvariantCulture), firstVertexPoint.Latitude.ToString("G", CultureInfo.InvariantCulture));
        //        geoText = string.Format(CultureInfo.InvariantCulture, "POLYGON(({0}))", sbGeoText.ToString());
        //    }
        //    return geoText;
        //}

        /// <summary>
        /// Generar string de titulo para la grafica svg
        /// </summary>
        /// <param name="chartTitle">Titulo</param>
        /// <returns></returns>
        public static string GetChartTitleSvgElement(string chartTitle)
        {
            string titleElementText = string.Empty;
            StringBuilder sbChartTitleElement = new StringBuilder();
            //Etiqueta
            sbChartTitleElement.Append("<text ");
            //coordenada x
            sbChartTitleElement.AppendFormat("x={0}", StringUtils.InQuotes("150"));
            //coordenada y
            sbChartTitleElement.AppendFormat(" y={0}", StringUtils.InQuotes("20"));
            //Tipo de fuente
            sbChartTitleElement.AppendFormat(" font-family={0}", StringUtils.InQuotes("Calibri"));
            sbChartTitleElement.AppendFormat(" font-weight={0}", StringUtils.InQuotes("bold"));
            //Tamaño de la fuente
            sbChartTitleElement.AppendFormat(" font-size={0}", StringUtils.InQuotes("15"));
            //NameSpace
            sbChartTitleElement.AppendFormat(" xmlns={0}", StringUtils.InQuotes("http://www.w3.org/2000/svg"));
            //Cerrar etiqueta
            sbChartTitleElement.AppendFormat(" >{0}</text>", chartTitle);
            titleElementText = sbChartTitleElement.ToString();
            return titleElementText;
        }

        /// <summary>
        /// Obtener el valor de tiempo de  offset de la aplicacion por usuario.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        //public static int GetApplicationDateTimeOffset(string userName)
        //{
        //    int applicationDatetimeOffset = -5;
        //    AppUserBll AppUserBll = new AppUserBll();
        //    IEnumerable<ViewAppUserSetting> userSettings = AppUserBll.GetUserSettings(userName);
        //    if (userSettings != null && userSettings.Any())
        //    {
        //        var userSettingsValue = userSettings.FirstOrDefault(u => u.SettingId.Equals(APPLICATION_DATETIME_OFFSET_SETTING_ID));
        //        if (userSettingsValue != null)
        //        {
        //            applicationDatetimeOffset = Convert.ToInt32(userSettingsValue.SettingValue, CultureInfo.InvariantCulture);
        //        }
        //    }
        //    return applicationDatetimeOffset;
        //}

    }
}
