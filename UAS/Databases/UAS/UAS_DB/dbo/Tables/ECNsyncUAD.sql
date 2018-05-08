CREATE TABLE [dbo].[ECNsyncUAD] (
    [SourceFileID]           INT          NOT NULL,
    [DateCreated]            DATETIME     DEFAULT (getdate()) NOT NULL,
    [ProcessCode]            VARCHAR (50) NULL,
    [IsUADEngineProcessed]   BIT          DEFAULT ((0)) NOT NULL,
    [IsECNUpdateCompleted]   BIT          DEFAULT ((0)) NOT NULL,
    [ECNUpdateDateCompleted] DATETIME     NULL,
    [UADHasProcessNeeds]     BIT          DEFAULT ((0)) NOT NULL,
	[GroupID]                INT          NULL,
    [Enabled]                BIT          NULL,
    CONSTRAINT [pk_ECNSyncUAD] PRIMARY KEY CLUSTERED ([SourceFileID] ASC, [DateCreated] ASC) WITH (FILLFACTOR = 90)
);

