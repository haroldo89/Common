// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/01/15.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Contiene utilidades comunes para trabajar con objetos de acceso conectado y 
//                  desconectado de ADO.NET.
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace AlbatrosSoft.Common.DataAccess.Extensions
{
    /// <summary>
    /// Contiene utilidades comunes para trabajar con objetos de acceso conectado y desconectado de ADO.NET.
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// Determina si la instancia de un DataSet posee datos.
        /// </summary>
        /// <param name="ds">Instancia del DataSet a evaluar.</param>
        /// <returns>true, si tiene al menos una tabla con datos, en caso contrario false.</returns>
        public static bool HasData(this DataSet ds)
        {
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determina si la instancia de un DataTable posee datos.
        /// </summary>
        /// <param name="dt">Instancia del DataTable a evaluar.</param>
        /// <returns>true, si tiene al menos una fila con datos, en caso contrario false.</returns>
        public static bool HasData(this DataTable dt)
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Permite renombrar las columnas de una tabla a partir de una lista de alias de nombres de columnas.
        /// </summary>
        /// <param name="dt">Instancia del DataTable.</param>
        /// <param name="columnNames">Lista de columnas a renombrar en la tabla con sus alias (Name = Nombre Original, Value = Alias).</param>
        /// <returns>Tabla con los nombres personalizados de columnas.</returns>
        public static DataTable RenameColumns(this DataTable dt, NameValueCollection columnNames)
        {
            if (dt.HasData() && columnNames != null)
            {
                try
                {
                    //Reemplazar los nombres de las columnas originales por los valores de alias
                    foreach (var columnNameKey in columnNames.Keys)
                    {
                        dt.Columns[columnNameKey.ToString()].ColumnName = columnNames[columnNameKey.ToString()];
                    }
                }
                catch (Exception exc)
                {
                    dt = GetErrorInfo(exc);
                }
            }
            return dt;
        }

        /// <summary>
        /// Convierte una Coleción a un DataTable.
        /// </summary>
        /// <typeparam name="T">Tipo de los elementos de la colección.</typeparam>
        /// <param name="items">Colección de elementos.</param>
        /// <returns>Datatable con los elementos de la colección.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            DataTable table = new DataTable(typeof(T).Name);
            if (items != null)
            {
                try
                {
                    //Obtener propiedades del objeto
                    PropertyDescriptorCollection itemProperties = TypeDescriptor.GetProperties(typeof(T));
                    var properties = itemProperties.Cast<PropertyDescriptor>().Where(p => IsValidType(p.PropertyType));
                    table = ToDataTable(items, properties);
                }
                catch (Exception exc)
                {
                    table = GetErrorInfo(exc);
                }
            }
            return table;
        }

        /// <summary>
        /// Convierte una Coleción a un DataTable.
        /// </summary>
        /// <typeparam name="T">Tipo de los elementos de la colección.</typeparam>
        /// <param name="items">Colección de elementos.</param>
        /// <param name="columnNames">Lista de propiedades con alias a incluir en la tabla.</param>
        /// <returns>Datatable con los elementos de la colección.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> items, NameValueCollection columnNames)
        {
            DataTable table = new DataTable(typeof(T).Name);
            if (items != null)
            {
                try
                {
                    //Obtener propiedades del objeto
                    PropertyDescriptorCollection itemProperties = TypeDescriptor.GetProperties(typeof(T));
                    var properties = itemProperties.Cast<PropertyDescriptor>().Where(p => IsValidType(p.PropertyType));
                    if (columnNames != null)
                    {
                        //Obtener columnas a incluir en la tabla
                        var columnsToInclude = columnNames.Keys.Cast<string>();
                        if (columnsToInclude.Any())
                        {
                            properties = properties.Where(p => columnsToInclude.Contains(p.Name));
                        }
                    }
                    table = ToDataTable(items, properties).RenameColumns(columnNames);
                }
                catch (Exception exc)
                {
                    table = GetErrorInfo(exc);
                }
            }
            return table;
        }

        /// <summary>
        /// Convierte una Coleción a un DataTable.
        /// </summary>
        /// <typeparam name="T">Tipo de los elementos de la colección.</typeparam>
        /// <param name="items">Colección de elementos.</param>
        /// <param name="columns">Nombre de propiedades a incluir en la tabla (Separadas por ,)</param>
        /// <returns>Datatable con los elementos de la colección.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> items, string columns)
        {
            DataTable table = new DataTable(typeof(T).Name);
            const char COLUMN_NAME_SEPARATOR = ',';
            if (items != null)
            {
                try
                {
                    //Obtener propiedades del objeto
                    PropertyDescriptorCollection itemProperties = TypeDescriptor.GetProperties(typeof(T));
                    var properties = itemProperties.Cast<PropertyDescriptor>().Where(p => IsValidType(p.PropertyType));
                    if (!string.IsNullOrEmpty(columns))
                    {
                        //Obtener columnas a incluir en la tabla
                        var columnsToInclude = columns.Trim().Replace(" ", string.Empty).Split(new char[] { COLUMN_NAME_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                        if (columnsToInclude.Any())
                        {
                            properties = properties.Where(p => columnsToInclude.Contains(p.Name));
                        }
                    }
                    table = ToDataTable(items, properties);
                }
                catch (Exception exc)
                {
                    table = GetErrorInfo(exc);
                }
            }
            return table;
        }

        /// <summary>
        /// Convierte una Coleción a un DataTable.
        /// </summary>
        /// <typeparam name="T">Tipo de los elementos de la colección.</typeparam>
        /// <param name="items">Colección de elementos.</param>
        /// <param name="propertyColumns">Conjunto de propiedades a incluir en la tabla.</param>
        /// <returns></returns>
        private static DataTable ToDataTable<T>(IEnumerable<T> items, IEnumerable<PropertyDescriptor> propertyColumns)
        {
            DataTable table = new DataTable(typeof(T).Name);
            if (items != null)
            {
                if (propertyColumns != null)
                {
                    if (propertyColumns.Any())
                    {
                        try
                        {
                            //Agregar columnas a la tabla
                            foreach (PropertyDescriptor prop in propertyColumns)
                            {
                                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                            }
                            //Agregar filas a la tabla
                            foreach (T item in items)
                            {
                                DataRow row = table.NewRow();
                                foreach (PropertyDescriptor prop in propertyColumns)
                                {
                                    //Asignar valor a la celda
                                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                                }
                                //Agregar fila a la tabla
                                table.Rows.Add(row);
                            }
                        }
                        catch (Exception exc)
                        {
                            table = GetErrorInfo(exc);
                        }
                    }
                }
            }
            return table;
        }

        private static bool IsValidType(Type type)
        {
            return IsPrimitiveType(type) || IsDTOType(type);
        }

        private static bool IsPrimitiveType(Type type)
        {
            return !IsDTOType(type) && !type.FullName.Contains("System.Collections");
        }

        private static bool IsDTOType(Type type)
        {
            return type.Namespace.EndsWith(".Dto");
        }

        private static DataTable GetErrorInfo(Exception exc)
        {
            DataTable dtError = new DataTable("ErrorInfo");
            dtError = new DataTable();
            dtError.Columns.Add("ErrorMessage");
            dtError.Columns.Add("ErrorType");
            dtError.Columns.Add("StackTrace");
            dtError.Rows.Add(exc.Message, exc.GetType().Name, exc.StackTrace);
            return dtError;
        }
    }
}
