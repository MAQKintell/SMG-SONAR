using System;
using System.Web;
using Iberdrola.Commons.Web;
//using Iberdrola.SMG.BLL.Process;
using System.Collections.Generic;
using Iberdrola.SMG.DAL.DTO;
using Iberdrola.SMG.BLL;
using Iberdrola.Commons.Security;
using Iberdrola.Commons.Configuration;

namespace Iberdrola.SMG.UI
{
    public partial class FrmUtilidadesAdministracion : FrmBase
    {        
        #region implementación de los eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                }
            }
            catch (Exception ex)
            {
                this.ManageException(ex);
            }
        }
        #endregion

        #region implementación métodos abstractos
        public override void LoadSessionData()
        {
            // coger los valores de sessión

            // cargar los valores en el formulario

            // eliminar de sesión los valores del formulario
        }

        public override void SaveSessionData()
        {
            // eliminar de sesión los valores del formulario si existían

            // coger los valores en el formulario

            // cargar los valores en sessión
        }
        
        public override void DeleteSessionData()
        {
            // eliminar de sesión los valores del formulario si existían
        }
        #endregion

       
        protected void OnBtnEncriptarPasswords_Click(object sender, EventArgs e)
        {
            try
            {
                List<UsuarioDTO> listaUsuarios = Usuarios.ObtenerUsuarios();
                foreach (UsuarioDTO u in listaUsuarios)
                {
                    try
                    {
                        string pass = EncryptHelper.Decrypt(u.Password);
                    }
                    catch(Exception ex)
                    {
                        // si salta la excepción es porque no estaba encriptado
                        // entonces modificamos la password del usuario y la guardamos
                        u.Password = EncryptHelper.Encrypt(u.Password);
                        Usuarios.CambiarPassword(u.Login, u.Password, Usuarios.ObtenerUsuarioLogeado().Login); 
                    }
                }
                //this.ShowMessage("Proceso Terminado");
            }
            catch (Exception ex)
            {

                //this.ManageException(ex);
            }
        }

        protected void OnBtnDesencriptarPasswords_Click(object sender, EventArgs e)
        {
            try
            {
                List<UsuarioDTO> listaUsuarios = Usuarios.ObtenerUsuarios();
                foreach (UsuarioDTO u in listaUsuarios)
                {
                    try
                    {
                        string pass = EncryptHelper.Decrypt(u.Password, true);

                        // Si no da error al desencriptar entonces es que estaba encryptada
                        u.Password = pass;
                        Usuarios.CambiarPassword(u.Login, u.Password, Usuarios.ObtenerUsuarioLogeado().Login);
                    }
                    catch (Exception ex)
                    {
                        // si salta la excepción es porque no estaba encriptado
                        // entonces no hacemos nada porque ya estaba desencriptada                        
                    }
                }
                //this.ShowMessage("Proceso Terminado");
            }
            catch (Exception ex)
            {

                //this.ManageException(ex);
            }
        }

     
        protected void OnBtnVolver_Click(object sender, EventArgs e)
        {
            try
            {
                this.DeleteSessionData();
                NavigationController.Backward();
            }
            catch (Exception ex)
            {
               // ManageException(ex);
            }
        }

        protected void OnBtnRefrescarConfiguracionColumnas_Click(object sender, EventArgs e)
        {
            try
            {
                DefinicionColumnas.Refresh();
            }
            catch (Exception ex)
            {
               // ManageException(ex);
            }
        }
        
    }
}
