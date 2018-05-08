CREATE TABLE [dbo].[Transformation] (
    [TransformationID]          INT           IDENTITY (1, 1) NOT NULL,
    [TransformationTypeID]      INT           NOT NULL,
    [TransformationName]        VARCHAR (100) NULL,
    [TransformationDescription] VARCHAR (750) NOT NULL,
	[MapsPubCode]				BIT			  NOT NULL DEFAULT ('false'),
	[LastStepDataMap]			BIT			  NOT NULL DEFAULT ('false'),
    [ClientID]                  INT           NOT NULL,
    [IsActive]                  BIT           NOT NULL DEFAULT ('true'),
    [DateCreated]               DATETIME      NOT NULL,
    [DateUpdated]               DATETIME      NULL,
    [CreatedByUserID]           INT           NOT NULL,
    [UpdatedByUserID]           INT           NULL,
	[IsTemplate]				BIT			  NOT NULL DEFAULT ('true'),
    CONSTRAINT [PK_Transformation_TransformationID] PRIMARY KEY CLUSTERED ([TransformationID] ASC) WITH (FILLFACTOR = 90),
);
GO
CREATE NONCLUSTERED INDEX [IDX_Transformation_ClientID]
    ON [dbo].[Transformation]([ClientID] ASC) WITH (FILLFACTOR = 90);

