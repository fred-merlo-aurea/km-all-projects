CREATE TABLE [dbo].[TransformSplitTrans] (
    [SplitTransformID] INT           IDENTITY (1, 1) NOT NULL,
    [TransformationID] INT           NULL,
    [SplitBeforeID]    INT           NULL,
    [DataMapID]        INT           NULL,
    [SplitAfterID]     INT           NULL,
    [Column]           VARCHAR (200) NULL,
    [IsActive]         BIT           NOT NULL,
    [DateCreated]      DATETIME      NOT NULL,
    [DateUpdated]      DATETIME      NULL,
    [CreatedByUserID]  INT           NOT NULL,
    [UpdatedByUserID]  INT      NULL,
    PRIMARY KEY CLUSTERED ([SplitTransformID] ASC) WITH (FILLFACTOR = 90)
);
GO