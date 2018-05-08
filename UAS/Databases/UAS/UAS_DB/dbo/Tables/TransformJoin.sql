CREATE TABLE [dbo].[TransformJoin] (
    [TransformJoinID]  INT           IDENTITY (1, 1) NOT NULL,
    [TransformationID] INT           NULL,
    [ColumnsToJoin]    VARCHAR (500) NOT NULL,
    [Delimiter]        VARCHAR (50)  NOT NULL,
    [IsActive]         BIT           NOT NULL,
    [DateCreated]      DATETIME      NOT NULL,
    [DateUpdated]      DATETIME      NULL,
    [CreatedByUserID]  INT           NOT NULL,
    [UpdatedByUserID]  INT      NULL,
    CONSTRAINT [PK_TransformJoin] PRIMARY KEY CLUSTERED ([TransformJoinID] ASC),
    CONSTRAINT [FK_TransformJoin_Transformation_TransformationID] FOREIGN KEY ([TransformationID]) REFERENCES [dbo].[Transformation] ([TransformationID])
);

