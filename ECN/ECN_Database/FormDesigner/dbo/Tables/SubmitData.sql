CREATE TABLE [dbo].[SubmitData] (
    [SubmitData_Seq_ID]    INT            IDENTITY (1, 1) NOT NULL,
    [SubmitHistory_Seq_ID] INT            NOT NULL,
    [Control_ID]           INT            NOT NULL,
    [Value]                NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_SubmitData] PRIMARY KEY CLUSTERED ([SubmitData_Seq_ID] ASC),
    CONSTRAINT [FK_SubmitHistory_SubmitData] FOREIGN KEY ([SubmitHistory_Seq_ID]) REFERENCES [dbo].[SubmitHistory] ([SubmitHistory_Seq_ID]) ON DELETE CASCADE
);

