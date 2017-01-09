using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using referenceArchitecture.service.Core.Base.BaseController;
using referenceArchitecture.Core.Exceptions;
using referenceArchitecture.Core.Resources;
using System.Web.Mvc;
using referenceArchitecture.repository.Edmx.Interfaces;
using referenceArchitecture.Core.Helpers;

namespace NoEstimates.service.Core.Base
{
    public class BaseService : IBaseService
    {
        //todo ARCHITECTURE add resources and dbcontext properties and move base to service layer.
        /// <summary>
        /// Controller UI
        /// </summary>
        private IControllerUI controllerUI;

        /// <summary>
        /// Controller UI
        /// </summary>
        public IControllerUI ControllerUI
        {
            get
            {
                return controllerUI;
            }
        }
        
        /// <summary>
        /// Key for the summaryError in modelState.
        /// </summary>
        public string SummaryError
        {
            get
            {
                return "SummaryError";
            }
        }

        /// <summary>
        /// Register a controller.
        /// </summary>
        /// <param name="_controllerUI">Controller to be registered.</param>
        public void register(IControllerUI _controllerUI)
        {
            if (_controllerUI == null) throw new ControllerPassedToServiceIsNull();
            this.controllerUI = _controllerUI;
        }

        /// <summary>
        /// Global resources instance.
        /// </summary>
        private IResource globalResources = DependencyResolver.Current.GetService<IResource>();

        /// <summary>
        /// Global resources property.
        /// </summary>
        public IResource GlobalResources
        {
            get
            {
                globalResources.getResources(globalResources.GlobalResourceFileName);
                return globalResources;
            }

            set { globalResources = value; }
        }

        /// <summary>
        /// Db context instance.
        /// </summary>
        private IDbContext dbContext = DependencyResolver.Current.GetService<IDbContext>();
        /// <summary>
        /// DbContext property.
        /// </summary>
        public IDbContext DbContext
        {
            get
            {
                return dbContext;
            }

            set { dbContext = value; }
        }

        /// <summary>
        /// Helper
        /// </summary>
        private Ihp _hp = DependencyResolver.Current.GetService<Ihp>();
        public Ihp Hp
        {
            get { return _hp; }
            set { _hp = value; }
        }
    }
}
