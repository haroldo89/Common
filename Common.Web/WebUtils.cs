// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/08/15.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Contiene funciones utiles para programacion web.
//             
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AlbatrosSoft.Common.Web
{
    /// <summary>
    /// Utilidades para proyectos web.
    /// </summary>
    public static class WebUtils
    {
        /// <summary>
        /// Devuelve la dirección IP version 4.0 correspondiente a un equipo local.
        /// </summary>
        /// <returns>Dirección de red interna del equipo local.</returns>
        public static string GetMachineNetworkAddress()
        {
            string ipAddress = string.Empty;
            string host = Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(host);
            if (ipHostInfo.AddressList != null)
            {
                ipAddress = ipHostInfo.AddressList.AsEnumerable().FirstOrDefault(a => a.AddressFamily.Equals(AddressFamily.InterNetwork)).ToString();
            }
            return ipAddress;
        }

        /// <summary>
        /// Obtiene la dirección IP externa a través de la cual proviene una solicitud hacia el servidor web.
        /// </summary>
        /// <returns>Dirección IP externa desde donde se origina una solicitud web.</returns>
        public static string GetExternalAddress()
        {
            string ipAddress = string.Empty;
            WebClient webClient = new WebClient();
            string URL_BASE = "http://checkip.dyndns.org/";
            //Expresion regular que coincide con una IPv4
            string IP_V4_REGEX = @"(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})";
            using (Stream data = webClient.OpenRead(URL_BASE))
            {
                StreamReader reader = new StreamReader(data);
                string responseString = reader.ReadToEnd();
                ipAddress = Regex.Match(responseString, IP_V4_REGEX, RegexOptions.Compiled).ToString();
            }
            return ipAddress;
        }

        /// <summary>
        /// Permite ordenar los datos de un objeto DataTable de acuerdo a la expresión y direccion indicadas.
        /// </summary>
        /// <param name="dt">Objeto de tipo DataTable a ordenar.</param>
        /// <param name="sortExpression">Expresion de ordenamiento.</param>
        /// <param name="sortDirection">Dirección del ordenamiento (ASC, DESC).</param>
        /// <returns>Conjunto de datos organizados de acuerdo a los criterios especificados.</returns>
        public static DataView SortDataTable(DataTable dt, string sortExpression, string sortDirection)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dataView = new DataView(dt);
                if (sortExpression != string.Empty)
                {
                    dataView.Sort = string.Format("{0} {1}", sortExpression, sortDirection);
                }
                return dataView;
            }
            return null;
        }

        /// <summary>
        /// Permite exportar un objeto DataTable a un archivo de hoja de cálculo de Excel.
        /// </summary>
        /// <param name="table">Instancia del DataTable.</param>
        /// <param name="fileName">Nombre del archivo de destino.</param>
        /// <param name="context">Contexto Http del servidor web.</param>
        public static void ExportToSpreadsheet(DataTable table, string fileName, HttpContext context)
        {
            if (table != null)
            {
                if (context != null)
                {
                    try
                    {
                        context.Response.Clear();
                        context.Response.Buffer = true;
                        context.Response.ContentType = "application/vnd.ms-excel";
                        context.Response.ContentEncoding = Encoding.GetEncoding("windows-1252");
                        context.Response.Charset = "utf-8";
                        context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));
                        //Definir estilos
                        StringBuilder sbDocBody = new StringBuilder();
                        sbDocBody.Append("<style>");
                        sbDocBody.Append(".Header {  background-color:Navy; color:#ffffff; font-weight:bold;font-family:Verdana; font-size:12px;}");
                        sbDocBody.Append(".SectionHeader { background-color:#8080aa; color:#ffffff; font-family:Verdana; font-size:12px;font-weight:bold;}");
                        sbDocBody.Append(".Content { background-color:#ccccff; color:#000000; font-family:Verdana; font-size:12px;text-align:left}");
                        sbDocBody.Append(".Label { background-color:#ccccee; color:#000000; font-family:Verdana; font-size:12px; text-align:right;}");
                        sbDocBody.Append("</style>");
                        sbDocBody.Append("<br><table align=\"center\" cellpadding=1 cellspacing=0 style=\"background-color:#000000;\">");
                        sbDocBody.Append("<tr><td width=\"500\">");
                        sbDocBody.Append("<table width=\"100%\" cellpadding=1 cellspacing=2 style=\"background-color:#ffffff;\">");
                        //
                        if (table.Rows.Count > 0)
                        {
                            sbDocBody.Append("<tr><td>");
                            sbDocBody.Append("<table width=\"600\" cellpadding=\"0\" cellspacing=\"2\"><tr><td>");
                            //Agregar encabezados de columna
                            sbDocBody.Append("<tr><td width=\"25\"> </td></tr>");
                            sbDocBody.Append("<tr>");
                            sbDocBody.Append("<td> </td>");
                            for (int i = 0; i < table.Columns.Count; i++)
                            {
                                sbDocBody.Append("<td class=\"Header\" width=\"120\">" + table.Columns[i].ToString().Replace(".", "<br>") + "</td>");
                            }
                            sbDocBody.Append("</tr>");
                            //Agregar filas de la tabla
                            for (int i = 0; i < table.Rows.Count; i++)
                            {
                                sbDocBody.Append("<tr>");
                                sbDocBody.Append("<td> </td>");
                                for (int j = 0; j < table.Columns.Count; j++)
                                {
                                    //var cellValue = table.Rows[i][j].ToString();
                                    var cellValue = GetCellValue(table.Rows[i][j]);
                                    sbDocBody.Append("<td class=\"Content\">" + ParseCellValue(cellValue) + "</td>");
                                }
                                sbDocBody.Append("</tr>");
                            }
                            sbDocBody.Append("</table>");
                            sbDocBody.Append("</td></tr></table>");
                            sbDocBody.Append("</td></tr></table>");
                        }
                        context.Response.Write(sbDocBody.ToString());
                        context.Response.End();
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Permite obtener el valor de una celda de Excel con formato
        /// </summary>
        /// <param name="inputValue">Valor de celda</param>
        /// <returns></returns>
        private static string GetCellValue(object inputValue)
        {
            string formattedValue = string.Empty;
            if (inputValue != null)
            {
                var valueType = inputValue.GetType().Name;
                switch (valueType)
                {
                    case "Int16":
                    case "Int32":
                        formattedValue = Convert.ToInt32(inputValue).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case "Int64":
                        formattedValue = Convert.ToInt64(inputValue).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case "Double":
                        formattedValue = Convert.ToDouble(inputValue).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case "Single":
                        formattedValue = Convert.ToSingle(inputValue).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case "Decimal":
                        formattedValue = Convert.ToDecimal(inputValue).ToString("G", CultureInfo.InvariantCulture);
                        break;
                    case "DateTime":
                        formattedValue = Convert.ToDateTime(inputValue).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        break;
                    default:
                        formattedValue = inputValue.ToString();
                        break;
                }
            }
            return formattedValue;
        }

        private static string ParseCellValue(string inputCellValue)
        {
            string outputCellValue = inputCellValue;
            const int MAX_NUMBER_LENGHT = 11;
            bool isNumeric = inputCellValue.ToCharArray().All(c => char.IsNumber(c));
            bool isDecimal = isNumeric && inputCellValue.IndexOfAny(new char[] { ',', '.' }) > 0;
            if (isNumeric && inputCellValue.Length > MAX_NUMBER_LENGHT && !isDecimal)
            {
                outputCellValue = string.Format("'{0}", inputCellValue);
            }
            return outputCellValue;
        }

        /// <summary>
        /// Permite exportar un objeto DataTable a un archivo de hoja de cálculo sin formato de Excel.
        /// </summary>
        /// <param name="table">Instancia del DataTable.</param>
        /// <param name="fileName">Nombre del archivo de destino.</param>
        /// <param name="context">Contexto Http del servidor web.</param>
        public static void ExportToFlatSpreadsheet(DataTable table, string fileName, HttpContext context)
        {
            if (table != null)
            {
                if (context != null)
                {
                    try
                    {
                        context.Response.Clear();
                        context.Response.Buffer = true;
                        context.Response.ContentType = "application/vnd.ms-excel";
                        context.Response.ContentEncoding = Encoding.GetEncoding("windows-1252");
                        context.Response.Charset = "utf-8";
                        context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));
                        //Agregar encabezados de columna
                        foreach (DataColumn column in table.Columns)
                        {
                            context.Response.Write(string.Format(" {0} \t", column.ColumnName));
                        }
                        context.Response.Write(Environment.NewLine);
                        //Agregar filas de la tabla
                        foreach (DataRow row in table.Rows)
                        {
                            for (int i = 0; i < table.Columns.Count; i++)
                            {
                                context.Response.Write(String.Format("{0}\t", row[i]));
                            }
                            context.Response.Write(Environment.NewLine);
                        }
                        context.Response.End();
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Cierra la sesión actual de una aplicación web y libera todos los recursos asociados a la misma.
        /// </summary>
        /// <param name="currentContext">Contexto HTTP de la aplicación web.</param>
        public static void CloseSession(HttpContext currentContext)
        {
            try
            {
                if (currentContext.Session != null)
                {
                    currentContext.Session.Clear();
                    currentContext.Session.Abandon();
                }
                if (currentContext.Request.Cookies != null)
                {
                    currentContext.Request.Cookies.Clear();
                }
                if (currentContext.Cache != null)
                {
                    if (currentContext.Cache.Count > 0)
                    {
                        DeleteCache(currentContext);
                    }
                }
                if (FormsAuthentication.IsEnabled)
                {
                    FormsAuthentication.SignOut();
                }
            }
            catch { }
        }

        /// <summary>
        /// Libera los recursos asociados a Cache de una aplicación Web.
        /// </summary>
        /// <param name="currentContext">Contexto HTTP de la aplicación web.</param>
        private static void DeleteCache(HttpContext currentContext)
        {
            ArrayList keys = new ArrayList();
            for (IDictionaryEnumerator enumerator = currentContext.Cache.GetEnumerator(); enumerator.MoveNext(); )
            {
                keys.Add(enumerator.Key.ToString());
            }
            foreach (string item in keys)
            {
                currentContext.Cache.Remove(item);
            }
        }

        /// <summary>
        /// Imprime una linea de texto en una pagina del servidor web
        /// </summary>
        /// <param name="line">Linea de texto a imprimir</param>
        /// <param name="page">Instancia de la página en el servidor.</param>
        public static void WriteLineInPage(string line, Page page)
        {
            page.Response.Write(string.Format("{0}<br>", line));
        }

        /// <summary>
        /// Muestra un mensaje de dialogo dentro de una página web.
        /// </summary>
        /// <param name="message">Mensaje a mostrar.</param>
        /// <param name="page">Instancia de la página en el servidor.</param>
        public static void ShowMessageDialog(string message, Page page)
        {
            string alertScriptBlock = "<script type='text/javascript'>alert('{0}');</script>";
            string scriptKey = Guid.NewGuid().ToString();
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), scriptKey, string.Format(alertScriptBlock, message), false);
        }

        /// <summary>
        /// Muestra un mensaje de dialogo dentro de un control de tipo contenedor.
        /// </summary>
        /// <param name="message">Mensaje a mostrar.</param>
        /// <param name="container">Control contenedor desde donde se mostrará el mensaje (ej. UpdatePanel)</param>
        public static void ShowMessageDialog(string message, Control container)
        {
            string alertScriptBlock = "<script type='text/javascript'>alert('{0}');</script>";
            string scriptKey = Guid.NewGuid().ToString();
            ScriptManager.RegisterClientScriptBlock(container, container.GetType(), scriptKey, string.Format(alertScriptBlock, message), false);
        }

        /// <summary>
        /// Permite limpiar los controles de una página.
        /// </summary>
        /// <param name="controls">Coleccion de controles de la página.</param>
        public static void ClearControls(ControlCollection controls)
        {
            //var rootControls = controls.Cast<Control>().Where(c => c.Parent != null).Where(c => !c.Parent.GetType().Equals(typeof(GridView)));
            var rootControls = controls.Cast<Control>().Where(c => !c.Parent.GetType().Equals(typeof(GridView)));
            foreach (Control control in rootControls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Text = string.Empty;
                }
                else if (control is Label)
                {
                    ((Label)control).Text = string.Empty;
                }
                else if (control is DropDownList)
                {
                    ((DropDownList)control).ClearSelection();
                }
                else if (control is RadioButtonList)
                {
                    ((RadioButtonList)control).ClearSelection();
                }
                else if (control is CheckBoxList)
                {
                    ((CheckBoxList)control).ClearSelection();
                }
                else if (control is RadioButton)
                {
                    ((RadioButton)control).Checked = false;
                }
                else if (control is CheckBox)
                {
                    ((CheckBox)control).Checked = false;
                }
                else if (control.HasControls())
                {
                    ClearControls(control.Controls);
                }
            }
        }
    }
}
