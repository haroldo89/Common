// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/01/15.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Expone toda la funcionalidad necesaria para comunicarse e interactuar con una 
//                  base de datos.
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
    /// Expone toda la funcionalidad necesaria para comunicarse e interactuar con una base de datos.
    /// </summary>
    public sealed class DbConnectionHelper : IDisposable
    {
        /// <summary>
        /// Cadena de conexion a la base de datos.
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Nombre de conexion a base de datos.
        /// </summary>
        public string ProviderName { get; set; }
        private DbProviderFactory provider;

        internal DbProviderFactory Provider
        {
            get
            {
                if (this.provider == null)
                {
                    try
                    {
                        this.provider = DbProviderFactories.GetFactory(this.ProviderName);
                    }
                    catch (Exception)
                    {
                        this.provider = null;
                    }
                }
                return this.provider;
            }
        }

        /// <summary>
        /// Constructor que recibe por parametros, la cadena de conexion y el proveedor de 
        /// conexion a la base de datos
        /// </summary>
        /// <param name="connectionString">Cadena de conexion</param>
        /// <param name="providerName">Proveedor de conexion a base de datos</param>
        public DbConnectionHelper(string connectionString, string providerName)
        {
            this.ConnectionString = connectionString;
            this.ProviderName = providerName;
        }

        internal DbConnection Connect()
        {
            DbConnection con = null;
            lock (_lock)
            {
                try
                {
                    if (this.Provider != null)
                    {
                        con = this.Provider.CreateConnection();
                        if (con != null)
                        {
                            con.ConnectionString = this.ConnectionString;
                            return con;
                        }
                    }
                }
                catch (DbException exc)
                {
                    throw exc;
                }
            }
            return null;
        }

        #region Miembros de IDisposable
        /// <summary>
        /// Releases all resources used by an instance of the <see cref="DbConnectionHelper" /> class.
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
        /// Releases unmanaged resources before an instance of the <see cref="DbConnectionHelper" /> 
        /// class is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// NOTE: Leave out the finalizer altogether if this class doesn't 
        /// own unmanaged resources itself, but leave the other methods
        /// exactly as they are.
        /// This method releases unmanaged resources by calling the virtual 
        /// <see cref="Dispose(bool)" /> method, passing in false.
        /// </remarks>
        ~DbConnectionHelper()
        {
            this.Dispose(false);
        }

        object _lock = new object();

        /// <summary>
        /// Releases the unmanaged resources used by an instance of the 
        /// <see cref="DbConnectionHelper" /> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources;        
        /// false to release only unmanaged resources.</param>
        internal void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_lock)
                {
                    //Trace.WriteLine(string.Format("Disposing {0} . . .", this.GetType().Name));
                    if (this.provider != null)
                    {
                        this.provider = null;
                    }
                }

            }
            // free native resources if there are any.      
        }
        #endregion Miembros de IDisposable

    }
}
