CREATE TABLE [dbo].[StatsFatigueReport] (
    [Id]             UNIQUEIDENTIFIER NULL,
    [CustomerId]     INT              NULL,
    [ParamStartDate] DATETIME         NULL,
    [ParamEndDate]   DATETIME         NULL,
    [FilterField]    VARCHAR (100)    NULL,
    [FilterValue]    VARCHAR (100)    NULL,
    [ExecStartTime]  DATETIME         NULL,
    [ExecEndTime]    DATETIME         NULL
);

