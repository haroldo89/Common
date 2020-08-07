using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.Mail
{
    /// <summary>
    /// Agrupa la información de los parámetros de conexión a un servicio de envío de mensajes a través de correo electrónico.
    /// </summary>
    public static class ConnectionStringParameter
    {
        /// <summary>
        /// Nombre de la cuenta de correo de origen.
        /// </summary>
        public static readonly string AccountName = "AccountName";

        /// <summary>
        /// Clave de la cuenta de correo de origen.
        /// </summary>
        public static readonly string Password = "Password";

        /// <summary>
        /// Dirección del servidor de correo saliente (SMTP).
        /// </summary>
        public static readonly string SmtpHost = "SmtpHost";

        /// <summary>
        /// Puerto de salida del servidor de correo SMTP.
        /// </summary>
        public static readonly string SmtpPort = "SmtpPort";

        /// <summary>
        /// Devuelve la lista de parámetros de conexión al servicio de envío de mensajes de correo electrónico.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> AsEnumerable()
        {
            IEnumerable<string> connectionParameters = new List<string>() 
            { 
                ConnectionStringParameter.AccountName,
                ConnectionStringParameter.Password,
                ConnectionStringParameter.SmtpHost,
                ConnectionStringParameter.SmtpPort
            };
            return connectionParameters;
        }
    }
}
