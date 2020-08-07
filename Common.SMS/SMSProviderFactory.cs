using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.SMS
{
    /// <summary>
    /// Proporciona el mecanismo base para crear un cliente de servicio para el envío de mensajes de texto.
    /// </summary>
    public abstract class SMSProviderFactory
    {
        /// <summary>
        /// Devuelve una instancia de un proveedor específico que presta servicios de envio de mensajes de texto.
        /// </summary>
        /// <param name="accountName">Nombre de cuenta de usuario del servicio de envio de SMS.</param>
        /// <param name="password">Clave de la cuenta de suscripción.</param>
        /// <returns></returns>
        public abstract SMSSender CreateSender(string accountName, string password);

        /// <summary>
        ///  Devuelve una instancia de un proveedor específico que presta servicios de envio de mensajes de texto.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión al servicio de envío de mensajes de texto.</param>
        /// <returns></returns>
        public virtual SMSSender CreateSender(string connectionString)
        {
            SMSSender sender = null;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Cadena de conexión no puede ser un valor nulo.", "connectionString");
            }
            if (!this.IsValidConnectionString(connectionString))
            {
                throw new Exception("La cadena de conexión al servicio de envío de mensajes SMS es inválida o posee información incompleta.");
            }
            try
            {
                var connectionParameters = this.GetConnectionParameters(connectionString);
                if (connectionParameters.Any())
                {
                    string accountName = connectionParameters[ConnectionStringParameter.AccountName];
                    string password = connectionParameters[ConnectionStringParameter.Password];
                    sender = this.CreateSender(accountName, password);
                }
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException("Ha ocurrido un error al cargar los parámetros de conexión al servicio de envío de SMS.", exc);
            }
            return sender;
        }

        /// <summary>
        /// Determina si una cadena de conexión al servicio de envío de mensajes SMS es válida.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión a validar.</param>
        /// <returns>true, si la cadena de conexión es válida, de lo contrario false.</returns>
        protected virtual bool IsValidConnectionString(string connectionString)
        {
            return ConnectionStringHelper.IsValidConnectionString(connectionString);
        }

        /// <summary>
        /// Devuelve un listado de clave/valor con la información de acceso a un proveedor de mensajería SMS.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión al servicio de envío de SMS.</param>
        /// <returns>Listado con la información de acceso al proveedor de mensajería.</returns>
        protected virtual IDictionary<string, string> GetConnectionParameters(string connectionString)
        {
            IDictionary<string, string> connectionParameters = ConnectionStringHelper.GetConnectionParameters(connectionString);
            return connectionParameters;
        }
    }
}
