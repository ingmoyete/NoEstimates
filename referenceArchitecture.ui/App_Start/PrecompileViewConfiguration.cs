using InteractivePreGeneratedViews;
using NoEstimates.repository.Core.ChangeDb;
using NoEstimates.repository.Edmx.BaseContextAndPartialClasses;
using referenceArchitecture.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoEstimates.ui.App_Start
{
    public static class PrecompileViewConfiguration
    {
        public static void RegisterPrecompileViews()
        {
            // Precompile views for entityframework
            var hp = DependencyResolver.Current.GetService<Ihp>();
            var changeDb = DependencyResolver.Current.GetService<IChangeDbConnection>();
            bool usePrecomplieViews = hp.getStringFromAppConfig("usingPrecompileViews") == "true";

            // Set configuration
            using (var context = new BaseContext(changeDb))
            {
                if (usePrecomplieViews)
                {
                    // Build route from separated commas values in app.config
                    var path = hp.getPathFromSeparatedCommaValue("preCompileViewEFInAppData");

                    InteractiveViews.SetViewCacheFactory
                    (
                        context,
                        new FileViewCacheFactory(path)
                    );
                }
            }
        }
    }
}