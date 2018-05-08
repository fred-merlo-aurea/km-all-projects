CREATE TABLE [dbo].[SubmitHistory] (
    [SubmitHistory_Seq_ID] INT              IDENTITY (1, 1) NOT NULL,
    [HistoryToken]         UNIQUEIDENTIFIER NOT NULL,
    [Form_Seq_ID]          INT              NOT NULL,
    [Status]               INT              NULL,
    [Added]                DATETIME         NOT NULL,
    CONSTRAINT [PK_SubmitHistory] PRIMARY KEY CLUSTERED ([SubmitHistory_Seq_ID] ASC),
    CONSTRAINT [FK_Form_SubmitHistory] FOREIGN KEY ([Form_Seq_ID]) REFERENCES [dbo].[Form] ([Form_Seq_ID]) ON DELETE CASCADE,
    CONSTRAINT [UQ_SubmitHistory_Token] UNIQUE NONCLUSTERED ([HistoryToken] ASC)
);

