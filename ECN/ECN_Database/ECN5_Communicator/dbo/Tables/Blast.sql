CREATE TABLE [dbo].[Blast] (
    [BlastID]             INT            IDENTITY (1, 1) NOT NULL,
    [CustomerID]          INT            NULL,
    [EmailSubject]        VARCHAR (255)  NULL,
    [EmailFrom]           VARCHAR (100)  NULL,
    [EmailFromName]       VARCHAR (100)  NULL,
    [SendTime]            DATETIME       NULL,
    [AttemptTotal]        BIGINT         NULL,
    [SendTotal]           BIGINT         NULL,
    [SendBytes]           BIGINT         NULL,
    [StatusCode]          VARCHAR (50)   NULL,
    [BlastType]           VARCHAR (50)   NULL,
    [CodeID]              INT            NULL,
    [LayoutID]            INT            NULL,
    [GroupID]             INT            NULL,
    [FinishTime]          DATETIME       NULL,
    [SuccessTotal]        BIGINT         NULL,
    [BlastLog]            VARCHAR (2000) NULL,
    [CreatedUserID]       INT            NULL,
    [FilterID]            INT            NULL,
    [Spinlock]            CHAR (1)       NOT NULL,
    [ReplyTo]             VARCHAR (100)  NULL,
    [TestBlast]           VARCHAR (1)    NULL,
    [BlastFrequency]      VARCHAR (50)   CONSTRAINT [DF_Blasts_BlastFrequency] DEFAULT ('ONETIME') NULL,
    [RefBlastID]          VARCHAR (2000) NULL,
    [BlastSuppression]    VARCHAR (2000) NULL,
    [AddOptOuts_to_MS]    BIT            NULL,
    [DynamicFromName]     VARCHAR (100)  NULL,
    [DynamicFromEmail]    VARCHAR (100)  NULL,
    [DynamicReplyToEmail] VARCHAR (100)  NULL,
    [BlastEngineID]       INT            NULL,
    [HasEmailPreview]     BIT            NULL,
    [BlastScheduleID]     INT            NULL,
    [OverrideAmount]      INT            NULL,
    [OverrideIsAmount]    BIT            NULL,
    [StartTime]           DATETIME       NULL,
    [CreatedDate]         DATETIME       CONSTRAINT [DF_Blast_CREATEDDATE] DEFAULT (getdate()) NULL,
    [NodeID]              VARCHAR (50)   NULL,
    [SampleID]            INT            NULL,
    [SmartSegmentID]      INT            NULL,
    [SMSOptInTotal]       INT            NULL,
    [UpdatedDate]         DATETIME       NULL,
    [UpdatedUserID]       INT            NULL,
    [EnableCacheBuster]   BIT            NULL,
	[IgnoreSuppression]   BIT			 NULL,
    CONSTRAINT [PK_Blasts] PRIMARY KEY CLUSTERED ([BlastID] ASC) WITH (FILLFACTOR = 80)
);




GO
CREATE NONCLUSTERED INDEX [IX_CustomerID]
    ON [dbo].[Blast]([CustomerID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_CID_Test_SendTime]
    ON [dbo].[Blast]([CustomerID] ASC, [TestBlast] ASC, [SendTime] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ids_Blasts_LayoutID]
    ON [dbo].[Blast]([LayoutID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_Blasts_1]
    ON [dbo].[Blast]([StatusCode] ASC, [BlastEngineID] ASC, [SendTime] ASC, [BlastType] ASC)
    INCLUDE([BlastID], [CustomerID]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_Blasts_CustomerID_TestBlast]
    ON [dbo].[Blast]([CustomerID] ASC, [TestBlast] ASC)
    INCLUDE([ReplyTo]) WITH (FILLFACTOR = 80);


GO
CREATE STATISTICS [_dta_stat_1051866814_2_1]
    ON [dbo].[Blast]([CustomerID], [BlastID]);


GO
CREATE STATISTICS [_dta_stat_1051866814_13_1_14]
    ON [dbo].[Blast]([LayoutID], [BlastID], [GroupID]);


GO
CREATE STATISTICS [_dta_stat_1051866814_1_14_2_13]
    ON [dbo].[Blast]([BlastID], [GroupID], [CustomerID], [LayoutID]);


GO
CREATE STATISTICS [_dta_stat_1051866814_22_14]
    ON [dbo].[Blast]([TestBlast], [GroupID]);


GO
CREATE STATISTICS [_dta_stat_1051866814_1_22_14]
    ON [dbo].[Blast]([BlastID], [TestBlast], [GroupID]);


GO
CREATE STATISTICS [_dta_stat_1051866814_10_22_1]
    ON [dbo].[Blast]([StatusCode], [TestBlast], [BlastID]);


GO
CREATE STATISTICS [_dta_stat_1051866814_14_1_10]
    ON [dbo].[Blast]([GroupID], [BlastID], [StatusCode]);


GO
CREATE STATISTICS [_dta_stat_1051866814_14_10_22_1]
    ON [dbo].[Blast]([GroupID], [StatusCode], [TestBlast], [BlastID]);


GO
CREATE STATISTICS [_dta_stat_1051866814_26_1_14_10_22]
    ON [dbo].[Blast]([AddOptOuts_to_MS], [BlastID], [GroupID], [StatusCode], [TestBlast]);


GO
GRANT DELETE
    ON OBJECT::[dbo].[Blast] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Blast] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Blast] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Blast] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Blast] TO [reader]
    AS [dbo];

