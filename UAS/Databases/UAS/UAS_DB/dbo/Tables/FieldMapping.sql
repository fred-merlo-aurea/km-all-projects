CREATE TABLE [dbo].[FieldMapping] (
    [FieldMappingID]      INT            IDENTITY (1, 1) NOT NULL,
	[FieldMappingTypeID]  INT			 NOT NULL,
	[IsNonFileColumn]	  BIT			 NOT NULL,
    [SourceFileID]        INT            NOT NULL,
    [IncomingField]       VARCHAR (50)   NOT NULL,
    [MAFField]            VARCHAR (50)   NOT NULL,
    [PubNumber]           INT            NULL,
    [DataType]            VARCHAR (50)   NULL,
    [PreviewData]         VARCHAR (1000) NULL,
    [ColumnOrder]         INT            CONSTRAINT [DF_FieldMapping_ColumnOrder] DEFAULT ((0)) NOT NULL,
	[HasMultiMapping]	  BIT			 CONSTRAINT [DF_FieldMapping_HasMultiMapping] DEFAULT ((0))NOT NULL,
    [DateCreated]         DATETIME       NOT NULL,
    [DateUpdated]         DATETIME       NULL,
    [CreatedByUserID]     INT            NOT NULL,
    [UpdatedByUserID]     INT       NULL,
	[DemographicUpdateCodeId] INT            CONSTRAINT [DF_FieldMapping_DemographicUpdateCodeId] DEFAULT ((1928)) NOT NULL,
    CONSTRAINT [PK_FieldMapping] PRIMARY KEY CLUSTERED ([FieldMappingID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_FieldMapping_SourceFile] FOREIGN KEY ([SourceFileID]) REFERENCES [dbo].[SourceFile] ([SourceFileID])
);
GO

CREATE NONCLUSTERED INDEX [NonClusteredIndex-PKFieldmapping ]
    ON [dbo].[FieldMapping]([FieldMappingID] ASC, [SourceFileID] ASC);
GO

