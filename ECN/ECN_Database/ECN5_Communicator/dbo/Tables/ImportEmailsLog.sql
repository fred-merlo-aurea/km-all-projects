CREATE TABLE [dbo].[ImportEmailsLog] (
    [ImportEmailID] INT           IDENTITY (1, 1) NOT NULL,
    [EmailAddress]  VARCHAR (200) NULL,
    [GroupID]       INT           NULL,
    [ActionCode]    VARCHAR (100) NULL,
    [FileName]      VARCHAR (200) NULL,
    [Date]          DATETIME      CONSTRAINT [DF_ImportEmailsLog_Date] DEFAULT (getdate()) NULL,
    [Reason]        VARCHAR (50)  NULL,
    [source]     VARCHAR (200) NULL,
    CONSTRAINT [PK_ImportEmailsLog] PRIMARY KEY CLUSTERED ([ImportEmailID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ImportEmailsLog_FileName_ActionCode]
    ON [dbo].[ImportEmailsLog]([FileName] ASC, [ActionCode] ASC);

