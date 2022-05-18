using System;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections;

namespace Iberdrola.Commons.Security
{
    public class PasswordUtils
    { 
        public const int PASSWORD_LENGHT = 6;
        public const int PASSWORD_NUMERIC_CHAR_COUNT = 2;
        public const int PASSWORD_UPPER_CASE_CHAR_COUNT = 1;
        public const int PASSWORD_LOWER_CASE_CHAR_COUNT = 1;
        public const int PASSWORD_MAX_COINCIDENCT_CHARS = 4;
        public const int PASSWORD_MAX_NON_ALPHANUMERIC_CHARS = 0;
        
        public static string GeneratePassword(int passwordLength, int numericCharCount)
        {            
            ArrayList arrCharPool = new ArrayList();
            Random rndNum = new Random();
            arrCharPool.Clear();
            string password = "";

            //Lower Case 
            for (int i = 97; i < 123; i++)
            {
                arrCharPool.Add(Convert.ToChar(i).ToString());
            }
             //Number
            for (int i = 48; i < 58; i++)
            {
                arrCharPool.Add(Convert.ToChar(i).ToString());
            }

             //Upper Case 
            for (int i = 65; i < 91; i++)
            {
                arrCharPool.Add(Convert.ToChar(i).ToString());
            }

            //Para saber si temenos los números que se nos piden.
            int actualNumericCharCount = 0;
            //Iterate through the number of characters required in the password
            for (int i = 0; i < passwordLength; i++)
            {
                int rndNumNext = rndNum.Next(arrCharPool.Count);

                // Comprobamos si el número obtenido corresponde a un caracter numérico
                if (rndNumNext >= 26 && rndNumNext < 36)
                {
                    numericCharCount++;
                }

                // Si estamos en el último los últimos caracteres y no vamos
                // a cumplir con el número de Números obligatorios
                // forzamos el random a que lo coja de las posiciones de los números.
                if (passwordLength - i == numericCharCount - actualNumericCharCount)
                {
                    rndNumNext = rndNum.Next(26, 36);
                    password += arrCharPool[rndNumNext].ToString();
                    numericCharCount++;
                }
                else
                {
                    password += arrCharPool[rndNumNext].ToString();
                }
            }
        return password;
        }

        /// <summary>
        /// Comprueba que la password vieja no coincida en más de 
        /// </summary>
        /// <param name="strOldPass"></param>
        /// <param name="strNewPass"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string strOldPass, string strNewPass)
        { 
        
            ArrayList alOldPass = new ArrayList(strOldPass.ToCharArray());
            ArrayList alNewPass = new ArrayList(strNewPass.ToCharArray());

       
            // Comprobamos que la contraseña nueva no coincida en X letras con la anterior.
            int intCountCoincidencias = 0;
            foreach (char c in alNewPass)
            {
                if (alOldPass.Contains(c))
                {
                    intCountCoincidencias++;
                }                
            }

            if (intCountCoincidencias > PASSWORD_MAX_COINCIDENCT_CHARS)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Comprueba que en la password haya al menos numericCharCount caracteres numéricos.
        /// </summary>
        /// <param name="strNewPass"></param>
        /// <param name="numericCharCount"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string strNewPass, int numericCharCount)
        {
            ArrayList alNewPass = new ArrayList(strNewPass.ToCharArray());

            // Comprobamos que al menos tenga los números que se indican.
            int intNumericCount = 0;
            foreach (char c in alNewPass)
            {
                int intValor;
                if (int.TryParse(c.ToString(), out intValor))
                {
                    intNumericCount++;
                }
            }

            if (numericCharCount > intNumericCount)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Comprueba que en la password haya al menos numericCharCount caracteres numéricos.
        /// </summary>
        /// <param name="strNewPass"></param>
        /// <param name="numericCharCount"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string strNewPass, int upperCaseCharCount, int lowerCaseCharCount)
        {
            ArrayList alNewPass = new ArrayList(strNewPass.ToCharArray());

            int intUpperCount = 0;
            int intLowerCount = 0;
            
            // Contamos el número de mayúsculas y minúsculas.            
            foreach (char c in alNewPass)
            {
                // Sólo contamos si no es número.
                int intValor;
                if (!int.TryParse(c.ToString(), out intValor))
                {
                    if (Char.IsUpper(c))
                    {
                        intUpperCount++;
                    }
                    else if (Char.IsLower(c))
                    { 
                        intLowerCount++;
                    }
                }
            }

            return (intUpperCount >= upperCaseCharCount && intLowerCount >= lowerCaseCharCount);
        }


        /// <summary>
        /// Comprueba que en la password no haya más de nonAlphanumericCharCount caracteres no alfanumericos.
        /// </summary>
        /// <param name="strNewPass"></param>
        /// <param name="numericCharCount"></param>
        /// <returns></returns>
        public static bool ValidatePasswordAlphanumeric(string strNewPass, int nonAlphanumericCharCount)
        {
            ArrayList alNewPass = new ArrayList(strNewPass.ToCharArray());
            int intNonAlphanumericCount = 0;
            
            // Contamos el número de mayúsculas y minúsculas.            
            foreach (char c in alNewPass)
            {
                if (!Char.IsLetterOrDigit(c))
                {
                    intNonAlphanumericCount++;
                }            
            }

            return (nonAlphanumericCharCount >= intNonAlphanumericCount);
        }
    }
}