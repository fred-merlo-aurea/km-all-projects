CREATE TABLE [dbo].[RequestQueryValue] (
    [RequestQueryValue_Seq_ID] INT           IDENTITY (1, 1) NOT NULL,
    [Control_ID]                  INT           NOT NULL,
    [Rule_Seq_ID]           INT           NOT NULL,
    [Name]                        NVARCHAR (30) NOT NULL,
    CONSTRAINT [PK_RequestQueryValue] PRIMARY KEY CLUSTERED ([RequestQueryValue_Seq_ID] ASC),
	CONSTRAINT [FK_Control_RequestQueryValue] FOREIGN KEY ([Control_ID]) REFERENCES [dbo].[Control] ([Control_ID]),
    CONSTRAINT [FK_Rule_RequestQueryValue] FOREIGN KEY ([Rule_Seq_ID]) REFERENCES [dbo].[Rule]([Rule_Seq_ID]) ON DELETE CASCADE
);

