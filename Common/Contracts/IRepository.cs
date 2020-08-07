// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/08/22.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Expone la funcionalidad base de un repositorio de objetos que ofrece servicios básicos 
//                  de lectura y escritura sobre un medio de persistencia.
//             
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
using System.Threading.Tasks;

namespace AlbatrosSoft.Common.Contracts
{
    /// <summary>
    /// Expone la funcionalidad base de un repositorio de objetos que ofrece servicios básicos de lectura y escritura sobre un medio de persistencia.
    /// </summary>
    /// <typeparam name="TEntity">Tipo concreto de objetos a manipular.</typeparam>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Adiciona el objeto especificado al almacén de datos.
        /// </summary>
        /// <param name="entity">Instancia del objeto a guardar.</param>
        /// <returns>Mensaje de resultado de la operación (Vacío = Operación Exitosa).</returns>
        string Add(TEntity entity);

        /// <summary>
        /// Persiste los cambios realizados al objeto indicado en el almacén de datos.
        /// </summary>
        /// <param name="entity">Instancia del objeto a guardar.</param>
        /// <returns>Mensaje de resultado de la operación (Vacío = Operación Exitosa).</returns>
        string Update(TEntity entity);

        /// <summary>
        /// Ejecuta la operación de eliminación del objeto indicado en el almacén de datos.
        /// </summary>
        /// <param name="entity">Instancia del objeto a eliminar.</param>
        /// <returns>Mensaje de resultado de la operación (Vacío = Operación Exitosa).</returns>
        string Remove(TEntity entity);
    }
}
