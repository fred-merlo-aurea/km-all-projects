using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADMS.Events;

namespace AutomatedDataMigrationService.Services.Transformer
{
    public interface ITransformer
    {
        /// <summary>
        /// Data Row has been validated
        /// </summary>
        /// <param name="eventMessage"></param>
        void HandleRowValidated(RowValidated eventMessage);
        event Action<FileTransformed> FileTransformed;
    }
}
