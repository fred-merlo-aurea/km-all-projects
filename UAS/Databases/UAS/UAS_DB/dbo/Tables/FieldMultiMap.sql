CREATE TABLE [dbo].[FieldMultiMap]
(
	[FieldMultiMapID]      INT            IDENTITY (1, 1) NOT NULL,
	[FieldMappingID]  INT			 NOT NULL,
	[FieldMappingTypeID]  INT			 NOT NULL,
    [MAFField]            VARCHAR (50)   NOT NULL,
    [DataType]            VARCHAR (50)   NULL,
    [PreviewData]         VARCHAR (1000) NULL,
    [ColumnOrder]         INT            CONSTRAINT [DF_FieldMapping_FMMColumnOrder] DEFAULT ((0)) NOT NULL,
    [DateCreated]         DATETIME       NOT NULL,
    [DateUpdated]         DATETIME       NULL,
    [CreatedByUserID]     INT            NOT NULL,
    [UpdatedByUserID]     INT       NULL
)
