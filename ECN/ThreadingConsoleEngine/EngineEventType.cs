namespace ECN_EngineFramework
{
    public enum EngineEventType
    {
        None = 0,
        EngineInitialized,
        IterationStarting,
        IterationFinished,
        EngineRestarted,
        TaskStart,
        TaskSuccessful,
        TaskFailed,
        EngineDisposing
    }
}
