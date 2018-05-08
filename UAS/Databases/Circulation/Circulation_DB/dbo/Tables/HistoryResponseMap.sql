CREATE TABLE [dbo].[HistoryResponseMap] (
    [HistoryResponseMapID] INT           IDENTITY (1, 1) NOT NULL,
    [SubscriptionID]       INT           NOT NULL,
    [ResponseID]           INT           NOT NULL,
    [IsActive]             BIT           NOT NULL,
    [DateCreated]          DATETIME      NOT NULL,
    [CreatedByUserID]      INT           NOT NULL,
    [ResponseOther]        VARCHAR (300) NULL,
    CONSTRAINT [PK_HistoryResponse] PRIMARY KEY CLUSTERED ([HistoryResponseMapID] ASC) WITH (FILLFACTOR = 80)
);

