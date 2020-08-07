using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.SMS.SmartDClient
{
    /// <summary>
    /// Expone la funcionalidad necesaria para realizar el envio de mensajes de texto a través de los 
    /// servicios proporcionados por el proveedor SmartD.
    /// </summary>
    public class SmartDSMSSender : SMSSender
    {
        //TODO: Agregar referencia de servicio web
        //private SMSWSSClient _SMSServiceClient;

        //public SMSWSSClient SMSServiceClient
        //{
        //    get
        //    {
        //        if (this._SMSServiceClient == null)
        //        {
        //            this._SMSServiceClient = new SMSWSSClient();
        //        }
        //        return this._SMSServiceClient;
        //    }
        //}

        internal SmartDSMSSender(string accountName, string password)
            : base(accountName, password)
        {
            //this.SMSServiceClient.ClientCredentials.UserName.UserName = accountName;
            //this.SMSServiceClient.ClientCredentials.UserName.Password = password;
        }

        public override int Send(string recipientNumber, string message, out string resultMessage)
        {
            int result = 0;
            resultMessage = string.Empty;
            try
            {
                //long sendOperation = this.SMSServiceClient.AbrirEnvio(string.Concat("Alertas Wizenz ", DateTime.UtcNow.AddHours(-5).ToString()));
                //result = Convert.ToInt32(this.SMSServiceClient.EnviarMensaje(message, long.Parse(recipientNumber), sendOperation));
            }
            catch (Exception exc)
            {
                resultMessage = exc.Message;
            }
            return result;
        }

        public override int Send(string[] recipients, string message, out string resultMessage)
        {
            return base.Send(recipients, message, out resultMessage);
        }

        /// <summary>
        /// Determina si un mensaje de texto fue enviado exitosamente a partir del código de respuesta 
        /// de envío del mensaje.
        /// </summary>
        /// <param name="responseCode">Código de respuesta de envío del mensaje.</param>
        /// <returns>true, si el mensaje fue enviado exitosamente, de lo contrario false.</returns>
        public override bool MessageSentSuccessfully(int responseCode)
        {
            return responseCode > 0;
        }
    }
}
