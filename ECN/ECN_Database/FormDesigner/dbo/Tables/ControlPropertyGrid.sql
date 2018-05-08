CREATE TABLE [dbo].[ControlPropertyGrid] (
    [ControlPropertyGrid_Seq_ID] INT            IDENTITY (1, 1) NOT NULL,
    [ControlProperty_ID]         INT            NOT NULL,
    [DataValue]                  NVARCHAR (500)  NULL,
    [DataText]                   NVARCHAR (500) NULL,
    [IsDefault]                  BIT            NULL,
    CONSTRAINT [PK_ControlPropertyGrid] PRIMARY KEY CLUSTERED ([ControlPropertyGrid_Seq_ID] ASC),
    CONSTRAINT [FK_ControlProperty_ControlPropertyGrid] FOREIGN KEY ([ControlProperty_ID]) REFERENCES [dbo].[ControlProperty] ([ControlProperty_Seq_ID])
);

