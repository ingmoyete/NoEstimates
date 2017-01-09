using Microsoft.Practices.Unity;
using referenceArchitecture.Core.Exceptions;
using referenceArchitecture.Core.Helpers;
using referenceArchitecture.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace referenceArchitecture.ui.Core.WebViewPageBase
{
    /// <summary>
    /// Base web view page for strongly typed view.
    /// </summary>
    /// <typeparam name="T">Class</typeparam>
    public abstract class BaseWebViewPage<T> : WebViewPage<T>
    {
        /// <summary>
        /// Helper to get variables from app.config.
        /// </summary>
        private Ihp _hp = DependencyResolver.Current.GetService<Ihp>();
        public Ihp hp
        {
            get { return _hp; }
            set { _hp = value; }
        }

        /// <summary>
        /// Global Resources object to get strings from csv.
        /// </summary>
        private IResource globalResources = DependencyResolver.Current.GetService<IResource>();
        public IResource GlobalResources
        {
            get
            {
                globalResources.getResources(globalResources.GlobalResourceFileName);
                return globalResources;
            }
        }

        /// <summary>
        /// Local Resources for a page.
        /// </summary>
        public IResource LocalResources
        {
            get
            {
                var resources = ViewData[hp.getLocalResourceKey()] as IResource;
                if (resources == null) throw new ResourceInViewDataIsNull();
                return resources;
            }
        }
    }

    /// <summary>
    /// Base web view page fo non strongly typed view.
    /// </summary>
    public abstract class BaseWebViewPage : WebViewPage { }
}