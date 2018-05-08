CREATE TABLE [dbo].[Summary_Data] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [EntityName]       VARCHAR (100) NOT NULL,
    [Type]             VARCHAR (50)  NOT NULL,
	[BrandID]		   INT			 NULL,
    [PubID]            INT           NULL,
	[PubTypeID]        INT           NULL,
    [DomainTrackingID] INT           NULL,
    [Date]             DATE          NOT NULL,
    [DateMonth]        INT           NOT NULL,
    [DateYear]         INT           NOT NULL,
    [CountsType]       VARCHAR (20)  NULL,
    [Counts]           BIGINT        CONSTRAINT [DF__Summary_A__recor__2DA7A64D] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Summary_Data] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE NONCLUSTERED INDEX [IDX_Summary_Data_1]
    ON [dbo].[Summary_Data]([EntityName] ASC, [Type] ASC)
    INCLUDE([PubID], [Counts]) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_Summary_Data_2]
    ON [dbo].[Summary_Data]([EntityName] ASC, [Type] ASC)
    INCLUDE([DateMonth], [DateYear], [Counts]) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_Summary_Data_3]
    ON [dbo].[Summary_Data]([EntityName] ASC, [Type] ASC, [DateMonth] ASC, [DateYear] ASC)
    INCLUDE([Counts]) WITH (FILLFACTOR = 90);
GO


