CREATE TABLE [dbo].[TransformSplit] (
    [TransformSplitID] INT          IDENTITY (1, 1) NOT NULL,
    [TransformationID] INT          NULL,
    [Delimiter]        VARCHAR (50) NOT NULL,
    [IsActive]         BIT          NOT NULL,
    [DateCreated]      DATETIME     NOT NULL,
    [DateUpdated]      DATETIME     NULL,
    [CreatedByUserID]  INT          NOT NULL,
    [UpdatedByUserID]  INT     NULL,
    CONSTRAINT [PK_TransformSplit] PRIMARY KEY CLUSTERED ([TransformSplitID] ASC),
    CONSTRAINT [FK_TransformSplit_Transformation_TransformationID] FOREIGN KEY ([TransformationID]) REFERENCES [dbo].[Transformation] ([TransformationID])
);

