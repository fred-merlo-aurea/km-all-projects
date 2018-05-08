using System;
using ADMS.Services.Emailer;
using ADMS.Services.FileMover;
using ADMS.Services.FileWatcher;
using ADMS.Services.UAD;
using ADMS.Services.Validator;
using ADMS.Services.DataCleanser;
using System.Collections.Generic;
using Ninject;

namespace ADMS
{
    public class BillTurner : IDisposable
    {
        public static Dictionary<int, FrameworkUAS.Object.ClientAdditionalProperties> ClientAdditionalProperties
        {
            get;
            set;
        }
        private StandardKernel _kernel;
        #region Subscriber / Publisher - NOT Used for now
        //private FileDetectedPublisher _fileUploadedPublisher;
        //private FileDetectedSubscriber _fileUploadedSubscriber;
        //private FileProcessedPublisher _fileProcessedPublisher;
        //private FileProcessedSubscriber _fileProcessedSubscriber;
        //private FileMovedPublisher _fileMovedPublisher;
        //private FileMovedSubscriber _fileMovedSubscriber;
        //private RowValidatedPublisher _rowValidatedPublisher;
        //private RowValidatedSubscriber _rowValidatedSubscriber;
        #endregion

        private IFileWatcher _fileWatcher;
        private IFileMover _fileMover;
        private IValidator _validator;
        private IEmailer _email;       
        private IDQMCleaner _dqmCleaner;
        private IUADProcessor _uadProcessor;
        private IAddressClean _addressClean;

        public void Initialize()
        {
            try
            {
                ClientAdditionalProperties = new Dictionary<int, FrameworkUAS.Object.ClientAdditionalProperties>();
            }
            catch (Exception ex)
            {
                LogError(ex, "ClientAdditionalProperties");
            }
            try
            {
                _kernel = new StandardKernel();
            }
            catch(Exception ex)
            { LogError(ex, "_kernel"); }
            try { 
                configureDependencyInjection();
            }
            catch (Exception ex)
            { LogError(ex, "configureDependencyInjection"); }
            try
            {
                initServices();
            }
            catch (Exception ex)
            { LogError(ex, "initServices"); }
            try { 
            bindEvents();
            }
            catch (Exception ex)
            { LogError(ex, "bindEvents"); }
            activateServices();
            
        }
        public void LogError(Exception ex,string msg)
        {
            #region Log Error
            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);

            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
            if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);

            KMPlatform.BusinessLogic.ApplicationLog appLogWorker = new KMPlatform.BusinessLogic.ApplicationLog();
            appLogWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".LogError", app, msg);
            #endregion
        }
        private void configureDependencyInjection()
        {
            _kernel.Bind<IFileWatcher>().To<FileWatcher>();
            _kernel.Bind<IFileMover>().To<FileMover>();
            _kernel.Bind<IValidator>().To<Validator>();
            _kernel.Bind<IEmailer>().To<Emailer>();

            _kernel.Bind<IAddressClean>().To<AddressClean>();
            _kernel.Bind<IProfileMatch>().To<ProfileMatch>();
            _kernel.Bind<IDQMCleaner>().To<DQMCleaner>();
            _kernel.Bind<IUADProcessor>().To<UADProcessor>();
        }

        private void bindEvents()
        {
            //The way we're going to do it for now.
            _fileWatcher.FileDetected += _fileMover.HandleFileDetected;
            _fileWatcher.FileProcessed += _email.HandleFileProcessed;

            _fileMover.FileMoved += _validator.HandleFileMoved;

            _validator.FileValidated += _fileMover.HandleFileValidated;
            _validator.FileValidated += _addressClean.HandleFileValidated;//_dqmCleaner
            _validator.CustomFileProcessed += _email.HandleCustomFileProcessed;
            _validator.CustomFileProcessed += _fileMover.HandleCustomFileProcessed;
            

            _dqmCleaner.FileCleansed += _uadProcessor.HandleFileCleansed;
            _dqmCleaner.FileProcessed += _email.HandleFileProcessed;

            _uadProcessor.FileProcessed += _email.HandleFileProcessed;

            _addressClean.FileAddressGeocoded += _dqmCleaner.HandleFileAddressGeocoded;
            _addressClean.FileProcessed += _email.HandleFileProcessed;
        }

        private void initServices()
        {
            _fileWatcher = _kernel.Get<IFileWatcher>();
            _fileMover = _kernel.Get<IFileMover>();
            _validator = _kernel.Get<IValidator>();
            _email = _kernel.Get<IEmailer>();
            
            _dqmCleaner = _kernel.Get<IDQMCleaner>(); 
            _addressClean = _kernel.Get<IAddressClean>();
            //_profileMatch = _kernel.Get<IProfileMatch>();

            _uadProcessor = _kernel.Get<IUADProcessor>();
            
        }

        private void activateServices()
        {
            _fileWatcher.Activate();
            //Used For Daily Email Sends
            //_email.Activate();
        }
        public void ReactivateWatcher()
        {
            _fileWatcher.Activate(true);
            // _fileWatcher.CheckForFiles(); 
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _kernel.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        

    }
}
