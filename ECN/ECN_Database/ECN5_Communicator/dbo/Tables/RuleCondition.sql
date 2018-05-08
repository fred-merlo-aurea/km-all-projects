CREATE TABLE [dbo].[RuleCondition] (
    [RuleConditionID] INT            IDENTITY (1, 1) NOT NULL,
    [RuleID]          INT            NULL,
    [Field]           VARCHAR (100)  NULL,
    [DataType]        VARCHAR (50)   NULL,
    [Comparator]      VARCHAR (50)   NULL,
    [Value]           VARCHAR (2000) NULL,
    [CreatedDate]     DATETIME       NULL,
    [CreatedUserID]   INT            NULL,
    [UpdatedDate]     DATETIME       NULL,
    [UpdatedUserID]   INT            NULL,
    [IsDeleted]       BIT            NULL,
    CONSTRAINT [PK_RuleConditions] PRIMARY KEY CLUSTERED ([RuleConditionID] ASC) WITH (FILLFACTOR = 80)
);

