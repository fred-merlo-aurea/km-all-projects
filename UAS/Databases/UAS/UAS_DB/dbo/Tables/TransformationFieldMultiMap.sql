CREATE TABLE [dbo].[TransformationFieldMultiMap] (
    [TransformationFieldMultiMapID] INT      IDENTITY (1, 1) NOT NULL,
    [TransformationID]         INT      NOT NULL,
    [SourceFileID]             INT      NOT NULL,
    [FieldMappingID]           INT      NOT NULL,
	[FieldMultiMapID]          INT      NOT NULL,
    [IsActive]                 BIT      NOT NULL,
    [DateCreated]              DATETIME NOT NULL,
    [DateUpdated]              DATETIME NULL,
    [CreatedByUserID]          INT      NOT NULL,
    [UpdatedByUserID]          INT NULL,
    CONSTRAINT [PK_TransformationFieldMultiMap] PRIMARY KEY CLUSTERED ([TransformationFieldMultiMapID] ASC),
    CONSTRAINT [FK_TransformationFieldMultiMap_FieldMapping_FieldMappingID] FOREIGN KEY ([FieldMappingID]) REFERENCES [dbo].[FieldMapping] ([FieldMappingID]),
    CONSTRAINT [FK_TransformationFieldMultiMap_Transformation_TransformationID] FOREIGN KEY ([TransformationID]) REFERENCES [dbo].[Transformation] ([TransformationID]),
    CONSTRAINT [FK_TransFieldMultiMap_SourceFile] FOREIGN KEY ([SourceFileID]) REFERENCES [dbo].[SourceFile] ([SourceFileID])
);