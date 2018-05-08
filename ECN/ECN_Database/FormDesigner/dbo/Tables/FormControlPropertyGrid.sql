CREATE TABLE [dbo].[FormControlPropertyGrid] (
    [FormControlPropertyGrid_Seq_ID] INT            IDENTITY (1, 1) NOT NULL,
    [Control_ID]                     INT            NOT NULL,
    [ControlProperty_ID]             INT            NOT NULL,
    [DataValue]                      NVARCHAR (500)  NOT NULL,
    [DataText]                       NVARCHAR (500) NULL,
    [IsDefault]                      BIT            NULL,
    [CategoryID] INT NULL, 
    [CategoryName] NVARCHAR(500) NULL, 
    [Order] INT NULL, 
    CONSTRAINT [PK_FormControlPropertyGrid] PRIMARY KEY CLUSTERED ([FormControlPropertyGrid_Seq_ID] ASC),
    CONSTRAINT [FK_Control_FormControlPropertyGrid] FOREIGN KEY ([Control_ID]) REFERENCES [dbo].[Control] ([Control_ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ControlProperty_FormControlPropertyGrid] FOREIGN KEY ([ControlProperty_ID]) REFERENCES [dbo].[ControlProperty] ([ControlProperty_Seq_ID])
);

