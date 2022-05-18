using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Security.Principal;
using System.Text;
using System.Web;
using Iberdrola.Commons.Exceptions;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Utils;
using Iberdrola.Commons.Web;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.DAL.DB;

namespace Iberdrola.SMG.BLL
{
    public class ErroresCondicionesGenerales
    {
        public static ErrorCondicionGeneralDTO ObtenerError(Int32 codigo)
        {
            ErrorCondicionGeneralDB errorCondicionGeneralDb = new ErrorCondicionGeneralDB();

            ErrorCondicionGeneralDTO e = errorCondicionGeneralDb.ObtenerError(codigo);

            return e;
        }

    }
}
