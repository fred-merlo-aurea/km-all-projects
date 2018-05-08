CREATE TABLE [dbo].[AggregateDimension] (
    [AggregateDimensionID]    INT           IDENTITY (1, 1) NOT NULL,
    [ClientID]                INT           NOT NULL,
    [IsActive]                BIT           NOT NULL,
    [StandardFields]          VARCHAR (MAX) NULL,
    [DimensionFields]         VARCHAR (MAX) NULL,
    [Formula]                 VARCHAR (MAX) NULL,
    [CreatedDimension]        VARCHAR (250) NOT NULL,
    [ClientCustomProcedureID] INT           NULL,
    [DateCreated]             DATETIME      NOT NULL,
    [DateUpdated]             DATETIME      NULL,
    [CreatedByUserID]         INT           NOT NULL,
    [UpdatedByUserID]         INT           NULL,
    CONSTRAINT [PK_AggregateDimension] PRIMARY KEY CLUSTERED ([AggregateDimensionID] ASC)
);
GO
