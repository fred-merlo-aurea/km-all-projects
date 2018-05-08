CREATE TABLE [dbo].[Rule] (
    [RuleID]             INT            IDENTITY (1, 1) NOT NULL,
    [RuleName]           VARCHAR (50)   NULL,
    [ConditionConnector] VARCHAR (10)   NULL,
    [WhereClause]        VARCHAR (8000) NULL,
    [CustomerID]         INT            NULL,
    [CreatedDate]        DATETIME       NULL,
    [CreatedUserID]      INT            NULL,
    [UpdatedDate]        DATETIME       NULL,
    [UpdatedUserID]      INT            NULL,
    [IsDeleted]          BIT            NULL,
    CONSTRAINT [PK_Rules] PRIMARY KEY CLUSTERED ([RuleID] ASC) WITH (FILLFACTOR = 80)
);

