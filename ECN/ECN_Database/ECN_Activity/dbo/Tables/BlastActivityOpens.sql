CREATE TABLE [dbo].[BlastActivityOpens] (
    [OpenID]        INT            IDENTITY (1, 1) NOT NULL,
    [BlastID]       INT            NOT NULL,
    [EmailID]       INT            NOT NULL,
    [OpenTime]      DATETIME       NOT NULL,
    [BrowserInfo]   VARCHAR (2048) NULL,
    [EAID]          INT            NULL,
    [EmailClientID] INT            NULL,
    [PlatformID]    INT            NULL,
    [RoundedTime]   DATETIME       NULL,
    CONSTRAINT [PK_BlastActivityOpens] PRIMARY KEY CLUSTERED ([OpenID] ASC) ON [ECN_Activity_Data_1]
);




GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityOpens_EmailID]
    ON [dbo].[BlastActivityOpens]([EmailID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_BlastActivityOpens_BlastEmailRoundTime]
    ON [dbo].[BlastActivityOpens]([BlastID] ASC, [EmailID] ASC, [RoundedTime] ASC) WITH (IGNORE_DUP_KEY = ON)
    ON [ECN_Activity_Index];

