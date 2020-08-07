using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.SMS
{
    /// <summary>
    /// Agrupa servicios para el análisis y lectura de información de cadenas de conexión a servicios de envío de mensajería a través de SMS.
    /// </summary>
    public static class ConnectionStringHelper
    {
        /// <summary>
        /// Separador de campo en cadena de conexión al servicio de envío de mensajes SMS.
        /// </summary>
        public const char CONNECTION_STRING_PARAMETER_FIELD_SEPARATOR = ';';

        /// <summary>
        /// Separador de valor de parámetro en cadena de conexión al servicio de envío de mensajes SMS.
        /// </summary>
        public const char CONNECTION_STRING_PARAMETER_VALUE_SEPARATOR = '=';

        /// <summary>
        /// Devuelve el nombre del proveedor contenido en la cadena de conexión al servicio de envío de mensajes SMS.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión a evaluar.</param>
        /// <returns>Nombre del proveedor del servicio de envío de mensajes SMS.</returns>
        public static string GetProviderName(string connectionString)
        {
            string providerName = ConnectionStringHelper.GetParameter(ConnectionStringParameter.ProviderName, connectionString);
            return providerName;
        }

        /// <summary>
        /// Devuelve el valor del parámetro especificado dentro de la cadena de conexión al servicio de envío de mensajes SMS.
        /// </summary>
        /// <param name="parameterName">Nombre del parámetro.</param>
        /// <param name="connectionString">Cadena de conexión a evaluar.</param>
        /// <returns>Valor del parámetro.</returns>
        public static string GetParameter(string parameterName, string connectionString)
        {
            string parameterValue = string.Empty;
            if (ConnectionStringHelper.IsValidConnectionString(connectionString))
            {
                IDictionary<string, string> connectionParameters = ConnectionStringHelper.GetConnectionParameters(connectionString);
                parameterValue = connectionParameters[parameterName];
            }
            return parameterValue;
        }

        /// <summary>
        /// Determina si una cadena de conexión al servicio de envío de mensajes SMS es válida.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión a validar.</param>
        /// <returns>true, si la cadena de conexión es válida, de lo contrario false.</returns>
        public static bool IsValidConnectionString(string connectionString)
        {
            bool isValid = true;
            //La cadena no es vacía
            isValid &= !string.IsNullOrEmpty(connectionString);
            //La cadena contiene todos los parámetros de conexión
            isValid &= ConnectionStringParameter.AsEnumerable().All(p => connectionString.Contains(p));
            //La cadena contiene el separador de campo
            isValid &= connectionString.Contains(ConnectionStringHelper.CONNECTION_STRING_PARAMETER_FIELD_SEPARATOR);
            //La cadena contiene el separador de valor de parámetro
            isValid &= connectionString.Contains(ConnectionStringHelper.CONNECTION_STRING_PARAMETER_VALUE_SEPARATOR);
            return isValid;
        }

        /// <summary>
        /// Devuelve un listado de clave/valor con la información de acceso a un proveedor de mensajería SMS.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión al servicio de envío de SMS.</param>
        /// <returns>Listado con la información de acceso al proveedor de mensajería.</returns>
        public static IDictionary<string, string> GetConnectionParameters(string connectionString)
        {
            IDictionary<string, string> connectionParameters = new Dictionary<string, string>();
            var connectionStringParameters = connectionString.Split(ConnectionStringHelper.CONNECTION_STRING_PARAMETER_FIELD_SEPARATOR);
            connectionParameters = (from p in connectionStringParameters
                                    select new
                                    {
                                        Key = p.Split(ConnectionStringHelper.CONNECTION_STRING_PARAMETER_VALUE_SEPARATOR).First(),
                                        Value = p.Split(ConnectionStringHelper.CONNECTION_STRING_PARAMETER_VALUE_SEPARATOR).Last()
                                    }).ToDictionary(p => p.Key, p => p.Value);
            return connectionParameters;
        }
    }
}
