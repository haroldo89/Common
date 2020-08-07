using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.Mail
{
    /// <summary>
    /// Contiene la funcionalidad necesaria para realizar el envio de mensajes de correo electrónico a través
    /// de una cuenta asociada a un servidor SMTP.
    /// </summary>
    public class MailSender : IDisposable
    {
        #region Definición de Constantes
        /// <summary>
        /// Valor de puerto por defecto
        /// </summary>
        private const int DEFAULT_PORT = 25;

        /// <summary>
        /// Separador de campo en cadena de conexión al servicio de envío de mensajes de correo.
        /// </summary>
        private const char CONNECTION_STRING_PARAMETER_FIELD_SEPARATOR = ';';

        /// <summary>
        /// Separador de valor de parámetro en cadena de conexión al servicio de envío de mensajes de correo.
        /// </summary>
        private const char CONNECTION_STRING_PARAMETER_VALUE_SEPARATOR = '=';
        #endregion Definición de Constantes

        #region Propiedades
        /// <summary>
        /// Dirección del servidor de salida SMTP.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Puerto del servidor de salida SMTP.
        /// </summary>
        public int HostPort { get; set; }

        /// <summary>
        /// Indicador de habilitación de envío de mensajes a través de SSL.
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Credenciales de conexión al servidor SMTP.
        /// </summary>
        private NetworkCredential AccountCredentials { get; set; }

        private SmtpClient _SendingClient;

        /// <summary>
        /// Instancia del cliente de envío de correo.
        /// </summary>
        public SmtpClient SendingClient
        {
            get
            {
                if (this._SendingClient == null)
                {
                    this._SendingClient = new SmtpClient()
                    {
                        Host = this.Host,
                        Port = this.HostPort,
                        EnableSsl = this.EnableSsl,
                        UseDefaultCredentials = false
                    };
                }
                return this._SendingClient;
            }
        }
        #endregion Propiedades

        public MailSender(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Cadena de conexión no puede ser un valor nulo.", "connectionString");
            }
            if (!this.IsValidConnectionString(connectionString))
            {
                throw new Exception("La cadena de conexión al servicio de envío de correo es inválida o posee información incompleta.");
            }
            var connectionParameters = this.GetConnectionParameters(connectionString);
            if (connectionParameters.Any())
            {
                this.LoadSmtpParameters(connectionParameters);
            }

        }

        private void LoadSmtpParameters(IEnumerable<string> connectionParameters)
        {
            IDictionary<string, string> smtpParameters = new Dictionary<string, string>();
            try
            {
                //Cargar información de parámetros
                smtpParameters = (from p in connectionParameters
                                  select new
                                  {
                                      Key = p.Split(CONNECTION_STRING_PARAMETER_VALUE_SEPARATOR).First(),
                                      Value = p.Split(CONNECTION_STRING_PARAMETER_VALUE_SEPARATOR).Last()
                                  }).ToDictionary(p => p.Key, p => p.Value);
                //Establecer valor de parámetros
                this.Host = smtpParameters[ConnectionStringParameter.SmtpHost];
                this.HostPort = Convert.ToInt32(smtpParameters[ConnectionStringParameter.SmtpPort], CultureInfo.InvariantCulture);
                var senderUserAccount = smtpParameters[ConnectionStringParameter.AccountName];
                var senderPassword = smtpParameters[ConnectionStringParameter.Password];
                this.AccountCredentials = new NetworkCredential(senderUserAccount, senderPassword);

            }
            catch (Exception exc)
            {
                throw new InvalidOperationException("Ha ocurrido un error al cargar los parámetros de conexión al cliente SMTP.", exc);
            }
        }

        private IEnumerable<string> GetConnectionParameters(string connectionString)
        {
            IEnumerable<string> connectionParameters = Enumerable.Empty<string>();
            connectionParameters = connectionString.Split(CONNECTION_STRING_PARAMETER_FIELD_SEPARATOR);
            return connectionParameters;
        }

        /// <summary>
        /// Inicializa una nueva instancia del servicio de envío de mensajes a través de correo electrónico.
        /// </summary>
        /// <param name="host">Dirección del servidor de salida SMTP.</param>
        /// <param name="senderUserAccount">Dirección de la cuenta de correo de origen.</param>
        /// <param name="senderPassword">Clave de la cuenta de correo de origen.</param>
        public MailSender(string host, string senderUserAccount, string senderPassword)
            : this(host, DEFAULT_PORT, senderUserAccount, senderPassword, false)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia del servicio de envío de mensajes a través de correo electrónico.
        /// </summary>
        /// <param name="host">Dirección del servidor de salida SMTP.</param>
        /// <param name="port">Puerto de salida del servidor de correo SMTP.</param>
        /// <param name="senderUserAccount">Dirección de la cuenta de correo de origen.</param>
        /// <param name="senderPassword">Clave de la cuenta de correo de origen.</param>
        public MailSender(string host, int port, string senderUserAccount, string senderPassword)
            : this(host, port, senderUserAccount, senderPassword, false)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia del servicio de envío de mensajes a través de correo electrónico.
        /// </summary>
        /// <param name="host">Dirección del servidor de salida SMTP.</param>
        /// <param name="port">Puerto de salida del servidor de correo SMTP.</param>
        /// <param name="senderUserAccount">Dirección de la cuenta de correo de origen.</param>
        /// <param name="senderPassword">Clave de la cuenta de correo de origen.</param>
        /// <param name="enableSsl">Indicador de habilitación de envío de mensajes a través de SSL.</param>
        public MailSender(string host, int port, string senderUserAccount, string senderPassword, bool enableSsl)
        {
            this.Host = host;
            this.HostPort = port;
            this.EnableSsl = enableSsl;
            this.AccountCredentials = new NetworkCredential(senderUserAccount, senderPassword);
        }

        #region Métodos
        /// <summary>
        /// Ejecuta el envío de un mensaje de correo electrónico a un destinatario. 
        /// </summary>
        /// <param name="fromDirection">Dirección de correo del remitente del mensaje.</param>
        /// <param name="recipient">Dirección de correo del destinatario del mensaje.</param>
        /// <param name="subject">>Asunto del mensaje.</param>
        /// <param name="body">Contenido o cuerpo del mensaje.</param>
        /// <param name="isBodyHtml">Indicador de contenido HTML.</param>
        /// <param name="errorMessage">Mensaje de resultado de la operación (Salida).</param>
        public void Send(string fromDirection, string recipient, string subject, string body, bool isBodyHtml, out string errorMessage)
        {
            this.Send(fromDirection, new string[] { recipient }, subject, body, isBodyHtml, out errorMessage);
        }

        /// <summary>
        /// Ejecuta el envío de un mensaje de correo electrónico a un destinatario con un archivo adjunto.
        /// </summary>
        /// <param name="fromDirection">Dirección de correo del remitente del mensaje.</param>
        /// <param name="recipient">Dirección de correo del destinatario del mensaje.</param>
        /// <param name="subject">>Asunto del mensaje.</param>
        /// <param name="body">Contenido o cuerpo del mensaje.</param>
        /// <param name="isBodyHtml">Indicador de contenido HTML.</param>
        /// <param name="attachmentStream">Referencia al flujo de datos del archivo adjunto.</param>
        /// <param name="errorMessage">Mensaje de resultado de la operación (Salida).</param>
        public void Send(string fromDirection, string recipient, string subject, string body, bool isBodyHtml, FileStream attachmentStream, out string errorMessage)
        {
            this.Send(fromDirection, new string[] { recipient }, subject, body, isBodyHtml, attachmentStream, out errorMessage);
        }

        /// <summary>
        /// Ejecuta el envío de un mensaje de correo electrónico a un grupo de destinatarios.
        /// </summary>
        /// <param name="fromDirection">Dirección de correo del remitente del mensaje.</param>
        /// <param name="recipients">Direcciones de correo de los destinatarios.</param>
        /// <param name="subject">Asunto del mensaje.</param>
        /// <param name="body">Contenido o cuerpo del mensaje.</param>
        /// <param name="isBodyHtml">Indicador de contenido HTML.</param>
        /// <param name="errorMessage">Mensaje de resultado de la operación (Salida).</param>
        public void Send(string fromDirection, string[] recipients, string subject, string body, bool isBodyHtml, out string errorMessage)
        {
            this.Send(fromDirection, recipients, subject, body, isBodyHtml, null, out errorMessage);
        }

        /// <summary>
        /// Ejecuta el envío de un mensaje de correo electrónico a un grupo de destinatarios con un archivo adjunto.
        /// </summary>
        /// <param name="fromDirection">Dirección de correo del remitente del mensaje.</param>
        /// <param name="recipients">Direcciones de correo de los destinatarios.</param>
        /// <param name="subject">Asunto del mensaje.</param>
        /// <param name="body">Contenido o cuerpo del mensaje.</param>
        /// <param name="isBodyHtml">Indicador de contenido HTML.</param>
        /// <param name="attachmentStream">Referencia al flujo de datos del archivo adjunto.</param>
        /// <param name="errorMessage">Mensaje de resultado de la operación (Salida).</param>
        public void Send(string fromDirection, string[] recipients, string subject, string body, bool isBodyHtml, FileStream attachmentStream, out string errorMessage)
        {
            MailMessage messageToSend = new MailMessage()
            {
                From = new MailAddress(fromDirection),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml,
                Priority = MailPriority.High
            };
            if (recipients != null)
            {
                recipients.ToList().ForEach(r => messageToSend.To.Add(r));
            }
            if (attachmentStream != null)
            {
                messageToSend.Attachments.Add(new Attachment(attachmentStream, Path.GetFileName(attachmentStream.Name)));
            }
            this.SendMailMessage(messageToSend, out errorMessage);
        }

        /// <summary>
        /// Realiza el envio de un mensaje de correo electrónico a partir de su representacion como objeto MailMessage.
        /// </summary>
        /// <param name="message">Instancia del objeto que representa al mensaje de envio</param>
        /// <param name="errorMessage">Mensaje de resultado de la operación.</param>
        private void SendMailMessage(MailMessage message, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (this.IsValidMessage(message))
            {
                try
                {
                    using (message)
                    {
                        this.SendingClient.Credentials = this.AccountCredentials;
                        this.SendingClient.Send(message);
                    }
                }
                catch (SmtpFailedRecipientsException exc)
                {
                    errorMessage = CommonUtils.GetErrorDetail(exc, "Error en la recepción del mensaje");
                }
                catch (SmtpException exc)
                {
                    errorMessage = CommonUtils.GetErrorDetail(exc, "Error en la operación de envio del mensaje");
                }
                catch (InvalidOperationException exc)
                {
                    errorMessage = CommonUtils.GetErrorDetail(exc, "Error causado por operación inválida");
                }
                catch (Exception exc)
                {
                    errorMessage = CommonUtils.GetErrorDetail(exc, "Error no identificado");
                }
            }
        }

        /// <summary>
        /// Establece si un mensaje de correo preparado para enviar es válido.
        /// </summary>
        /// <param name="message">Mensaje de correo a validar.</param>
        /// <returns>true, si el mensaje esta completo y bien formado, de lo contrario false.</returns>
        private bool IsValidMessage(MailMessage message)
        {
            bool validMessage = message != null;
            if (validMessage)
            {
                validMessage &= !string.IsNullOrEmpty(message.From.Address);
                validMessage &= StringUtils.IsValidEmail(message.From.Address);
                validMessage &= message.To.AsEnumerable().All(m => !string.IsNullOrEmpty(m.Address));
                validMessage &= message.To.AsEnumerable().All(m => StringUtils.IsValidEmail(m.Address));
                if (message.Attachments.Count > 0)
                {
                    validMessage &= message.Attachments.AsEnumerable().All(a => a.ContentStream != null);
                }
            }
            return validMessage;
        }

        /// <summary>
        /// Determina si una cadena de conexión al servicio de envío de mensajes de correo electrónico es válida.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión a validar.</param>
        /// <returns>true, si la cadena de conexión es válida, de lo contrario false.</returns>
        private bool IsValidConnectionString(string connectionString)
        {
            bool isValid = true;
            //La cadena no es vacía
            isValid &= !string.IsNullOrEmpty(connectionString);
            //La cadena contiene todos los parámetros de conexión
            isValid &= ConnectionStringParameter.AsEnumerable().All(p => connectionString.Contains(p));
            //La cadena contiene el separador de campo
            isValid &= connectionString.Contains(CONNECTION_STRING_PARAMETER_FIELD_SEPARATOR);
            //La cadena contiene el separador de valor de parámetro
            isValid &= connectionString.Contains(CONNECTION_STRING_PARAMETER_VALUE_SEPARATOR);
            return isValid;
        }
        #endregion Métodos

        #region Miembros de IDisposable
        /// <summary>
        /// Releases all resources used by an instance of the <see cref="MailSender" /> class.
        /// </summary>
        /// <remarks>
        /// This method calls the virtual <see cref="Dispose(bool)" /> method, passing in true, 
        /// and then suppresses 
        /// finalization of the instance.
        /// </remarks>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged resources before an instance of the <see cref="MailSender" /> 
        /// class is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// NOTE: Leave out the finalizer altogether if this class doesn't 
        /// own unmanaged resources itself, but leave the other methods
        /// exactly as they are.
        /// This method releases unmanaged resources by calling the virtual 
        /// <see cref="Dispose(bool)" /> method, passing in false.
        /// </remarks>
        ~MailSender()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by an instance of the 
        /// <see cref="MailSender" /> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources;        
        /// false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._SendingClient != null)
                {
                    this._SendingClient.Dispose();
                    this._SendingClient = null;
                }
            }
        }
        #endregion Miembros de IDisposable
    }
}
