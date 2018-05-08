CREATE TABLE [dbo].[Control] (
    [Control_ID]  INT              IDENTITY (1, 1) NOT NULL,
    [Form_Seq_ID] INT              NOT NULL,
    [Order]       INT              NOT NULL,
    [Type_Seq_ID] INT              NOT NULL,
    [FieldLabel]  NVARCHAR (MAX)   NULL,
    [HTMLID]      UNIQUEIDENTIFIER NOT NULL,
    [FieldID]     INT              NULL,
    [FieldLabelHTML] NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Control] PRIMARY KEY CLUSTERED ([Control_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ControlType_Control] FOREIGN KEY ([Type_Seq_ID]) REFERENCES [dbo].[ControlType] ([ControlType_Seq_ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Form_Control] FOREIGN KEY ([Form_Seq_ID]) REFERENCES [dbo].[Form] ([Form_Seq_ID]) ON DELETE CASCADE
);

