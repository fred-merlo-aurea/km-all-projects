CREATE TABLE [dbo].[DataCompareDimension] (
    [DataCompareDimensionId]          INT              IDENTITY (1, 1) NOT NULL,
    [ProductId]              INT              NULL,
    [SFRecordIdentifier] UNIQUEIDENTIFIER NOT NULL,
    [MAFField]           VARCHAR (255)    NULL,
    [Value]              VARCHAR (MAX)    NULL,
    [NotExists]          BIT              NULL,
    [DateCreated]        DATETIME         CONSTRAINT [DF_DataCompareDimension_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]        DATETIME         NULL,
    [CreatedByUserID]    INT              NOT NULL,
    [UpdatedByUserID]    INT              NULL,
	[DemographicUpdateCodeId] INT		  CONSTRAINT [DF_DataCompareDimension_DemographicUpdateCodeId] DEFAULT (1928) NOT NULL,
	[IsAdhoc] BIT						  CONSTRAINT [DF_DataCompareDimension_IsAdhoc] DEFAULT (0) NOT NULL,
	[ResponseOther] VARCHAR(256) NULL,
    CONSTRAINT [PK_DataCompareDimension] PRIMARY KEY CLUSTERED ([DataCompareDimensionId] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IDX_DataCompareDimension_SFRecordIdentifier]
    ON [dbo].[DataCompareDimension]([SFRecordIdentifier] ASC) WITH (FILLFACTOR = 70);
GO