using System;
using System.Data;

namespace KM.Common.Import
{
    internal class ErrorArgs
    {
        public ErrorArgs(
            DataTable errorTable,
            Exception error,
            string clientMessage,
            string[] stringRow = null,
            int? rowNumber = null,
            bool? fatalError = null)
        {
            if (errorTable == null)
            {
                throw new ArgumentNullException(nameof(errorTable));
            }

            ErrorTable = errorTable;
            Error = error;
            ClientMessage = clientMessage;
            StringRow = stringRow;
            RowNumber = rowNumber;
            FatalError = fatalError;
        }

        public ErrorArgs(
            DataTable errorTable,
            string formattedError,
            string clientMessage,
            string[] stringRow = null,
            int? rowNumber = null,
            bool? fatalError = null)
        {
            if (errorTable == null)
            {
                throw new ArgumentNullException(nameof(errorTable));
            }

            ErrorTable = errorTable;
            FormattedError = formattedError;
            ClientMessage = clientMessage;
            StringRow = stringRow;
            RowNumber = rowNumber;
            FatalError = fatalError;
        }

        public DataTable ErrorTable { get; private set; }
        public Exception Error { get; private set; }
        public string FormattedError { get; private set; }
        public string ClientMessage { get; private set; }
        public string[] StringRow { get; private set; }
        public int? RowNumber { get; private set; }
        public bool? FatalError { get; private set; }
    }
}
