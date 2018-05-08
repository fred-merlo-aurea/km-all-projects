CREATE TABLE [dbo].[AdHocDimension] (
    [AdHocDimensionID]   INT           IDENTITY (1, 1) NOT NULL,
	[AdHocDimensionGroupId] INT NOT NULL,
    [IsActive]           BIT           NOT NULL,
    [MatchValue]         VARCHAR (500) NOT NULL,
    [Operator]           VARCHAR (50)  NOT NULL,    
    [DimensionValue]     VARCHAR (500) NOT NULL,
    [UpdateUAD]          BIT           NOT NULL,
    [UADLastUpdatedDate] DATETIME      NOT NULL,
    [DateCreated]        DATETIME      NOT NULL,
    [DateUpdated]        DATETIME      NULL,
    [CreatedByUserID]    INT           NOT NULL,
    [UpdatedByUserID]    INT           NULL,
    CONSTRAINT [PK_AdHocDimension] PRIMARY KEY CLUSTERED ([AdHocDimensionID] ASC)
);
GO
