// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo.
// Fecha de Creación		:   2014/03/07.
// Producto o sistema	    :   Common.Web
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Contiene la funcionalidad necesaria para serializar y deserializar 
//                  objetos en formato Json.
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace AlbatrosSoft.Common.Web
{
    /// <summary>
    /// Contiene la funcionalidad necesaria para serializar y deserializar objetos en formato Json.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Obtiene la representación en formato JSON de la información de un objeto.
        /// </summary>
        /// <typeparam name="T">Tipo concreto del objeto a serializar.</typeparam>
        /// <param name="element">Instancia de objeto a serializar.</param>
        /// <returns>Cadena en formato JSON que representa al objeto serializado.</returns>
        public static string Serialize<T>(T element)
        {
            DataContractJsonSerializer dataToSerialize = new DataContractJsonSerializer(typeof(T));
            string objectName = string.Empty;
            var elementType = GetElementType(element);
            var attr = Attribute.GetCustomAttributes(elementType).First(a => a is DataContractAttribute);
            if (attr != null)
            {
                objectName = string.Format(CultureInfo.InvariantCulture, "{0}{1}:", "{", StringUtils.InQuotes((attr as DataContractAttribute).Name));
            }
            string jsonString = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                dataToSerialize.WriteObject(stream, element);
                jsonString = Encoding.UTF8.GetString(stream.ToArray());
            }
            //Replace Json Date String                                         
            string p = @"\\/Date\((\d+)\+\d+\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            string jSonDataToSend = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", objectName, jsonString, "}");
            return jSonDataToSend;
        }

        /// <summary>
        /// Reconstruye un objeto desde una cadena en formato JSON.
        /// </summary>
        /// <typeparam name="T">Tipo concreto del objeto a deserializar.</typeparam>
        /// <param name="jsonText">Cadena en formato JSON que representa al objeto serializado.</param>
        /// <returns>Instancia del objeto deserializado.</returns>
        public static T Deserialize<T>(string jsonText)
        {
            //Convierte "yyyy-MM-dd HH:mm:ss" String a "\/Date(1319266795390+0800)\/"
            string dateRegex = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(dateRegex);
            jsonText = reg.Replace(jsonText, matchEvaluator);
            DataContractJsonSerializer dataToDeserilize = new DataContractJsonSerializer(typeof(T));
            T obj = default(T);
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonText)))
            {
                obj = (T)dataToDeserilize.ReadObject(stream);
            }
            return obj;
        }

        /// <summary>
        /// Convierte fecha de string Serializado
        /// </summary>
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime date = new DateTime(1970, 1, 1);
            date = date.AddMilliseconds(long.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture));
            date = date.ToLocalTime();
            result = date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            return result;
        }

        /// <summary>
        /// Convierte fecha formato string a formato fecha JSon
        /// </summary>
        private static string ConvertDateStringToJsonDate(Match match)
        {
            string result = string.Empty;
            DateTime date = DateTime.Parse(match.Groups[0].Value, CultureInfo.InvariantCulture);
            date = date.ToUniversalTime();
            TimeSpan timeSpan = date - DateTime.Parse("1970-01-01", CultureInfo.InvariantCulture);
            result = string.Format(CultureInfo.InvariantCulture, "\\/Date({0}+0800)\\/", timeSpan.TotalMilliseconds);
            return result;
        }

        private static Type GetElementType<T>(T element)
        {
            Type elementType = typeof(T);
            if (IsEnumerableType(elementType))
            {
                var genericArgs = elementType.GetGenericArguments();
                if (genericArgs.Any())
                {
                    elementType = genericArgs.First();
                }
            }
            return elementType;
        }

        private static bool IsEnumerableType(Type type)
        {
            return type.FullName.Contains("System.Collections");
        }
    }
}
