CREATE TABLE [dbo].[EditionActivityLog] (
    [EAID]           INT           IDENTITY (1, 1) NOT NULL,
    [EditionID]      INT           NOT NULL,
    [EmailID]        INT           NULL,
    [BlastID]        INT           NULL,
    [ActionDate]     DATETIME      NOT NULL,
    [ActionTypeCode] VARCHAR (50)  NOT NULL,
    [ActionValue]    VARCHAR (255) NOT NULL,
    [IPAddress]      VARCHAR (50)  NOT NULL,
    [IsAnonymous]    BIT           NULL,
    [LinkID]         INT           NULL,
    [PageID]         INT           NULL,
    [SessionID]      VARCHAR (36)  NULL,
    [PageEnd]        INT           NULL,
    [PageStart]      INT           NULL,
    [CreatedDate]    DATETIME      CONSTRAINT [DF_EditionActivityLog_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedUserID]  INT           NULL,
    [IsDeleted]      BIT           CONSTRAINT [DF_EditionActivityLog_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]    DATETIME      NULL,
    [UpdatedUserID]  INT           NULL,
    CONSTRAINT [PK_EditionActivityLog] PRIMARY KEY CLUSTERED ([EAID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_EditionID]
    ON [dbo].[EditionActivityLog]([EditionID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EditionID_EmailID]
    ON [dbo].[EditionActivityLog]([EditionID] ASC, [EmailID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EditionID_ActionTypeCode]
    ON [dbo].[EditionActivityLog]([EditionID] ASC, [ActionTypeCode] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EditionActivityLog_ActionValue]
    ON [dbo].[EditionActivityLog]([ActionValue] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EditionActivityLog_eaid_ActionValue]
    ON [dbo].[EditionActivityLog]([EAID] ASC, [ActionValue] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EmailID]
    ON [dbo].[EditionActivityLog]([EmailID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_EditionActivityLog_sessionID_editionID]
    ON [dbo].[EditionActivityLog]([SessionID] ASC, [EditionID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE STATISTICS [_dta_stat_37575172_2_4_3_6_5_1]
    ON [dbo].[EditionActivityLog]([EmailID], [ActionTypeCode], [EditionID], [ActionValue], [ActionDate], [EAID]);


GO
CREATE STATISTICS [_dta_stat_37575172_5_4_3_2]
    ON [dbo].[EditionActivityLog]([ActionDate], [ActionTypeCode], [EditionID], [BlastID], [EmailID], [EAID]);


GO
CREATE STATISTICS [_dta_stat_37575172_2_5_4]
    ON [dbo].[EditionActivityLog]([EditionID], [ActionDate], [ActionTypeCode], [EAID]);


GO
CREATE STATISTICS [_dta_stat_37575172_3_5]
    ON [dbo].[EditionActivityLog]([EditionID], [ActionDate], [EAID]);

