CREATE TABLE [dbo].[OverwritedataPostValue] (
    [OverwritedataValue_Seq_ID] INT           IDENTITY (1, 1) NOT NULL,
    [Control_ID]                  INT           NOT NULL,
    [Rule_Seq_ID]           INT           NOT NULL,
    [Value]                        NVARCHAR (30) NOT NULL,
    CONSTRAINT [PK_OverwritedataPostValue] PRIMARY KEY CLUSTERED ([OverwritedataValue_Seq_ID] ASC),
	CONSTRAINT [FK_Control_OverwritedataPost] FOREIGN KEY ([Control_ID]) REFERENCES [dbo].[Control] ([Control_ID]),
	CONSTRAINT [FK_Rule_OverwritedataPost] FOREIGN KEY ([Rule_Seq_ID]) REFERENCES [dbo].[Rule]([Rule_Seq_ID]) ON DELETE CASCADE

);

