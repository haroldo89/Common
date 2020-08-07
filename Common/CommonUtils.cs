// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/08/22.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Contiene funcionalidades de utilidad de proposito general para las diferentes 
//                  aplicaciones
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AlbatrosSoft.Common
{
    /// <summary>
    /// Utilidades comunes
    /// </summary>
    public static class CommonUtils
    {
        /// <summary>
        /// Devuelve la versión del ensamblado principal del sistema que se encuentra en ejecución.
        /// </summary>
        /// <returns>Version del ensamblado ejecutable del sistema.</returns>
        public static string GetAppVersion()
        {
            var appVersion = "1.0.0.0";
            var executingAssembly = Assembly.GetCallingAssembly();
            if (executingAssembly != null)
            {
                appVersion = CommonUtils.GetAssemblyVersion(executingAssembly);
            }
            return appVersion;
        }

        /// <summary>
        /// /// Devuelve la versión del ensamblado en donde se encuentra definido el tipo especificado.
        /// </summary>
        /// <param name="innerType">Tipo a evaluar.</param>
        /// <returns>Version del ensamblado del tipo indicado.</returns>
        public static string GetAssemblyVersion(Type innerType)
        {
            var assemblyVersion = string.Empty;
            if (innerType == null)
            {
                throw new ArgumentNullException("Tipo interno del ensamblado no puede ser un valor nulo.", "innerType");
            }
            var containerAssembly = innerType.Assembly;
            if (containerAssembly != null)
            {
                assemblyVersion = CommonUtils.GetAssemblyVersion(containerAssembly);
            }
            return assemblyVersion;
        }

        /// <summary>
        /// Devuelve la versión de un ensamblado especificado.
        /// </summary>
        /// <param name="assembly">Instancia del ensamblado a evaluar.</param>
        /// <returns>Versión del ensamblado.</returns>
        public static string GetAssemblyVersion(Assembly assembly)
        {
            var assemblyVersion = string.Empty;
            if (assembly == null)
            {
                throw new ArgumentNullException("Ensamblado no puede ser un valor nulo.", "assembly");
            }
            var assemblyName = new AssemblyName(assembly.FullName);
            assemblyVersion = assemblyName.Version.ToString();
            return assemblyVersion;
        }

        /// <summary>
        /// Obtiene la fecha actual del sistema local.
        /// </summary>
        /// <returns>Fecha actual del sistema local.</returns>
        public static DateTime GetCurrentDate()
        {
            var HOURS_TO_SUBSTRACT = -5;
            return GetLocalDate(HOURS_TO_SUBSTRACT);
        }

        /// <summary>
        /// Obtiene la fecha actual del sistema local.
        /// </summary>
        /// <param name="timeOffset">Cantidad de horas a adicionar o restar a la fecha/hora UTC.</param>
        /// <returns>Fecha actual del sistema local.</returns>
        public static DateTime GetLocalDate(double timeOffset)
        {
            return DateTime.UtcNow.AddHours(timeOffset);
        }

        /// <summary>
        /// Devuelve la fecha especificada sin el componente de hora.
        /// </summary>
        /// <param name="source">Fecha a truncar</param>
        /// <returns>Valor de fecha sin el componente hora.</returns>
        public static DateTime TruncDate(DateTime source)
        {
            return new DateTime(source.Year, source.Month, source.Day, 0, 0, 0);
        }

        /// <summary>
        /// Obtiene el detalle de error de una excepcion.
        /// </summary>
        /// <param name="exc">Instancia de la excepcion.</param>
        /// <returns>Detalle de error de la excepcion.</returns>
        public static string GetErrorDetail(Exception exc)
        {
            return GetErrorDetail(exc, false);
        }

        /// <summary>
        /// Obtiene el detalle de error de una excepcion.
        /// </summary>
        /// <param name="exc">Instancia de la excepcion.</param>
        /// <param name="messagePrefix">Prefijo para el mensaje de error.</param>
        /// <returns>Detalle de error de la excepcion.</returns>
        public static string GetErrorDetail(Exception exc, string messagePrefix)
        {
            return GetErrorDetail(exc, false, messagePrefix);
        }

        /// <summary>
        ///  Obtiene el detalle de error de una excepcion.
        /// </summary>
        /// <param name="exc">Instancia de la excepcion.</param>
        /// <param name="includeDetail">Indica si se incluye o no el detalle de la excepcion</param>
        /// <returns>Detalle de error de la excepcion.</returns>
        public static string GetErrorDetail(Exception exc, bool includeDetail)
        {
            return GetErrorDetail(exc, includeDetail, "Ha ocurrido un error");
        }

        /// <summary>
        ///  Obtiene el detalle de error de una excepcion.
        /// </summary>
        /// <param name="exc">Instancia de la excepcion.</param>
        /// <param name="includeDetail">Indica si se incluye o no el detalle de la excepcion</param>
        /// <param name="errorPrefix">Prefijo para el mensaje de error.</param>
        /// <returns>Detalle de error de la excepcion.</returns>
        public static string GetErrorDetail(Exception exc, bool includeDetail, string errorPrefix)
        {
            var sbErrorMessage = new StringBuilder();
            sbErrorMessage.AppendLine(string.Format("{0}: {1}", errorPrefix, exc.Message));
            if (includeDetail)
            {
                sbErrorMessage.AppendLine(string.Format("Tipo: {0}", exc.GetType().FullName));
                sbErrorMessage.AppendLine(string.Format("StackTrace: {0}", exc.StackTrace));
                //sbErrorMessage.AppendLine(string.Format("Detalle: {0}", exc.ToString()));    
                if (exc.InnerException != null)
                {
                    sbErrorMessage.AppendLine(string.Format("InnerException: {0}", exc.InnerException));
                }
            }
            return sbErrorMessage.ToString();
        }

        /// <summary>
        /// Reconstruye un objeto desde una cadena en formato XML.
        /// </summary>
        /// <param name="data">Texto en formato xml.</param>
        /// <param name="objecType">Tipo del objeto de retorno.</param>
        /// <returns>Objeto deserializado.</returns>
        public static object DeserializeXmlString(string data, Type objecType)
        {
            object deserializedData = null;
            try
            {
                using (StringReader reader = new StringReader(data))
                {
                    XmlSerializer serializer = new XmlSerializer(objecType);
                    XmlReader xmlReader = XmlReader.Create(reader);
                    deserializedData = serializer.Deserialize(xmlReader);
                }
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return deserializedData;
        }

        /// <summary>
        /// Serializa un objeto determinado en una cadena XML.
        /// </summary>
        /// <param name="instance">Objeto a serializar.</param>
        /// <returns>Cadena XML correspondiente al objeto serializado.</returns>
        public static string SerializeObjectAsXml(object instance)
        {
            string xmlSerializedString = null;
            try
            {
                if (instance == null)
                {
                    throw new ArgumentNullException("instance", "El Objeto a serializar no puede ser nulo.");
                }
                using (StringWriter sw = new StringWriter(CultureInfo.CurrentCulture))
                {
                    XmlSerializer serializer = new XmlSerializer(instance.GetType());
                    XmlWriterSettings settings = new XmlWriterSettings
                    {
                        Indent = true,
                        OmitXmlDeclaration = false
                    };

                    XmlWriter xmlWriter = XmlWriter.Create(sw, settings);

                    serializer.Serialize(xmlWriter, instance);
                    xmlSerializedString = sw.ToString().Trim();
                }
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return xmlSerializedString;
        }
    }
}
