CREATE TABLE [dbo].[ControlType] (
    [ControlType_Seq_ID] INT           NOT NULL,
    [Name]               NVARCHAR (50) NOT NULL,
    [MainType_ID]        INT           NULL,
    [KMPaidQueryString] NVARCHAR(50) NULL, 
    CONSTRAINT [PK_ControlType] PRIMARY KEY CLUSTERED ([ControlType_Seq_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ControlType_ControlType] FOREIGN KEY ([MainType_ID]) REFERENCES [dbo].[ControlType] ([ControlType_Seq_ID])
);

