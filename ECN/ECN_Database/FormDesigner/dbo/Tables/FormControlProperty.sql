CREATE TABLE [dbo].[FormControlProperty] (
    [FormControlProperty_Seq_ID] INT            IDENTITY (1, 1) NOT NULL,
    [Control_ID]                 INT            NOT NULL,
    [ControlProperty_ID]         INT            NOT NULL,
    [Value]                      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_FormControlProperty] PRIMARY KEY CLUSTERED ([FormControlProperty_Seq_ID] ASC),
    CONSTRAINT [FK_Control_FormControlProperty] FOREIGN KEY ([Control_ID]) REFERENCES [dbo].[Control] ([Control_ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ControlProperty_FormControlProperty] FOREIGN KEY ([ControlProperty_ID]) REFERENCES [dbo].[ControlProperty] ([ControlProperty_Seq_ID])
);

