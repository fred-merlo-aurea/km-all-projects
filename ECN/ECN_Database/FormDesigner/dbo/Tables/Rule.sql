CREATE TABLE [dbo].[Rule] (
    [Rule_Seq_ID]           INT            IDENTITY (1, 1) NOT NULL,
    [Form_Seq_ID]           INT            NULL,
    [Control_ID]            INT            NULL,
    [ConditionGroup_Seq_ID] INT            NOT NULL,
    [Type]                  INT            NOT NULL,
    [Action]                NVARCHAR (MAX) NULL,
    [UrlToRedirect]         NVARCHAR (1792) NULL,
    [Order]                 INT            NOT NULL,
	[ResultType]            INT            NOT NULL DEFAULT ((0)),
    [NonQualify] INT NULL DEFAULT ((0)), 
    [SuspendpostDB] INT NULL DEFAULT ((0)), 
    [Overwritedatapost] INT NULL DEFAULT ((0)), 
	 [ActionJs] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED ([Rule_Seq_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [chk_FormOrControl] CHECK ([Form_Seq_ID] IS NOT NULL OR [Control_ID] IS NOT NULL),
    CONSTRAINT [FK_ConditionGroup_Rule] FOREIGN KEY ([ConditionGroup_Seq_ID]) REFERENCES [dbo].[ConditionGroup] ([ConditionGroup_Seq_ID]),
    CONSTRAINT [FK_Control_Rule] FOREIGN KEY ([Control_ID]) REFERENCES [dbo].[Control] ([Control_ID]),
    CONSTRAINT [FK_Form_Rule] FOREIGN KEY ([Form_Seq_ID]) REFERENCES [dbo].[Form] ([Form_Seq_ID])
);

