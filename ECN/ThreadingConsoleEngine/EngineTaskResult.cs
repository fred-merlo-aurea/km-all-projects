namespace ECN_EngineFramework
{
    public enum EngineActionResult
    {
        Unknown = 0,
        OK,            // happiness
        Retry,         // try again
        Warning,       // increment the warning count
        Error,         // log, send a sev 2 notice
        CriticalError, // log, send a sev 1 notice
        FatalError     // terminate the engine
    }
}
