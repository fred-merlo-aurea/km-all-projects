CREATE TABLE [dbo].[RuleDetails] (
    [RuleDetailID] INT          IDENTITY (1, 1) NOT NULL,
    [RuleID]       INT          NOT NULL,
    [FieldType]    VARCHAR (50) NULL,
    [CompareType]  VARCHAR (50) NULL,
    [FieldName]    VARCHAR (50) NULL,
    [Comparator]   VARCHAR (50) NULL,
    [CompareValue] VARCHAR (50) NULL,
    CONSTRAINT [PK_RuleDetails] PRIMARY KEY CLUSTERED ([RuleDetailID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_RuleDetails_Rules] FOREIGN KEY ([RuleID]) REFERENCES [dbo].[Rules] ([RuleID])
);


GO
CREATE NONCLUSTERED INDEX [ix_RuleDetails_RuleID]
    ON [dbo].[RuleDetails]([RuleID] ASC) WITH (FILLFACTOR = 80);

