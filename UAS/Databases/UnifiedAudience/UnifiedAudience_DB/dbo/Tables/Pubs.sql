CREATE TABLE [dbo].[Pubs] (
    [PubID]               INT           IDENTITY (1, 1) NOT NULL,
    [PubName]             VARCHAR (100) NOT NULL,
    [istradeshow]         BIT           NULL,
    [PubCode]             VARCHAR (50)  NULL,
    [PubTypeID]           INT           NULL,
    [GroupID]             INT           NULL,
    [EnableSearching]     BIT           NULL,
    [Score]               INT           NULL,
    [SortOrder]           INT           NULL,
    [DateCreated]         DATETIME      CONSTRAINT [DF_Pubs_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]         DATETIME      NULL,
    [CreatedByUserID]     INT           NULL,
    [UpdatedByUserID]     INT           NULL,
    [ClientID]            INT           NULL,
    [YearStartDate]       VARCHAR (5)   NULL,
    [YearEndDate]         VARCHAR (5)   NULL,
    [IssueDate]           DATETIME      NULL,
    [IsImported]          BIT           NULL,
    [IsActive]            BIT           NULL,
    [AllowDataEntry]      BIT           NULL,
    [FrequencyID]         INT           NULL,
    [KMImportAllowed]     BIT           NULL,
    [ClientImportAllowed] BIT           NULL,
    [AddRemoveAllowed]    BIT           NULL,
    [AcsMailerInfoId]     INT           NULL,
    [IsUAD]               BIT           NULL,
    [IsCirc]              BIT           NULL,
	[IsOpenCloseLocked]   BIT			NULL DEFAULT ('false'), 
    [HasPaidRecords] BIT NULL, 
    [UseSubGen] BIT NULL, 
    CONSTRAINT [PK_Pubs] PRIMARY KEY CLUSTERED ([PubID] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_Pubs_Pubcode]
    ON [dbo].[Pubs]([PubCode] ASC) WITH (FILLFACTOR = 90);
GO