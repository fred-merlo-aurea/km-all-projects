CREATE TABLE [dbo].[Rules] (
    [RuleID]        INT           IDENTITY (1, 1) NOT NULL,
    [RuleName]      VARCHAR (100) NOT NULL,
    [PublicationID] INT           NOT NULL,
    [EditionID]     INT           NOT NULL,
    [WhereClause]   TEXT          NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_Rules_CreateDate] DEFAULT (getdate()) NOT NULL,
    [CreatedUserID] INT           NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_Rules_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME      NULL,
    [UpdatedUserID] INT           NULL,
    CONSTRAINT [PK_Rules] PRIMARY KEY CLUSTERED ([RuleID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Rules_Editions] FOREIGN KEY ([EditionID]) REFERENCES [dbo].[Edition] ([EditionID]),
    CONSTRAINT [FK_Rules_Publications] FOREIGN KEY ([PublicationID]) REFERENCES [dbo].[Publication] ([PublicationID])
);


GO
CREATE NONCLUSTERED INDEX [ix_Rules_PublicationID]
    ON [dbo].[Rules]([PublicationID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ix_Rules_EditionID]
    ON [dbo].[Rules]([EditionID] ASC) WITH (FILLFACTOR = 80);

