CREATE TABLE [dbo].[Condition] (
    [Condition_Seq_ID]      INT           IDENTITY (1, 1) NOT NULL,
    [Control_ID]            INT           NOT NULL,
    [ConditionGroup_Seq_ID] INT           NOT NULL,
    [Operation_ID]          INT           NOT NULL,
    [Value]                 NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Condition] PRIMARY KEY CLUSTERED ([Condition_Seq_ID] ASC),
    CONSTRAINT [FK_ConditionGroup_Condition] FOREIGN KEY ([ConditionGroup_Seq_ID]) REFERENCES [dbo].[ConditionGroup] ([ConditionGroup_Seq_ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Control_Condition] FOREIGN KEY ([Control_ID]) REFERENCES [dbo].[Control] ([Control_ID])
);

