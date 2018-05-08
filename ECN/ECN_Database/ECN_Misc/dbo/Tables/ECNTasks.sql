CREATE TABLE [dbo].[ECNTasks] (
    [TaskID]        INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]    INT           NULL,
    [Product]       VARCHAR (50)  CONSTRAINT [DF_ECNErrors_Prodcut] DEFAULT ('') NULL,
    [TaskType]      VARCHAR (250) CONSTRAINT [DF_ECNErrors_ErrorType] DEFAULT ('') NULL,
    [TaskPath]      VARCHAR (250) CONSTRAINT [DF_ECNErrors_ErrorPath] DEFAULT ('') NULL,
    [TaskStack]     TEXT          CONSTRAINT [DF_ECNErrors_ErrorStack] DEFAULT ('') NULL,
    [Priority]      VARCHAR (50)  CONSTRAINT [DF_ECNTasks_Priority] DEFAULT ('low') NULL,
    [Status]        VARCHAR (50)  CONSTRAINT [DF_ECNErrors_Status] DEFAULT ('pending') NULL,
    [InternalNotes] TEXT          CONSTRAINT [DF_ECNErrors_InternalNotes] DEFAULT ('') NULL,
    [AssignedTo]    VARCHAR (50)  CONSTRAINT [DF_ECNErrors_AssignedTo] DEFAULT ('') NULL,
    [NotifyAM]      VARCHAR (50)  CONSTRAINT [DF_ECNErrors_NotifyAM] DEFAULT ('') NULL,
    [DateAdded]     DATETIME      CONSTRAINT [DF_ECNErrors_DateAdded] DEFAULT (getdate()) NULL,
    [DateUpdated]   DATETIME      CONSTRAINT [DF_ECNErrors_DateUpdated] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_ECNErrors] PRIMARY KEY CLUSTERED ([TaskID] ASC) WITH (FILLFACTOR = 80)
);

