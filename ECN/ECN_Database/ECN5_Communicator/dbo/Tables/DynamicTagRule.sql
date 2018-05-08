CREATE TABLE [dbo].[DynamicTagRule] (
    [DynamicTagRuleID] INT      IDENTITY (1, 1) NOT NULL,
    [DynamicTagID]     INT      NULL,
    [RuleID]           INT      NULL,
    [ContentID]        INT      NULL,
    [Priority]         INT      NULL,
    [CreatedUserID]    INT      NULL,
    [CreatedDate]      DATETIME NULL,
    [UpdatedUserID]    INT      NULL,
    [UpdatedDate]      DATETIME NULL,
    [IsDeleted]        BIT      NULL,
    CONSTRAINT [PK_DynamicTagRules] PRIMARY KEY CLUSTERED ([DynamicTagRuleID] ASC) WITH (FILLFACTOR = 80)
);

