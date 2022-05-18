using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iberdrola.Commons.Validations
{
    /// <summary>
    /// Descripción breve de PhoneValidator
    /// </summary>
    public class PhoneValidator
    {
        public PhoneValidator()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        public bool Validate(string value)
        {
            bool result = true;
            try
            {
                int longNumero = value.Length;
                string digit0 = value.Substring(0, 1);
                string digit1 = value.Substring(1, 1);
                string digit2 = value.Substring(2, 1);
                string digit3 = value.Substring(3, 1);
                string digit4 = value.Substring(4, 1);
                string digit5 = value.Substring(5, 1);

                //La primera posicion debería ser el '+'
                if (digit0 != "+")
                {
                    result = false;
                }
                else
                { 
                    switch (digit1)
                    {
                        case "3":
                            switch (digit2)
                            {
                                case "3":
                                    // FRANCIA
                                    if (longNumero != 13)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        if (digit3 != "0")
                                        {
                                            result = false;
                                        }
                                        //else
                                        //{
                                        //    if (digit4 == "6" || digit4 == "7") MOVIL else FIJO
                                        //}
                                    }
                                    break;
                                case "4":
                                    // ESPAÑA
                                    if (longNumero != 12)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        if (digit3 == "8" || digit3 == "9")
                                        {
                                            //FIJO
                                        }
                                        else if (digit3 == "6" || digit3 == "7")
                                        {
                                            //MOVIL
                                        }
                                        else
                                        {
                                            result = false;
                                        }
                                    }
                                    break;
                                case "5":
                                    switch (digit3)
                                    {
                                        case "1":
                                            // PORTUGAL
                                            if (longNumero != 13)
                                            {
                                                result = false;
                                            }
                                            else
                                            {
                                                if (digit4 == "2" || digit4 == "3")
                                                {
                                                    //FIJO
                                                }
                                                else if (digit4 == "9" && (digit5 == "6" || digit5 == "3" || digit5 == "2" || digit5 == "1"))
                                                {
                                                    //MOVIL
                                                }
                                                else
                                                {
                                                    result = false;
                                                }
                                            }
                                            break;
                                        case "3":
                                            // IRLANDA
                                            if (longNumero < 11 || longNumero > 15)
                                            {
                                                result = false;
                                            }
                                            else
                                            {
                                                if (digit4 != "0")
                                                {
                                                    result = false;
                                                }
                                                else if (digit5 == "8" && longNumero==14)
                                                {
                                                    //MOVIL
                                                }
                                                else
                                                {
                                                    result = false;
                                                }
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case "9":
                                    // ITALIA
                                    if (longNumero < 9 || longNumero > 14)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        if (digit3 == "0")
                                        {
                                            //FIJO
                                        }
                                        else if (digit3 == "3" && (longNumero==12 || longNumero==13))
                                        {
                                            //MOVIL
                                        }
                                        else
                                        {
                                            result = false;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "4":
                            switch (digit2)
                            {
                                case "1":
                                    // SUIZA
                                    if (longNumero != 12)
                                    {
                                        result = false;
                                    }
                                    //else
                                    //{
                                    //    if (digit3 + digit4 == cod_provincia) FIJO else if (digit3 == "7") MOVIL
                                    //}
                                    break;
                                case "4":
                                    // REINO UNIDO
                                    if (longNumero != 13)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        if (digit3 == "1" || digit3 == "2")
                                        {
                                            //FIJO
                                        }
                                        else if (digit3 == "7")
                                        {
                                            //MOVIL
                                        }
                                        else
                                        {
                                            result = false;
                                        }
                                    }
                                    break;
                                case "9":
                                    // ALEMANIA
                                    if (longNumero < 9 || longNumero > 18)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        if (digit3 == "0")
                                        {
                                            //FIJO
                                        }
                                        else if (digit4 == "0" && digit4 == "1" && (digit5 == "5" || digit5 == "6" || digit5 == "7")) //Por aqui no pasaría nunca, porque la anterior validacion la engloba
                                        {
                                            //MOVIL longNumero>14
                                        }
                                        else
                                        {
                                            result = false;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}