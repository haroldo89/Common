using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.SMS.ElibomClient
{
    /// <summary>
    /// Inicializador de instancias del servicio de envio de SMS del proveedor Elibom.
    /// </summary>
    public class ElibomFactory : SMSProviderFactory
    {
        /// <summary>
        /// Construye una instancia del servicio de envio de SMS del proveedor Elibom.
        /// </summary>
        /// <param name="accountName">Nombre de cuenta de usuario del servicio de envio de SMS.</param>
        /// <param name="password">Clave de la cuenta de suscripción.</param>
        /// <returns>Instancia de la clase de servicio especifica del proveedor Elibom para el envio de SMS.</returns>
        public override SMSSender CreateSender(string accountName, string password)
        {
            SMSSender sender = new ElibomSMSSender(accountName, password);
            return sender;
        }
    }
}
