using System.Security.Principal;

namespace Iberdrola.Commons.Security
{
    /// <summary>
    /// AppPrincipal
    /// </summary>
    public class AppPrincipal : GenericPrincipal
    {
        /// <summary>
        ///     
        /// </summary>
        /// <param name="identity" type="System.Security.Principal.IIdentity">
        ///     <para>
        ///         
        ///     </para>
        /// </param>
        /// <param name="roles" type="string[]">
        ///     <para>
        ///         
        ///     </para>
        /// </param>
        /// <returns>
        ///     A void value...
        /// </returns>
        public AppPrincipal(IIdentity identity, string[] roles)
            : base(identity, roles)
        {
        }

        private bool _isSecurePassword = true;

        /// <summary>
        /// Indica si la password del usuario es una password que cumple con los requisitos
        /// de seguridad
        /// </summary>
        public bool IsSecurePassword
        {
            get { return _isSecurePassword; }
            set { _isSecurePassword = value; }
        }
    }
}