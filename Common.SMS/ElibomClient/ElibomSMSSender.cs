using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.SMS.ElibomClient
{
    /// <summary>
    /// Expone la funcionalidad necesaria para realizar el envio de mensajes de texto a través de los 
    /// servicios proporcionados por el proveedor Elibom.
    /// </summary>
    public class ElibomSMSSender : SMSSender
    {
        /// <summary>
        /// Código de respuesta de envío de mensaje correcto.
        /// </summary>
        public readonly int SMS_RESULT_OK_CODE = 0;

        //TODO: Agregar referencia a servicio web.
        //private SendMessageWS _SMSServiceClient;

        //private SendMessageWS SMSServiceClient
        //{
        //    get
        //    {
        //        if (this._SMSServiceClient == null)
        //        {
        //            this._SMSServiceClient = new SendMessageWSClient();
        //        }
        //        return this._SMSServiceClient;
        //    }
        //}

        internal ElibomSMSSender(string accountName, string password)
            : base(accountName, password)
        {

        }

        /// <summary>
        /// Realiza el envío de un mensaje SMS a el(los) destinatario(s) indicados.
        /// </summary>
        /// <param name="recipientNumber">Número de el(los) destinatario(s) (seperados por ',')</param>
        /// <param name="message">Texto del mensaje a enviar.</param>
        /// <param name="resultMessage">Mensaje con el resultado de la operación.</param>
        /// <returns>
        /// 0- Éxito: el mensaje fue creado con éxito. 
        /// 1 - Credenciales Inválidas. Email o contraseña inválidos. 
        /// 2 - Parámetros incompletos o inválidos. 
        /// -1 - Error desconocido. 
        /// </returns>
        public override int Send(string recipientNumber, string message, out string resultMessage)
        {
            int result = -1;
            resultMessage = string.Empty;
            try
            {
                //result = this.SMSServiceClient.sendMessage(this.ServiceAccountName, this.ServiceAccountPassword, recipientNumber, message);
            }
            catch (Exception exc)
            {
                resultMessage = exc.Message;
            }
            return result;

        }

        /// <summary>
        /// Determina si un mensaje de texto fue enviado exitosamente a partir del código de respuesta 
        /// de envío del mensaje.
        /// </summary>
        /// <param name="responseCode">Código de respuesta de envío del mensaje.</param>
        /// <returns>true, si el mensaje fue enviado exitosamente, de lo contrario false.</returns>
        public override bool MessageSentSuccessfully(int responseCode)
        {
            return responseCode.Equals(this.SMS_RESULT_OK_CODE);
        }
    }
}
