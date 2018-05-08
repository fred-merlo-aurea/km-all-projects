CREATE TABLE [dbo].[BlastActivityReads] (
    [ReadID]        INT            IDENTITY (1, 1) NOT NULL,
    [BlastID]       INT            NOT NULL,
    [EmailID]       INT            NOT NULL,
    [ReadTime]      DATETIME       NOT NULL,
    [BrowserInfo]   VARCHAR (2048) NULL,
    [EAID]          INT            NULL,
    [EmailClientID] INT            NULL,
    [PlatformID]    INT            NULL,
    CONSTRAINT [PK_BlastActivityReads] PRIMARY KEY CLUSTERED ([ReadID] ASC) WITH (FILLFACTOR = 80)
);




GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityReads_BlastID]
    ON [dbo].[BlastActivityReads]([BlastID] ASC)
    ON [ECN_Activity_Index];

