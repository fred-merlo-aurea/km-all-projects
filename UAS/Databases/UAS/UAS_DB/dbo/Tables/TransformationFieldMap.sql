CREATE TABLE [dbo].[TransformationFieldMap] (
    [TransformationFieldMapID] INT      IDENTITY (1, 1) NOT NULL,
    [TransformationID]         INT      NOT NULL,
    [SourceFileID]             INT      NOT NULL,
    [FieldMappingID]           INT      NOT NULL,
    [IsActive]                 BIT      NOT NULL,
    [DateCreated]              DATETIME NOT NULL,
    [DateUpdated]              DATETIME NULL,
    [CreatedByUserID]          INT      NOT NULL,
    [UpdatedByUserID]          INT NULL,
    CONSTRAINT [PK_TransformationFieldMap] PRIMARY KEY CLUSTERED ([TransformationFieldMapID] ASC),
    CONSTRAINT [FK_TransformationFieldMap_FieldMapping_FieldMappingID] FOREIGN KEY ([FieldMappingID]) REFERENCES [dbo].[FieldMapping] ([FieldMappingID]),
    CONSTRAINT [FK_TransformationFieldMap_Transformation_TransformationID] FOREIGN KEY ([TransformationID]) REFERENCES [dbo].[Transformation] ([TransformationID]),
    CONSTRAINT [FK_TransFieldMap_SourceFile] FOREIGN KEY ([SourceFileID]) REFERENCES [dbo].[SourceFile] ([SourceFileID])
);

