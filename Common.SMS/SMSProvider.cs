using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.SMS
{
    /// <summary>
    /// Fachada que expone los proveedores disponibles para el servicio de envío de mensajes SMS.
    /// </summary>
    public static class SMSProvider
    {
        //const string ELIBOM_PROVIDER = "Elibom";
        //const string SMARTD_PROVIDER = "SmartD";

        ///// <summary>
        ///// Agrupa los proveedores soportados para el envío de mensajes SMS.
        ///// </summary>
        //private static IDictionary<string, Func<SMSProviderFactory>> _SMSProviderFactories;

        //static SMSProvider()
        //{
        //    LoadFactories();
        //}

        ///// <summary>
        ///// Establece los proveedores soportados para el envío de mensajes SMS.
        ///// </summary>
        //private static void LoadFactories()
        //{
        //    _SMSProviderFactories = new Dictionary<string, Func<SMSProviderFactory>>()
        //    {
        //        {ELIBOM_PROVIDER, () => new ElibomFactory()},
        //        {SMARTD_PROVIDER, () => new SmartDFactory()}
        //    };
        //}

        ///// <summary>
        ///// Devuelve la instancia del proveedor encargado de inicializar el servicio de envío de mensajes SMS
        ///// que corresponde al nombre del proveedor especificado.
        ///// </summary>
        ///// <param name="providerName">Nombre del proveedor de envío de SMS.</param>
        ///// <returns>Proveedor encargado de inicializar el servicio de envío de mensajes SMS para el nombre del proveedor especificado.</returns>
        //public static SMSProviderFactory GetFactory(string providerName)
        //{
        //    SMSProviderFactory factory = null;
        //    if (string.IsNullOrEmpty(providerName))
        //    {
        //        throw new ArgumentException("Nombre de proveedor no puede ser un valor nulo.", "providerName");
        //    }
        //    Func<SMSProviderFactory> factoryCreator;
        //    if (_SMSProviderFactories.TryGetValue(providerName, out factoryCreator))
        //    {
        //        factory = factoryCreator();
        //    }
        //    else
        //    {
        //        throw new NotImplementedException("Tipo de formato de entrada no soportado.");
        //    }
        //    return factory;
        //}
    }
}
