CREATE TABLE [dbo].[BlastActivityExceptions] (
    [ExceptionID]   INT            IDENTITY (1, 1) NOT NULL,
    [BlastID]       INT            NULL,
    [ErrorMessage]  VARCHAR (8000) NULL,
    [MessageXML]    XML            NULL,
    [ExceptionDate] DATETIME       CONSTRAINT [DF_BlastActivityExceptions_ExceptionDate] DEFAULT (getdate()) NULL,
    [IsCleared]     BIT            CONSTRAINT [DF_BlastActivityExceptions_IsCleared] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_BlastActivityExceptions] PRIMARY KEY CLUSTERED ([ExceptionID] ASC) WITH (FILLFACTOR = 80)
);

