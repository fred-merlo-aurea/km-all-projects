CREATE TABLE [dbo].[TransformDataMap] (
    [TransformDataMapID] INT           IDENTITY (1, 1) NOT NULL,
    [TransformationID]   INT           NULL,
    [PubID]              INT           NOT NULL,
    [MatchType]          VARCHAR (50)  NOT NULL,
    [SourceData]         VARCHAR (200) NOT NULL,
    [DesiredData]        VARCHAR (200) NOT NULL,
    [IsActive]           BIT           NOT NULL,
    [DateCreated]        DATETIME      NOT NULL,
    [DateUpdated]        DATETIME      NULL,
    [CreatedByUserID]    INT           NOT NULL,
    [UpdatedByUserID]    INT      NULL,
    CONSTRAINT [PK_TransformDataMap] PRIMARY KEY CLUSTERED ([TransformDataMapID] ASC),
    CONSTRAINT [FK_TransformDataMap_Transformation_TransformationID] FOREIGN KEY ([TransformationID]) REFERENCES [dbo].[Transformation] ([TransformationID])
);

