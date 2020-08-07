using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.SMS
{
    /// <summary>
    /// Representa un proveedor de servicio de envío de mensajes de Texto.
    /// </summary>
    public abstract class SMSSender
    {
        protected string ServiceAccountName { get; set; }
        protected string ServiceAccountPassword { get; set; }

        public SMSSender(string accountName, string password)
        {
            this.ServiceAccountName = accountName;
            this.ServiceAccountPassword = password;
        }

        /// <summary>
        /// Realiza el envío de un mensaje SMS a el(los) destinatario(s) indicados.
        /// </summary>
        /// <param name="recipientNumber">Número de el(los) destinatario(s) (seperados por ',')</param>
        /// <param name="message">Texto del mensaje a enviar.</param>
        /// <param name="resultMessage">Mensaje con el resultado de la operación.</param>
        /// <returns>Código de respuesta de la operación de envío del mensaje.</returns>
        public abstract int Send(string recipientNumber, string message, out string resultMessage);

        /// <summary>
        /// Determina si un mensaje de texto fue enviado exitosamente a partir del código de respuesta 
        /// de envío del mensaje.
        /// </summary>
        /// <param name="responseCode">Código de respuesta de envío del mensaje.</param>
        /// <returns>true, si el mensaje fue enviado exitosamente, de lo contrario false.</returns>
        public abstract bool MessageSentSuccessfully(int responseCode);

        public virtual int Send(string[] recipients, string message, out string resultMessage)
        {
            throw new NotImplementedException("Operación no soportada por este proveedor.");
        }
    }
}
