// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/08/22.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Extensiones para fechas.
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
using System.Globalization;

namespace AlbatrosSoft.Common
{
    /// <summary>
    /// Extensiones para fechas.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Obtener rango de semana apartir de una fecha especificada, siendo el domingo el primer dia de la semana 
        /// y el sabado el ultimo dia de la semana.
        /// </summary>
        /// <param name="date">Fecha</param>
        /// <param name="cultureInfo">Informacion de cultura</param>
        /// <param name="startWeekDate">Primer dia de la semana(parametro de salida)</param>
        /// <param name="endWeekDate">Ultimo de la semana (parametro de salida)</param>
        public static IEnumerable<DateTime> GetWeek(this DateTime date, CultureInfo cultureInfo)
        {
            List<DateTime> dateTimes = new List<DateTime>();
            DateTime startWeekDate, endWeekDate;
            if (date == null)
            {
                throw new ArgumentNullException("date", "El valor de fecha no puede ser un valor nulo o vacio ");
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException("cultureInfo", "El valor de la informacion de cultura no puede ser un valor nulo o vacio ");
            }
            //Obtener primer de la semana
            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            int offset = firstDayOfWeek - date.DayOfWeek;
            if (offset != 1)
            {
                DateTime weekStart = date.AddDays(offset);
                DateTime endOfWeek = weekStart.AddDays(6);
                startWeekDate = weekStart;
                endWeekDate = endOfWeek;
            }
            else
            {
                startWeekDate = date.AddDays(-6);
                endWeekDate = date;
            }
            //Truncar fechas.
            startWeekDate = new DateTime(startWeekDate.Year, startWeekDate.Month, startWeekDate.Day);
            endWeekDate = new DateTime(endWeekDate.Year, endWeekDate.Month, endWeekDate.Day);
            //Agregar fechas a array
            dateTimes.Insert(0, startWeekDate);
            dateTimes.Insert(1, endWeekDate);
            //Devolver rango de fechas.
            return dateTimes;
        }

        /// <summary>
        /// Obtener rango de mes apartir de una fecha especificada.
        /// </summary>
        /// <param name="date">Fecha.</param>
        /// <param name="startMonthDate">Fecha inicial del rango de fechas(parametro de salida).</param>
        /// <param name="endMonthDate">Fecha final del rango de fechas(parametro de salida).</param>
        public static IEnumerable<DateTime> GetMonth(this DateTime date)
        {
            List<DateTime> dateTimes = new List<DateTime>();
            DateTime startMonthDate, endMonthDate;
            if (date == null)
            {
                throw new ArgumentNullException("date", "El valor de fecha no puede ser un valor nulo o vacio ");
            }
            //Obtener primer dia del mes apartir del dia actual
            startMonthDate = new DateTime(date.Year, date.Month, 1);
            //Obtener fecha final del mes apartir del dia actual.
            endMonthDate = startMonthDate.AddMonths(1);
            endMonthDate = endMonthDate.AddDays(-1);
            //Truncar fechas.
            startMonthDate = new DateTime(startMonthDate.Year, startMonthDate.Month, startMonthDate.Day);
            endMonthDate = new DateTime(endMonthDate.Year, endMonthDate.Month, endMonthDate.Day);
            //Agregar fechas a array
            dateTimes.Insert(0, startMonthDate);
            dateTimes.Insert(1, endMonthDate);
            return dateTimes;
        }
    }
}
