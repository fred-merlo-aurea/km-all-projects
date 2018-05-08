CREATE TABLE [dbo].[TransformationPubMap] (
    [TransformationPubMapID] INT      IDENTITY (1, 1) NOT NULL,
    [TransformationID]       INT      NOT NULL,
    [PubID]                  INT      NOT NULL,
    [IsActive]               BIT      NULL,
    [DateCreated]            DATETIME NULL,
    [DateUpdated]            DATETIME NULL,
    [CreatedByUserID]        INT      NULL,
    [UpdatedByUserID]        INT NULL,
    CONSTRAINT [PK_TransformationPubMap] PRIMARY KEY CLUSTERED ([TransformationPubMapID] ASC),
    CONSTRAINT [FK_TransformationPubMap_Transformation_TransformationID] FOREIGN KEY ([TransformationID]) REFERENCES [dbo].[Transformation] ([TransformationID])
);

