CREATE TABLE [dbo].[FormResult] (
    [FormResult_Seq_ID] INT            IDENTITY (1, 1) NOT NULL,
    [Form_Seq_ID]       INT            NOT NULL,
    [ResultType]        INT            NOT NULL,
    [Message]           NVARCHAR (MAX) NULL,
    [URL]               NVARCHAR (1792) NULL,
	[JsMessage] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_FormResult] PRIMARY KEY CLUSTERED ([FormResult_Seq_ID] ASC),
    CONSTRAINT [FK_Form_FormResult] FOREIGN KEY ([Form_Seq_ID]) REFERENCES [dbo].[Form] ([Form_Seq_ID]) ON DELETE CASCADE,
    CONSTRAINT [UC_ResultType] UNIQUE NONCLUSTERED ([Form_Seq_ID] ASC, [ResultType] ASC)
);

