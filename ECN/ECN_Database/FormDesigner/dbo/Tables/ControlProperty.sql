CREATE TABLE [dbo].[ControlProperty] (
    [ControlProperty_Seq_ID] INT            IDENTITY (1, 1) NOT NULL,
    [Type_ID]                INT            NOT NULL,
    [PropertyName]           NVARCHAR (30)  NOT NULL,
    [PropertyType]           INT            NOT NULL,
    [PropertyValues]         NVARCHAR (200) NULL,
    CONSTRAINT [PK_ControlProperty] PRIMARY KEY CLUSTERED ([ControlProperty_Seq_ID] ASC),
    CONSTRAINT [FK_ControlType_ControlProperty] FOREIGN KEY ([Type_ID]) REFERENCES [dbo].[ControlType] ([ControlType_Seq_ID])
);

