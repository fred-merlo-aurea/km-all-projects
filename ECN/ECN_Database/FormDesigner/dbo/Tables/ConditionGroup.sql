CREATE TABLE [dbo].[ConditionGroup] (
    [ConditionGroup_Seq_ID] INT IDENTITY (1, 1) NOT NULL,
    [MainGroup_ID]          INT NULL,
    [LogicGroup]            BIT NOT NULL,
    CONSTRAINT [PK_ConditionGroup] PRIMARY KEY CLUSTERED ([ConditionGroup_Seq_ID] ASC),
    CONSTRAINT [FK_ConditionGroup_ConditionGroup] FOREIGN KEY ([MainGroup_ID]) REFERENCES [dbo].[ConditionGroup] ([ConditionGroup_Seq_ID])
);

