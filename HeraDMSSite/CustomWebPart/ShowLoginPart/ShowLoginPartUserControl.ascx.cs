using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.Office.SecureStoreService.Server;
using Microsoft.BusinessData.Infrastructure.SecureStore;
using System.Security;

namespace HeraDMS.CustomWebPart.ShowLoginPart
{
    public partial class ShowLoginPartUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCurrentUserInfo();
                //Page.ClientScript.re(this.GetType(), "js", "activeDemo.SetUserAccount(\"" + user + "\",\"" + user + "\");", true);
            }
        }

        /// <summary>
        /// 从SSS中获取用户账户和密码
        /// </summary>
        private void GetCurrentUserInfo() 
        {
            string m_userName = string.Empty; 
            string m_password = string.Empty;
            string m_appId = "ShippingID"; 
            SecureStoreProvider m_provider = new SecureStoreProvider(); 
            SPSite m_site = SPContext.Current.Site;
            SPServiceContext m_serviceContext = SPServiceContext.GetContext(m_site); 
            m_provider.Context = m_serviceContext; 
            try { 
                SecureStoreCredentialCollection m_sscc = m_provider.GetCredentials(m_appId);
                foreach (SecureStoreCredential ssc in m_sscc) 
                { 
                    switch (ssc.CredentialType)
                    { 
                        case SecureStoreCredentialType.Generic:                            
                            break; 
                        case SecureStoreCredentialType.Key:                             
                            break; 
                        case SecureStoreCredentialType.Password: 
                            m_password = ToClrString(ssc.Credential); 
                            break; 
                        case SecureStoreCredentialType.Pin:                            
                            break; 
                        case SecureStoreCredentialType.UserName:                    
                            m_userName = ToClrString(ssc.Credential);
                            break;
                        case SecureStoreCredentialType.WindowsPassword: 
                            break; 
                        case SecureStoreCredentialType.WindowsUserName:   
                            break;
                        default:                            
                            break; 
                    } 
                } 
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "js", "alert('该用户未在SSS里维护');", true);
            }
            this.hfUserName.Value = m_userName;
            this.hfPsd.Value = m_password;
        }

        internal string ToClrString(SecureString p_string) 
        { 
            var m_ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(p_string); 
            try 
            { 
                return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(m_ptr); 
            } finally { 
                System.Runtime.InteropServices.Marshal.FreeBSTR(m_ptr);
            }
        } 
    }
}
