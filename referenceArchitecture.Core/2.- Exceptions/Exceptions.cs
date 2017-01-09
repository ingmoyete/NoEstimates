using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.Core.Exceptions
{
    #region Service Exceptions
    /// <summary>
    /// Exceptions of the service layer.
    /// </summary>
    public class ServiceError : Exception
    {
        public ServiceError(string message) : base(message) { }
    }

    public class ContextIsNullBecauseTwoBusinessTransacionsCalls : ServiceError
    {
        public ContextIsNullBecauseTwoBusinessTransacionsCalls() : base("Context is null, call only one business transaction method per request. You are calling a second business transaction method (that uses an using statement) in the same request.") { }
    }



    #endregion  

    #region Core Exceptions
    /// <summary>
    /// Exeptions of the core layer.
    /// </summary>
    public class CoreError : Exception
    {
        public CoreError(string message) : base(message) { }
    }

    /// <summary>
    /// The parameter minutesInCache from app.config is invalid.
    /// </summary>
    public class InvalidValueInAppConfig : CoreError
    {
        public InvalidValueInAppConfig(string keyAppConfig) : base("The parameter {0} set in App.config is null, empty, or white space.") { }
    }

    /// <summary>
    /// The logger directory does not exist.
    /// </summary>
    public class LoggerDirectoryDoesNotExist : CoreError
    {
        public LoggerDirectoryDoesNotExist(string folderName) : base(string.Format("The logger directory folder {0} does not exist.", folderName)) { }
    }

    /// <summary>
    /// The resource file does not exist.
    /// </summary>
    public class ResourceNotExist : CoreError
    {
        public ResourceNotExist(string resourceName) : base(string.Format("The resource file {0} does not exist.", resourceName)) { }
    }

    /// <summary>
    /// Resource from viewData (filter is null).
    /// </summary>
    public class ResourceInViewDataIsNull : CoreError
    {
        public ResourceInViewDataIsNull() : base("The resource in viewData is null (check the filter and the BaseWebViewPage).") { }
    }

    /// <summary>
    /// The default value in app.config for "not found resource key" is null, empty or white space.
    /// </summary>
    public class InvalidDeafultMessageForNotFoundResourceKey : CoreError
    {
        public InvalidDeafultMessageForNotFoundResourceKey() : base("The default value in app.config for 'not found resource key' is null, empty or white space.") { }
    }

    /// <summary>
    /// The key of the resource does not exist.
    /// </summary>
    public class KeyOfResourceDoesNotExist : CoreError
    {
        public KeyOfResourceDoesNotExist(string keyName) : base(string.Format("The key {0} does not exist.", keyName)) { }
    }

    /// <summary>
    /// There is a null modelState in the MVCModelStateWrapper.
    /// </summary>
    public class NullModelStateInMVCWrapper : CoreError
    {
        public NullModelStateInMVCWrapper() : base("Se esta estableciendo ModelState nulo in the mvc wrapper.") { }
    }

    /// <summary>
    /// The controller registered by the service is null.
    /// </summary>
    public class ControllerPassedToServiceIsNull : CoreError
    {
        public ControllerPassedToServiceIsNull() : base("The controller registered by the service is null.") { }
    }
    #endregion

    #region Repository Exceptions
    /// <summary>
    /// Exceptions of the repository layer.
    /// </summary>
    public class RepositoryError : Exception
    {
        public RepositoryError(string message) : base(message) { }
    }
    /// <summary>
    /// Check if the id for a primary key is higher than zero.
    /// </summary>
    public class PrimaryKeyIdMustBeHigherThanZero : RepositoryError
    {
        public PrimaryKeyIdMustBeHigherThanZero() : base("Id (primary key) must be higher than 0.") { }
    }

    /// <summary>
    /// Check if the id for a foreign key is higher than zero.
    /// </summary>
    public class ForeignKeyIdMustBeHigherThanZero : RepositoryError
    {
        public ForeignKeyIdMustBeHigherThanZero() : base("Id (foreign key) must be higher than 0.") { }
    }
    /// <summary>
    /// Invalid id.
    /// </summary>
    public class IdDoesNotExistInDb : RepositoryError
    {
        public IdDoesNotExistInDb(string id, string table) : base(string.Format("The Id {0} does not exist in table {1}.", id, table)) { }
    }

    #endregion

    #region Presentation Exceptions
    /// <summary>
    /// Exceptions of the presentation layer.
    /// </summary>
    public class PresentationError : Exception
    {
        public PresentationError(string message) : base(message) { }
    }

    /// <summary>
    /// Error view could not be reached from app.config.
    /// </summary>
    public class ErrorViewPathNotFound : PresentationError
    {
        public ErrorViewPathNotFound() : base("Error view could not be reached. Check the value in app.config") { }
    }
}
    #endregion
