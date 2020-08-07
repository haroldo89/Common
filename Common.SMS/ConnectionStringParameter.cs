using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.SMS
{
    /// <summary>
    /// Agrupa la información de los parámetros de conexión a un servicio de envío de mensajes a través de SMS.
    /// </summary>
    public static class ConnectionStringParameter
    {
        /// <summary>
        /// Nombre de la cuenta del proveedor de SMS.
        /// </summary>
        public static readonly string AccountName = "AccountName";

        /// <summary>
        /// Clave de la cuenta del proveedor de SMS.
        /// </summary>
        public static readonly string Password = "Password";

        /// <summary>
        /// Nombre del proveedor de SMS.
        /// </summary>
        public static readonly string ProviderName = "ProviderName";

        /// <summary>
        /// Devuelve la lista de parámetros de conexión al servicio de envío de mensajes de SMS.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> AsEnumerable()
        {
            IEnumerable<string> connectionParameters = new List<string>() 
            { 
                ConnectionStringParameter.AccountName,
                ConnectionStringParameter.Password,
                ConnectionStringParameter.ProviderName
            };
            return connectionParameters;
        }
    }
}
