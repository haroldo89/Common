// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/01/15.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Expone las funcionalidades básicas más comunes que se utilizan al interactuar con 
//                  una base de datos
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Diagnostics;


namespace AlbatrosSoft.Common.DataAccess
{
    /// <summary>
    /// Expone las funcionalidades básicas más comunes que se utilizan al interactuar con una base de datos
    /// </summary>
    public class GenericRepository : IDisposable
    {
        /// <summary>
        /// Helper para conexion a base de datos.
        /// </summary>
        protected DbConnectionHelper dbHelper;

        //public abstract void SetProperties(BaseEntity entity, DataTable dt);
        //public abstract void SetProperties(BaseEntity entity, DataRow row);
        //public abstract void SetProperties(BaseEntity entity, DbDataReader dr);

        /// <summary>
        /// Constructor que recibe por parametros la cadena de conexion y 
        /// el nombre de proveedor para la conexion a la base de datos
        /// </summary>
        /// <param name="connectionString">Cadena de conexion</param>
        /// <param name="providerName">Proveedor de conexion a base de datos</param>
        public GenericRepository(string connectionString, string providerName)
            : this(new DbConnectionHelper(connectionString, providerName))
        {

        }

        /// <summary>
        /// Constructore que recibe por parametro un helper de conexion a base de datos
        /// </summary>
        /// <param name="dbHelper"></param>
        public GenericRepository(DbConnectionHelper dbHelper)
        {
            if (dbHelper == null)
            {
                throw new Exception("El objeto manejador de conexion a base de datos no puede ser nulo.");
            }
            this.dbHelper = dbHelper;
        }

        /// <summary>
        /// Ejecuta una consulta SQL contra una base de datos y devuelve los resultados obtenidos en forma de tabla.
        /// </summary>
        /// <param name="sql">Comando SQL a ejecutar.</param>
        /// <returns>Resultados devueltos por la consulta en forma de tabla.</returns>
        public DataTable GetData(string sql)
        {
            DataTable dt = null;
            lock (_lock)
            {
                using (var con = this.dbHelper.Connect())
                {
                    if (this.TryOpenConnection(con))
                    {
                        DbCommand cmd = con.CreateCommand();
                        if (cmd != null)
                        {
                            cmd.CommandText = sql;
                            cmd.CommandTimeout = 0;
                            DbDataReader dr = cmd.ExecuteReader();
                            if (dr != null)
                            {
                                dt = new DataTable();
                                dt.Load(dr);
                            }
                        }
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado que retorna datos y devuelve los resultados obtenidos en forma de tabla.
        /// </summary>
        /// <param name="procedureName">Nombre del procedimiento</param>
        /// <param name="inputParameters">Parámetros de entrada del procedimiento (null indica sin parámetros).</param>
        /// <returns>Resultados devueltos por el procedimiento en forma de tabla.</returns>
        public DataTable GetData(string procedureName, ICollection<DbParameterItem> inputParameters)
        {
            DataTable dt = null;
            lock (_lock)
            {
                using (var con = this.dbHelper.Connect())
                {
                    if (this.TryOpenConnection(con))
                    {
                        DbCommand cmd = con.CreateCommand();
                        if (cmd != null)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = procedureName;
                            cmd.CommandTimeout = 0;
                            if (inputParameters != null)
                            {
                                foreach (var item in inputParameters)
                                {
                                    DbParameter parameter = this.dbHelper.Provider.CreateParameter();
                                    this.SetParameterProperties(parameter, item);
                                    cmd.Parameters.Add(parameter);
                                }
                            }
                            DbDataReader dr = cmd.ExecuteReader();
                            if (dr != null)
                            {
                                dt = new DataTable();
                                dt.Load(dr);
                            }
                        }
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Ejecuta un comando sql de manipulación de datos y devuelve el número de filas afectadas tras la ejecución
        /// del comando.
        /// </summary>
        /// <param name="sql">Comando SQL a ejecutar.</param>
        /// <returns>Número de filas afectadas tras la ejecución del comando.</returns>
        public int ExecuteNonQuery(string sql)
        {
            int rowsAffected = 0;
            lock (_lock)
            {
                using (var con = this.dbHelper.Connect())
                {
                    if (this.TryOpenConnection(con))
                    {
                        DbCommand cmd = con.CreateCommand();
                        if (cmd != null)
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sql;
                            cmd.CommandTimeout = 0;
                            rowsAffected = cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado o función alojados en la base de datos de destino.
        /// </summary>
        /// <param name="procedureName">Nombre del procedimiento o función.</param>
        /// <param name="inputParameters">Parámetros de entrada del procedimiento (null indica sin parámetros).</param>
        /// <returns>Número de filas afectadas tras la ejecución del procedimiento almacenado o función.</returns>
        public int ExecuteNonQuery(string procedureName, ICollection<DbParameterItem> inputParameters)
        {
            int rowsAffected = 0;
            lock (_lock)
            {
                using (var con = this.dbHelper.Connect())
                {
                    if (this.TryOpenConnection(con))
                    {
                        DbCommand cmd = con.CreateCommand();
                        if (cmd != null)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = procedureName;
                            cmd.CommandTimeout = 0;
                            if (inputParameters != null)
                            {
                                foreach (var item in inputParameters)
                                {
                                    DbParameter parameter = this.dbHelper.Provider.CreateParameter();
                                    this.SetParameterProperties(parameter, item);
                                    cmd.Parameters.Add(parameter);
                                }
                            }
                            rowsAffected = cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado o función alojados en la base de datos de destino.
        /// </summary>
        /// <param name="procedureName">Nombre del procedimiento o función.</param>
        /// <param name="inputParameters">Parámetros de entrada del procedimiento.</param>
        /// <param name="outputParameter">Parámetro de salida principal (para el caso de procedimientos almacenados.)</param>
        /// <returns>Número de filas afectadas tras la ejecución del procedimiento almacenado o función.</returns>
        public object ExecuteNonQuery(string procedureName, ICollection<DbParameterItem> inputParameters, DbParameterItem outputParameter)
        {
            object outputValue = 0;
            using (var con = this.dbHelper.Connect())
            {
                if (this.TryOpenConnection(con))
                {
                    DbCommand cmd = con.CreateCommand();
                    if (cmd != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = procedureName;
                        cmd.CommandTimeout = 0;
                        foreach (var item in inputParameters)
                        {
                            DbParameter parameter = this.dbHelper.Provider.CreateParameter();
                            this.SetParameterProperties(parameter, item);
                            cmd.Parameters.Add(parameter);
                        }
                        DbParameter outParameter = this.dbHelper.Provider.CreateParameter();
                        this.SetParameterProperties(outParameter, outputParameter);
                        cmd.Parameters.Add(outParameter);
                        cmd.ExecuteNonQuery();
                        outputValue = outParameter.Value;
                        //Actualizar valor de parámetros de salida
                        inputParameters.Where(p => p.Direction == ParameterDirection.Output).ToList().ForEach(p => p.Value = cmd.Parameters.Cast<DbParameter>().FirstOrDefault(dbp => dbp.ParameterName.Equals(p.Name)).Value);
                    }
                }
            }
            return outputValue;
        }

        /// <summary>
        /// Determina si existen datos que cumplen con los criterios de una consulta SQL.
        /// </summary>
        /// <param name="sql">Consulta SQL a evaluar.</param>
        /// <returns>true, si existen datos que cumplen con la consulta, de lo contrario false.</returns>
        public bool ExistsData(string sql)
        {
            DataTable dt = this.GetData(sql);
            return dt == null ? false : dt.Rows.Count > 0;
        }

        #region Generación de autonuméricos
        /// <summary>
        /// Permite generar un valor autonumerico dentro de una tabla de una base de datos conectada.
        /// </summary>
        /// <param name="field">Campo de la tabla.</param>
        /// <param name="table">Nombre de la tabla.</param>
        /// <returns>Nuevo valor autonumerico.</returns>
        public int GenerateAutonumber(string field, string table)
        {
            string sql = string.Format("SELECT MAX({0}) AS autonum FROM {1}", field, table);
            return this.GetAutonumber(sql);
        }

        /// <summary>
        /// Permite generar un valor autonumerico dentro de una tabla de una base de datos conectada para una condicion especifica.
        /// </summary>
        /// <param name="field">Campo de la tabla.</param>
        /// <param name="table">Nombre de la tabla.</param>
        /// <param name="mainField">Campo principal de la tabla.</param>
        /// <param name="mainValue">Valor principal de la tabla.</param>
        /// <returns></returns>
        public int GenerateAutonumber(string field, string table, string mainField, int mainValue)
        {
            string sql = string.Format("SELECT MAX({0}) AS autonum FROM {1} WHERE {2} = {3}", field, table, mainField, mainValue);
            return this.GetAutonumber(sql);
        }

        /// <summary>
        /// Devuelve valor incrementado de un campo autonumerico de una tabla.
        /// </summary>
        /// <param name="sql">Consulta para generacion del autonumerico.</param>
        /// <returns>Valor incrementado para el campo autonumerico.</returns>
        private int GetAutonumber(string sql)
        {
            int autoNumber = 0;
            using (DataTable dt = this.GetData(sql))
            {
                int.TryParse(Convert.ToString(dt.Rows[0]["autonum"]), out autoNumber);
            }
            return autoNumber + 1;
        }
        #endregion

        private void SetParameterProperties(DbParameter parameter, DbParameterItem parameterItem)
        {
            parameter.ParameterName = parameterItem.Name;
            parameter.DbType = parameterItem.Type;
            parameter.Direction = parameterItem.Direction;
            parameter.Value = parameterItem.Value;
            if (parameterItem.Size > 0)
            {
                parameter.Size = parameterItem.Size;
            }
        }

        private bool TryOpenConnection(DbConnection con)
        {
            lock (_lock)
            {
                if (con.State != ConnectionState.Open)
                {
                    try
                    {
                        con.Open();
                        return true;
                    }
                    //catch (InvalidOperationException)
                    finally
                    {
                    }
                }
            }
            return false;
        }

        #region Miembros de IDisposable
        /// <summary>
        /// Releases all resources used by an instance of the <see cref="GenericRepository" /> class.
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
        /// Releases unmanaged resources before an instance of the <see cref="GenericRepository" /> 
        /// class is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// NOTE: Leave out the finalizer altogether if this class doesn't 
        /// own unmanaged resources itself, but leave the other methods
        /// exactly as they are.
        /// This method releases unmanaged resources by calling the virtual 
        /// <see cref="Dispose(bool)" /> method, passing in false.
        /// </remarks>
        ~GenericRepository()
        {
            this.Dispose(false);
        }

        object _lock = new object();

        /// <summary>
        /// Releases the unmanaged resources used by an instance of the 
        /// <see cref="GenericRepository" /> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources;        
        /// false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_lock)
                {
                    //Trace.WriteLine(string.Format("Disposing {0} . . .", this.GetType().Name));
                    if (this.dbHelper != null)
                    {
                        this.dbHelper.Dispose();
                        this.dbHelper = null;
                    }
                }
            }
            // free native resources if there are any.      
        }
        #endregion Miembros de IDisposable

    }
}
