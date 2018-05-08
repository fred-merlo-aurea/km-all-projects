using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECN_EngineFramework
{
    [Flags]
    public enum LogTarget
    {
        Console = 1,
        File = 2,
        All = 3
    }

    public enum LogLevel
    {
        Normal,
        Verbose,
        Debug
    }

    abstract public class ThreadingConsoleEngine<T> : IDisposable
        //where T : IEngineTaskContainer
        //where ConfigurationT : Abstract.IEngineConfiguration, new()
    {
        public uint IterationCount { get; private set; }

        #region logging

        LogLevel LogMessagesAtLevel = LogLevel.Normal;

        object logStreamMutex = new Object();
        public StreamWriter LogStreamWriter { get; private set; }

        public void Log(LogLevel level, LogTarget target, string message)
        {
            if ((int)level > (int)LogMessagesAtLevel)
            {
                return;
            }

            string logMessage = String.Format("[{0}] {1}", DateTime.Now, message);         
            if ((target & LogTarget.Console) == LogTarget.Console)
            {
                Console.WriteLine(logMessage);
            }
            lock (logStreamMutex)
            {
                if ((target & LogTarget.File) == LogTarget.File)
                {
                    LogStreamWriter.WriteLine(logMessage);
                }
            }
        }

        public void Log(LogLevel level, LogTarget target, string format, params object [] args)
        { 
            Log(level, target, String.Format(format, args));
        }

        public void Log(LogTarget target, string format, params object[] args)
        {
            Log(LogLevel.Normal, target, String.Format(format, args));
        }

        public void Log(LogLevel level, string format, params object[] args)
        {
            Log(level, LogTarget.All, String.Format(format, args));
        }

        public void Log(string format, params object[] args)
        {
            Log(LogLevel.Normal, LogTarget.All, String.Format(format, args));
        }

        public void Log(LogLevel level, string message)
        {
            Log(level, LogTarget.All, message);
        }

        public void Log(LogTarget target, string message)
        {
            Log(LogLevel.Normal, target, message);
        }

        public void Log(string message)
        {
            Log(LogLevel.Normal, LogTarget.All, message);
        }

        #endregion logging
        #region events

        public class EngineEventArgs
        {
            public EngineEventType EventType { get; set; }
            public Task Task { get; set; }
            public T TaskData { get; set; }
            public Exception Exception { get; set; }

            public override string ToString()
            {
                return TaskData.ToString() + (null == Exception ? "" : ", Exception: " + Exception.ToString());
            }
        }

        public delegate EngineActionResult EngineTask(EngineEventArgs eventArgs);

        protected virtual event EventHandler<EngineEventArgs> OnEngineInitialized;
        protected virtual event EventHandler<EngineEventArgs> OnIterationStarting;
        protected virtual event EventHandler<EngineEventArgs> OnTaskSuccessful;
        protected virtual event EventHandler<EngineEventArgs> OnTaskFailed;
        protected virtual event EventHandler<EngineEventArgs> OnIterationFinished;
        protected virtual event EventHandler<EngineEventArgs> OnEngineRestarted;
        protected virtual event EventHandler<EngineEventArgs> OnEngineDisposing;

        protected virtual void FireEngineEvent(EventHandler<EngineEventArgs> eventHandler, EngineEventArgs eventArgs)
        {
            if (null != eventHandler)
            {
                eventHandler(this, eventArgs);
            }
        }
        protected void FireEngineEvent(EventHandler<EngineEventArgs> eventHandler, EngineEventType eventType, T task = default(T))
        {
            FireEngineEvent(eventHandler, new EngineEventArgs { EventType = eventType, TaskData = task });
        }

        #endregion events
        #region constructors

        protected ThreadingConsoleEngine()
        {
            //Console.WriteLine("reached Engine constructor");
            // wire-up any event handlers here
        }

        #endregion constructors
        #region Helpers

        public void SendCriticalError(Exception exception, object detail)
        {
            SendCriticalError(exception, detail.ToString());
        }
        public void SendCriticalError(Exception exception, string detail) 
        {
            KM.Common.Entity.ApplicationLog.LogCriticalError(exception, AppConfig.Get.WarningFromName, AppConfig<int>.Get.KMCommon_Application, detail);
        }

        public void SendWarning(string shortDescription, string message, [CallerMemberName] string issueWithMethodName="")
        {
            string toEmailAddress = AppConfig.Get.WarningToEmailAddress;
            //string toEmailAddress = "corwin.brust@teamkm.com";

            string application         = AppConfig.Get.WarningSourceApplication;
            string fromName            = AppConfig.Get.WarningFromName;            
            string replyToEmailAddress = EmailDirectHelper.DefaultReplyToAddress;
            string subject             = String.Format("{0} Warning: {1}", application, shortDescription);
            string body = 
                String.Format(@"<html>
<head></head>
<body>
    <h1>Warning: <pre>{0}</pre></h1>
    <h2>Source: {1}.{2}</h2>
    <div>{3}</div>
</body>
<html>",  shortDescription, application, issueWithMethodName, message);

            Task.Run(() => EmailDirectHelper.SendEmailAsync(toEmailAddress, subject,body,application,fromName, replyToEmailAddress, issueWithMethodName)).Wait();
        }

        #endregion Helpers

        public bool TryAction(int maxAttemptCount, Func<bool> action, string actionDescription, bool throwOnFailure = true)
        {
            IEnumerable<Exception> exceptions;
            int attemptCount;
            bool success = TryAction(maxAttemptCount, action, out attemptCount, out exceptions);
            if (success)
            {
                return true;
            }
            
            AggregateException aggregateException = new AggregateException(String.Format("{0} failed after {1:n0} attempts", actionDescription, attemptCount), exceptions);
            Log("{0} failed after {1:n0} attempts, EXCEPTIONS: {2}", actionDescription, attemptCount, aggregateException);
            if (throwOnFailure)
            {
                throw aggregateException;
            }
            return false;
        }

        public bool TryAction(int maxAttemptCount, Func<bool> action, out int attempt, out IEnumerable<Exception> exceptions)
        {
            List<Exception> exceptionList = new List<Exception>();
            exceptions = exceptionList;
            for (attempt = 0; attempt < maxAttemptCount; ++attempt)
            {
                try
                {
                    if (action())
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    exceptionList.Add(exception);
                }
            }
            return false;
        }

        /// <summary>
        /// Similar to the version where the delegate must return boolean except that the delegate is assumed 
        /// to be successful if it does not throw an error.
        /// </summary>
        /// <param name="maxAttemptCount"></param>
        /// <param name="action"></param>
        /// <param name="actionDescription"></param>
        /// <param name="throwOnFailure"></param>
        /// <returns></returns>
        public bool TryAction(int maxAttemptCount, Action action, string actionDescription, bool throwOnFailure = true)
        {
            return TryAction(maxAttemptCount, () => { action(); return true; }, actionDescription, throwOnFailure);
        }

        /// <summary>
        /// Similar to the version where the delegate must return boolean except that the delegate is assumed 
        /// to be successful if it does not throw an error.
        /// </summary>
        /// <param name="maxAttemptCount"></param>
        /// <param name="action"></param>
        /// <param name="attempt"></param>
        /// <param name="exceptions"></param>
        /// <returns></returns>
        public bool TryAction(int maxAttemptCount, Action action, out int attempt, out IEnumerable<Exception> exceptions)
        {
            return TryAction(maxAttemptCount, () => { action(); return true; }, out attempt, out exceptions);
        }

        protected CancellationTokenSource cancellationTokenSource;
        public void Start(Action mainLoopAction)
        {
            Inititalize();

            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            
            int maxIterations = AppConfig<int>.Get.MaxIterations;
            while (false == cancellationToken.IsCancellationRequested && (IterationCount++ < maxIterations || maxIterations == 0))
            {
                FireEngineEvent(OnIterationStarting, new EngineEventArgs { EventType = EngineEventType.IterationStarting });
                
                mainLoopAction();
                Task.WaitAll(IncompleteTasks); // prevent tasks from spanning iterations
                
                FireEngineEvent(OnIterationFinished, new EngineEventArgs { EventType = EngineEventType.IterationFinished });
            }
        }

        public void Quit() { cancellationTokenSource.Cancel(); }
        public bool Quitting { get { return cancellationTokenSource.Token.IsCancellationRequested; } }

        #region startup and shutdown

        protected virtual void Inititalize(StreamWriter loggingStreamWriter = null)
        {
            InitializeInteral(loggingStreamWriter);
            FireEngineEvent(OnEngineInitialized, EngineEventType.EngineInitialized);
        }

        protected virtual StreamWriter CreateLoggingStreamWriter(string fileName=null)
        {
            return new StreamWriter(fileName ?? AppConfig.LogFile);
        }

        void InitializeInteral(StreamWriter loggingStreamWriter = null)
        {
            if (null != loggingStreamWriter || null == LogStreamWriter)
            {
                LogStreamWriter = loggingStreamWriter ?? CreateLoggingStreamWriter();
                LogStreamWriter.AutoFlush = true;
                LogMessagesAtLevel = AppConfig<LogLevel>.Get.LogLevel;
            }
        }

        protected virtual void RestartEngine(bool startNewLogFile=false)
        {
            DisposeInternal();
            InitializeInteral(startNewLogFile ? CreateLoggingStreamWriter() : null);
            FireEngineEvent(OnEngineRestarted, EngineEventType.EngineRestarted);
        }

        void DisposeInternal()
        {
            LogStreamWriter.Dispose();
        }

        public virtual void Dispose()
        {
            FireEngineEvent(OnEngineDisposing, EngineEventType.EngineDisposing);
            
            DisposeInternal();
        }

        #endregion startup and shutdown
        #region multi-tasking

        object TaskListMutex = new object(); // for locking the task-list
        //List<T> TaskList = new List<T>();
        Dictionary<Task, T> TaskDictionary = new Dictionary<Task, T>();

        public Task[] IncompleteTasks
        {
            get
            {
                lock (TaskListMutex)
                {
                    return (from t in TaskDictionary where t.Key.IsCompleted == false select t.Key).ToArray();
                }
            }
        }

        public void AddTask(Action<T> action, T taskData)
        {
            int maxConcurrentThreads = AppConfig<int>.Get.MaxConcurrentThreads;
            if (maxConcurrentThreads > 0 && maxConcurrentThreads <= TaskDictionary.Keys.Where(t => t.Status == TaskStatus.Running).Count())
            {
                Task.WaitAny(IncompleteTasks);
            }

            Task task = null;
            task = new Task(() =>
            {
                try
                {
                    action(taskData);
                    FireEngineEvent(OnTaskSuccessful, new EngineEventArgs
                    {
                        EventType = EngineEventType.TaskSuccessful,
                        TaskData = taskData,
                        Task = task
                    });
                }
                catch (Exception exception)
                {
                    FireEngineEvent(OnTaskFailed, new EngineEventArgs
                    {
                        EventType = EngineEventType.TaskFailed,
                        TaskData = taskData,
                        Task = task,
                        Exception = exception
                    });
                }
                finally
                {
                    lock (TaskListMutex)
                    {
                        TaskDictionary.Remove(task);
                    }
                }
            });
            lock (TaskListMutex)
            {
                TaskDictionary.Add(task, taskData);
            }
            task.Start();
        }


        /*
        Task[] ToSystemTaskArray(IEnumerable<Task> tasks=null)
        {
            lock (TaskListMutex)
            {
                return (tasks ?? TaskDictionary.Keys).ToArray();
            }
        }

        public IEnumerable<Task> IncompleteTasks
        {
            get
            {
                lock (TaskListMutex)
                {
                    return from t in TaskDictionary where t.Key.IsCompleted == false select t.Key;
                }
            }
        }

        public void WaitAll(IEnumerable<Task> tasks = null)
        {
            Task.WaitAll(ToSystemTaskArray(tasks??IncompleteTasks));
        }

        public void WaitAny(IEnumerable<Task> tasks = null)
        {
            Task.WaitAny(ToSystemTaskArray(tasks??IncompleteTasks));
        }

        public void RaiseCriticalError(Exception e, string logNotes)
        {
            throw new NotImplementedException();
        }
         * */


        /*public void ProcessAndRemoveTasks(Task[] tasks, EngineTask taskProcessingHandler = null, EngineEventType eventType = EngineEventType.None)
        {
            lock (TaskListMutex)
            {
                List<Task> tasksToRemove = new List<Task>();
                foreach (Task task in tasks)
                {
                    if (null != taskProcessingHandler)
                    {
                        EngineEventArgs taskProcessingEvent = new EngineEventArgs { EventType = eventType, TaskData = TaskDictionary[task] };
                        taskProcessingHandler(taskProcessingEvent);
                    }
                    tasksToRemove.Add(task);
                }
                tasksToRemove.ForEach(t => TaskDictionary.Remove(t));
                //foreach (Task t in tasks) TaskDictionary.Remove(t);
            }
        }
        public void ProcessAndRemoveTasks(IEnumerable<Task> tasks, EngineTask taskProcessingHandler=null, EngineEventType eventType = EngineEventType.None)
        {
            ProcessAndRemoveTasks(tasks.ToArray(), taskProcessingHandler, eventType);
        }

        public void ProcessAndRemoveAllTasksHavingStatus(TaskStatus status, EngineTask taskProcessingHandler=null, EngineEventType eventType=EngineEventType.None)
        {
            IEnumerable<Task> tasksHavingStatus;
            lock (TaskListMutex)
            {
                tasksHavingStatus = from x in TaskDictionary.Keys where x.Status == status select x;
            }
            ProcessAndRemoveTasks(tasksHavingStatus, taskProcessingHandler, eventType);
        }

        private void ProcessCompletedTasks()
        {
            // reap successful
            EngineTask completedHandler = (eventArgs) => { FireEngineEvent(OnTaskSuccessful, eventArgs); return EngineActionResult.Unknown; };
            ProcessAndRemoveAllTasksHavingStatus(TaskStatus.RanToCompletion, completedHandler, EngineEventType.TaskSuccessful);

            // reap failed
            EngineTask faultedHandler = (eventArgs) => { FireEngineEvent(OnTaskFailed, eventArgs); return EngineActionResult.Unknown; };
            ProcessAndRemoveAllTasksHavingStatus(TaskStatus.Faulted, faultedHandler, EngineEventType.TaskFailed);
        }*/

        #endregion multi-tasking
    }
}
