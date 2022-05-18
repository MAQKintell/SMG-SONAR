using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;

namespace Iberdrola.Commons.Web
{
    /// <summary>
    /// Descripción breve de NavigationController
    /// </summary>
    public class NavigationController
    {
        public static String PREVIOUS_URL_LIST_KEY = "PREVIOUS_URL_LIST_KEY";
        public static String PREVIOUS_FORM_TYPE_KEY = "PREVIOUS_FORM_TYPE_KEY";
        public static String PREVIOUS_FORM_KEY = "PREVIOUS_FORM_KEY";
        public static String IS_BACKWARD_NAME = "IS_BACKWARD";
        public static String IS_BACKWARD_VALUE = "1";

        
        private NavigationController()
        {

        }

        
        //public static void Forward(string url, Iberdrola.Commons.Web.FrmBase form)
        //{
        
        //    string currentUrl = HttpContext.Current.Request.Url.ToString();

        //    CurrentSession.SetMandatoryAttribute(PREVIOUS_URL_KEY, currentUrl);
        //    CurrentSession.SetMandatoryAttribute(PREVIOUS_FORM_TYPE_KEY, form.GetType());
        //    CurrentSession.SetMandatoryAttribute(PREVIOUS_FORM_KEY, form);

        //    HttpContext.Current.Response.Redirect(url);
        //}

        //public static void Forward(string url, object controlState)
        //{

        //    string currentUrl = HttpContext.Current.Request.Url.ToString();

        //    CurrentSession.SetMandatoryAttribute(PREVIOUS_URL_KEY, currentUrl);
        //    //CurrentSession.SetMandatoryAttribute(PREVIOUS_FORM_TYPE_KEY, form.GetType());
        //    CurrentSession.SetMandatoryAttribute(PREVIOUS_FORM_KEY, controlState);

        //    HttpContext.Current.Response.Redirect(url);
        //}


        public static void Forward(string url, string currentUrl)
        {
            List<String> urlList = (List<String>)CurrentSession.GetAttribute(PREVIOUS_URL_LIST_KEY);
            if (urlList == null)
            {
                urlList = new List<String>();
            }
            urlList.Add(currentUrl);

            CurrentSession.SetAttribute(PREVIOUS_URL_LIST_KEY, urlList);
            HttpContext.Current.Response.Redirect(url, false);

        }

        public static void Forward(string url)
        {
            Forward(url, HttpContext.Current.Request.Url.ToString());
        }

        public static Boolean CanBackward()
        {
            List<String> urlList = (List<String>)CurrentSession.GetAttribute(PREVIOUS_URL_LIST_KEY);
            if (urlList != null && urlList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Backward()
        {
            List<String> urlList = (List<String>) CurrentSession.GetAttribute(PREVIOUS_URL_LIST_KEY);
            if (urlList != null)
            {
                String previousUrl = urlList[urlList.Count - 1];
                urlList.RemoveAt(urlList.Count - 1);
                CurrentSession.SetAttribute(PREVIOUS_URL_LIST_KEY, urlList);

                if (previousUrl != null)
                {
                    String strQuestionQuote = "?";
                    if (previousUrl.IndexOf("?") != -1)
                    {
                        strQuestionQuote = "&";
                    }

                    if (previousUrl.Contains(IS_BACKWARD_NAME) == false)
                    {
                        previousUrl += strQuestionQuote + IS_BACKWARD_NAME + "=" + IS_BACKWARD_VALUE;
                    }
                                        
                    HttpContext.Current.Response.Redirect(previousUrl, false);
                 }
            }
        }

        public static void ClearHistory()
        {
            List<String> urlList = (List<String>)CurrentSession.GetAttribute(PREVIOUS_URL_LIST_KEY);
            if (urlList != null)
            {
                urlList.Clear();
                CurrentSession.SetAttribute(PREVIOUS_URL_LIST_KEY, urlList);
            }
        }

        public static Boolean IsBackward()
        {
            String strIsBackward = HttpContext.Current.Request.QueryString[IS_BACKWARD_NAME];
            if (IS_BACKWARD_VALUE.Equals(strIsBackward))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
