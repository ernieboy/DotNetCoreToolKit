using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreToolKit.Library.Models.Common
{

    /// <summary>
    /// An OperationResult object is useful when you perform an operation and you need to return multiple pieces of data from the operation
    /// For example, you might need to return a list of errors that occur as part of invoking the operation. You could use this class when saving stuff into storage
    /// either locally in a database or remotely in web service calls. 
    /// Inspired by Deborah Kurata (https://www.pluralsight.com/authors/deborah-kurata).
    /// </summary>
    public class OperationResult
    {
        private List<string> _errorMessages;

        /// <summary>
        /// We initially set Success to True on construction because we are optimistic that the operation will succeed.  Always remember to set Success
        /// to False as soon as errors are encountered when invoking the operation. 
        /// </summary>
        public OperationResult()
        {
            Success = true;
            _errorMessages = new List<string>();
        }

        /// <summary>
        /// This flag communicates to the client whether the operation was a success or failure. It should be set to False if the operation failed. The
        /// client must always check this flag after invoking an operation which returns an OperationResult object in order to decide what to do next.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// List of messages to return to the client. You would normally populate this list with error messages which occur when performing the operation.
        /// The client should then use these messages to communicate with the user e.g. display the messages as UI errors. Note, these messages must be sanitised 
        /// before being displayed on the UI, for example, do not add stack traces here since they will reveal sensitive information about the underlying sysytem.
        /// Stack traces are useless to users as well.
        /// </summary>
        public IReadOnlyCollection<string> ErrorMessages => _errorMessages.Any() ? 
            new List<string>(_errorMessages) : (_errorMessages = new List<string>());

        /// <summary>
        /// Adds a message to the list of messages to return back to the client.
        /// </summary>
        /// <param name="errorMessage">The message to add</param>
        public void AddErrorMessage(string errorMessage)
        {
            if (errorMessage == null) return;
            _errorMessages.Add(errorMessage);
        }

    }
}
