CREATE TABLE [dbo].[TransformAssign] (
    [TransformAssignID] INT           IDENTITY (1, 1) NOT NULL,
    [TransformationID]  INT           NULL,
    [Value]             VARCHAR (200) NOT NULL,
    [IsActive]          BIT           NOT NULL,
    [HasPubID]          BIT           NOT NULL,
    [DateCreated]       DATETIME      NOT NULL,
    [DateUpdated]       DATETIME      NULL,
    [CreatedByUserID]   INT           NOT NULL,
    [UpdatedByUserID]   INT      NULL,
	[PubID]				INT		 NULL,
    CONSTRAINT [PK_TransformAssign] PRIMARY KEY CLUSTERED ([TransformAssignID] ASC),
    CONSTRAINT [FK_TransformAssign_Transformation_TransformationID] FOREIGN KEY ([TransformationID]) REFERENCES [dbo].[Transformation] ([TransformationID])
);

