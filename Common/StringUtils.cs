// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo
// Fecha de Creación		:   2014/01/15.
// Producto o sistema	    :   
// Empresa			        :   
// Proyecto			        :   
// Cliente			        :   
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Agrupa un conjunto de utilidades comunes para trabajar con cadenas de texto.
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
using System.Globalization;
using System.Text.RegularExpressions;

namespace AlbatrosSoft.Common
{
    /// <summary>
    /// Agrupa un conjunto de utilidades comunes para trabajar con cadenas de texto.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Devuelve una cadena de entrada encerrada en comillas dobles.
        /// </summary>
        /// <param name="input">Cadena de entrada.</param>
        /// <returns>Texto encerrado entre comillas dobles.</returns>
        public static string InQuotes(string input)
        {
            return string.Format(CultureInfo.CurrentCulture, "\"{0}\"", input);
        }

        /// <summary>
        /// Devuelve una cadena de entrada encerrada en comillas sencillas.
        /// </summary>
        /// <param name="input">Cadena de entrada.</param>
        /// <returns>Texto encerrado entre comillas sencillas.</returns>
        public static string InSingleQuotes(string input)
        {
            return string.Format(CultureInfo.CurrentCulture, "'{0}'", input);
        }

        /// <summary>
        /// Convierte una cadena de formato a una expresion de formato.
        /// </summary>
        /// <param name="format">Cadena de entrada con el formato.</param>
        /// <returns>Expresión del formato.</returns>
        public static string ToFormatString(string format)
        {
            string formatExpression = string.Concat("{0:", format, "}");
            return formatExpression;
        }

        /// <summary>
        /// Transforma un valor numérico flotante de doble precisión al formato de cadena de texto independiente de la cultura.
        /// </summary>
        /// <param name="value">Valor numérico a transformar.</param>
        /// <returns>Representación en formato de texto.</returns>
        public static string ToDoubleString(double value)
        {
            string stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);
            return stringValue;
        }

        /// <summary>
        /// Extrae el texto localizado en el medio de dos cadenas de texto.
        /// </summary>
        /// <param name="beginString">Cadena de texto inicial</param>
        /// <param name="endString">Cadena de texto final</param>
        /// <param name="sourceString">Texto de origen</param>
        /// <returns>Texto contenido entre las dos cadenas.</returns>
        public static string GetStringInBetween(string beginString, string endString, string sourceString)
        {
            return GetStringInBetween(beginString, endString, sourceString, false, false);
        }

        /// <summary>
        /// Extrae el texto localizado en el medio de dos cadenas de texto.
        /// </summary>
        /// <param name="beginString">Cadena de texto inicial</param>
        /// <param name="endString">Cadena de texto final</param>
        /// <param name="sourceString">Texto de origen</param>
        /// <param name="includeBegin">Valor que indica si se incluye la cadena de inicio</param>
        /// <param name="includeEnd">Valor que indica si se incluye la cadena de fin</param>
        /// <returns>Texto contenido entre las dos cadenas.</returns>
        public static string GetStringInBetween(string beginString, string endString, string sourceString, bool includeBegin, bool includeEnd)
        {
            string result = string.Empty;
            int indexOfBegin = sourceString.IndexOf(beginString);
            if (indexOfBegin != -1)
            {
                //Incluir cadena de inicio
                if (includeBegin)
                {
                    indexOfBegin -= beginString.Length;
                }
                sourceString = sourceString.Substring(indexOfBegin + beginString.Length);
                int indexOfEnd = sourceString.IndexOf(endString);
                if (indexOfEnd != -1)
                {
                    //Incluir cadena de fin
                    if (includeEnd)
                    {
                        indexOfEnd += endString.Length;
                    }
                    result = sourceString.Substring(0, indexOfEnd);
                }
            }
            return result;
        }

        /// <summary>
        /// Determina si una cadena de texto coresponde a una direccion de correo electrónico válida.
        /// </summary>
        /// <param name="mail">Cadena de texto a validar.</param>
        /// <returns>true, si es una direccion de correo válida, de lo contrario false.</returns>
        public static bool IsValidEmail(string mail)
        {
            string mailRegex = @"^[A-Za-z]{1}[\w\.]+@(\[d{1,3}\.d{1,3}\.d{1,3}\.|(\w+\.)+)(\w{2,4})(\]{0,1})$";
            return Regex.IsMatch(mail ?? string.Empty, mailRegex);
        }

        /// <summary>
        /// Determina si una cadena de texto corresponde a un valor numérico.
        /// </summary>
        /// <param name="inputString">Cadena de texto a validar.</param>
        /// <returns>true, si el valor es numérico, de lo contrario false.</returns>
        public static bool IsNumeric(string inputString)
        {
            bool isNumeric = false;
            if (!string.IsNullOrEmpty(inputString))
            {
                isNumeric = inputString.ToCharArray().All(c => char.IsNumber(c));
            }
            return isNumeric;
        }


        /// <summary>
        /// Determina si una cadena de entrada es una clave segura.
        /// </summary>
        /// <param name="passwordValue">Valor de clave a validar.</param>
        /// <param name="passwordMinLength">Longitud de la clave.</param>
        /// <param name="errorMessage">Mensaje con el resultado de la validación.</param>
        /// <returns>true si la clave es segura, en caso contrario false.</returns>
        public static bool IsSafePassword(string passwordValue, int passwordMinLength, out string errorMessage)
        {
            int DEFAULT_PASSWORD_MIN_UPPER = 1;
            int DEFAULT_PASSWORD_MIN_LOWER = 0;
            int DEFAULT_PASSWORD_MIN_NUMBERS = 1;
            int DEFAULT_PASSWORD_MIN_SPECIAL_CHARS = 1;
            return IsSafePassword(passwordValue, passwordMinLength, DEFAULT_PASSWORD_MIN_UPPER, DEFAULT_PASSWORD_MIN_LOWER, DEFAULT_PASSWORD_MIN_NUMBERS, DEFAULT_PASSWORD_MIN_SPECIAL_CHARS, out errorMessage);
        }

        /// <summary>
        /// Determina si una cadena de entrada es una clave segura.
        /// </summary>
        /// <param name="passwordValue">Valor de clave a validar.</param>
        /// <param name="passwordMinLength">Longitud de la clave.</param>
        /// <param name="passwordMinUpper">Numero mínimo de mayúsculas que debe tener la clave.</param>
        /// <param name="passwordMinLower">Numero mínimo de minúsculas que debe tener la clave.</param>
        /// <param name="passwordMinNumbers">Numero mínimo de dígitos que debe tener la clave.</param>
        /// <param name="passwordMinSpecialChars">Numero mínimo de carácters especiales que debe tener la clave.</param>
        /// <param name="errorMessage">Mensaje con el resultado de la validación.</param>
        /// <returns>true si la clave es segura, en caso contrario false.</returns>
        public static bool IsSafePassword(string passwordValue, int passwordMinLength, int passwordMinUpper, int passwordMinLower, int passwordMinNumbers, int passwordMinSpecialChars, out string errorMessage)
        {
            errorMessage = string.Empty;
            StringBuilder sbErrorMessage = new StringBuilder();
            Regex upperRegex = new Regex("[A-Z]");
            Regex lowerRegex = new Regex("[a-z]");
            Regex numbersRegex = new Regex("[0-9]");
            Regex specialCharsRegex = new Regex("[^a-zA-Z0-9]");
            // Verifica la longitud de la contraseña.
            if (!string.IsNullOrEmpty(passwordValue))
            {
                if (passwordValue.Length < passwordMinLength)
                {
                    sbErrorMessage.AppendLine(string.Format("La clave debe contener mínimo {0} carácter(es).", passwordMinLength));
                }
                //Se validan mayusculas
                if (passwordMinUpper > 0)
                {
                    if (upperRegex.Matches(passwordValue).Count < passwordMinUpper)
                    {
                        sbErrorMessage.AppendLine(string.Format("La clave debe contener mínimo {0} mayúscula(s).", passwordMinUpper));
                    }
                }
                //Se validan minusculas
                if (passwordMinLower > 0)
                {
                    if (lowerRegex.Matches(passwordValue).Count < passwordMinLower)
                    {
                        sbErrorMessage.AppendLine(string.Format("La clave debe contener mínimo {0} minúscula(s).", passwordMinLower));
                    }
                }
                //Se validan numeros
                if (passwordMinNumbers > 0)
                {
                    if (numbersRegex.Matches(passwordValue).Count < passwordMinNumbers)
                    {
                        sbErrorMessage.AppendLine(string.Format("La clave debe contener mínimo {0} número(s).", passwordMinNumbers));
                    }
                }
                //Se validan caracteres especiales
                if (passwordMinSpecialChars > 0)
                {
                    if (specialCharsRegex.Matches(passwordValue).Count < passwordMinSpecialChars)
                    {
                        sbErrorMessage.AppendLine(string.Format("La clave debe contener mínimo {0} carácter(es) especial(es).", passwordMinSpecialChars));
                    }
                }
                errorMessage = sbErrorMessage.ToString();
            }
            return string.IsNullOrEmpty(errorMessage);
        }

        /// <summary>
        /// Permite generar un password de manera aleatoria.
        /// //TODO: Mejorar el algoritmo de generacion.
        /// </summary>
        /// <param name="passwordLength">Longitud del Password a generar</param>
        /// <returns></returns>
        public static string GenerateRandomPassword(int passwordLength)
        {
            string lowerLettersString = new string(Enumerable.Range(97, 26).Select(c => Convert.ToChar(c)).ToArray());
            string upperLettersString = lowerLettersString.ToUpper();
            string numberString = new string(Enumerable.Range(0, 10).Select(c => Convert.ToChar(c.ToString())).ToArray());
            string specialCharsString = "!@$?#";
            Byte[] randomBytes = new Byte[10];
            char[] chars = new char[10];
            Random randomObj = null;
            int allowedCharCount = lowerLettersString.Length;
            for (int i = 0; i < 3; i++)
            {
                randomObj = new Random((int)DateTime.UtcNow.Ticks);
                randomObj.NextBytes(randomBytes);
                chars[i] = lowerLettersString[(int)randomBytes[i] % allowedCharCount];
            }
            allowedCharCount = upperLettersString.Length;
            for (int i = 3; i < 6; i++)
            {
                randomObj = new Random((int)DateTime.UtcNow.Ticks);
                randomObj.NextBytes(randomBytes);
                chars[i] = upperLettersString[(int)randomBytes[i] % allowedCharCount];
            }
            allowedCharCount = numberString.Length;
            for (int i = 6; i < 9; i++)
            {
                randomObj = new Random((int)DateTime.UtcNow.Ticks);
                randomObj.NextBytes(randomBytes);
                chars[i] = numberString[(int)randomBytes[i] % allowedCharCount];
            }
            allowedCharCount = specialCharsString.Length;
            for (int i = 9; i < 10; i++)
            {
                randomObj = new Random();
                randomObj.NextBytes(randomBytes);
                chars[i] = specialCharsString[(int)randomBytes[i] % allowedCharCount];
            }
            return new string(chars);
        }

        /// <summary>
        /// Es un correo valido.
        /// </summary>
        /// <param name="ipAddress">Direccion.</param>
        /// <returns></returns>
        public static bool IsValidAddress(string ipAddress)
        {
            //Expresion regular que coincide con una IPv4
            string IP_V4_REGEX = @"(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})";
            return Regex.IsMatch(ipAddress, IP_V4_REGEX, RegexOptions.CultureInvariant);
        }

        /// <summary>
        /// Rellena una cadena de caracteres de longitud fija con un caracter de relleno específico
        /// y los datos de origen a rellenar indicados.
        /// </summary>
        /// <param name="fillChar">Carácter de relleno.</param>
        /// <param name="data">Cadena de origen a rellenar.</param>
        /// <param name="dataToFillLenght">Longitud de la cadena final.</param>
        /// <param name="fillLeft">Indicador de relleno ai inicio o al final de la cadena.</param>
        /// <returns></returns>
        public static string FillString(string fillChar, string data, int dataToFillLenght, bool fillLeft)
        {
            int fillCharCount = dataToFillLenght - data.Length;
            StringBuilder sbOutputString = new StringBuilder();
            string strFilled = string.Empty;
            for (int i = 0; i < fillCharCount; i++)
            {
                sbOutputString.Append(fillChar);
            }
            strFilled = fillLeft ? string.Concat(sbOutputString.ToString(), data) : string.Concat(data, sbOutputString.ToString());
            return strFilled;
        }

        /// <summary>
        /// Remueve los acentos de un texto.
        /// </summary>
        /// <param name="inputString">Valor a cambiar</param>
        /// <returns></returns>
        public static string RemoveChartAccents(string inputString)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(inputString))
            {
                StringBuilder sbInput = new StringBuilder(inputString);
                sbInput.Replace("á", "a");
                sbInput.Replace("Á", "A");
                sbInput.Replace("é", "e");
                sbInput.Replace("É", "E");
                sbInput.Replace("í", "i");
                sbInput.Replace("Í", "I");
                sbInput.Replace("ñ", "n");
                sbInput.Replace("Ñ", "N");
                sbInput.Replace("ó", "o");
                sbInput.Replace("Ó", "O");
                sbInput.Replace("ú", "u");
                sbInput.Replace("Ú", "U");
                sbInput.Replace("ü", "u");
                sbInput.Replace("Ü", "U");
                result = sbInput.ToString();
            } return result;
        }

        /// <summary>
        /// Reemplaza el un texto con los correspondientes valores de los acentos.
        /// </summary>
        /// <param name="inputString">Valor a cambiar.</param>
        /// <returns></returns>
        public static string ReplaceCharsWithHTMLCodes(string inputString)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(inputString))
            {
                StringBuilder sbInput = new StringBuilder(inputString);
                sbInput.Replace("á", "&aacute;");
                sbInput.Replace("Á", "&Aacute;");
                sbInput.Replace("é", "&eacute;");
                sbInput.Replace("É", "&Eacute;");
                sbInput.Replace("í", "&iacute;");
                sbInput.Replace("Í", "&Iacute;");
                sbInput.Replace("ñ", "&ntilde;");
                sbInput.Replace("Ñ", "&Ntilde;");
                sbInput.Replace("ó", "&oacute;");
                sbInput.Replace("Ó", "&Oacute;");
                sbInput.Replace("ú", "&uacute;");
                sbInput.Replace("Ú", "&Uacute;");
                sbInput.Replace("ü", "&uuml;");
                sbInput.Replace("Ü", "&Uuml;");
                sbInput.Replace("¿", "&iquest;");
                sbInput.Replace("¡", "&iexcl;");
                result = sbInput.ToString();
            } return result;
        }
    }
}
