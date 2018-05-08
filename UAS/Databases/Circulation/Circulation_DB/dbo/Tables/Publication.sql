CREATE TABLE [dbo].[Publication] (
    [PublicationID]   INT          IDENTITY (1, 1) NOT NULL,
    [PublicationName] VARCHAR (50) NOT NULL,
    [PublicationCode] VARCHAR (50) NOT NULL,
    [PublisherID]     INT          NOT NULL,
    [YearStartDate]   CHAR (5)     NOT NULL,
    [YearEndDate]     CHAR (5)     NOT NULL,
    [IssueDate]       DATE         NULL,
    [IsImported]      BIT          NOT NULL,
    [IsActive]        BIT          NOT NULL,
    [AllowDataEntry]  BIT          NOT NULL,
    [FrequencyID]     INT          NULL,
	KMImportAllowed	  BIT Default('false') NOT NULL, 
	ClientImportAllowed	  BIT Default('false') NOT NULL,
	AddRemoveAllowed	  BIT Default('false') NOT NULL,
	[AcsMailerInfoId]		INT NULL,
    [DateCreated]     DATETIME     NOT NULL,
    [DateUpdated]     DATETIME     NULL,
    [CreatedByUserID] INT          NOT NULL,
    [UpdatedByUserID] INT          NULL,
    [IsOpenCloseLocked] BIT NULL DEFAULT ('false'), 
    CONSTRAINT [PK_Publication] PRIMARY KEY CLUSTERED ([PublicationCode] ASC, [PublisherID] ASC) WITH (FILLFACTOR = 80)
);

