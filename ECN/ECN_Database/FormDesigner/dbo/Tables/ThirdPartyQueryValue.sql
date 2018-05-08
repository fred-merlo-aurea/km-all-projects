CREATE TABLE [dbo].[ThirdPartyQueryValue] (
    [ThirdPartyQueryValue_Seq_ID] INT           IDENTITY (1, 1) NOT NULL,
    [Control_ID]                  INT           NOT NULL,
    [FormResult_Seq_ID]           INT           NOT NULL,
    [Name]                        NVARCHAR (30) NOT NULL,
    CONSTRAINT [PK_ThirdPartyQueryValue] PRIMARY KEY CLUSTERED ([ThirdPartyQueryValue_Seq_ID] ASC),
    CONSTRAINT [FK_Control_ThirdPartyQueryValue] FOREIGN KEY ([Control_ID]) REFERENCES [dbo].[Control] ([Control_ID]),
    CONSTRAINT [FK_FormResult_ThirdPartyQueryValue] FOREIGN KEY ([FormResult_Seq_ID]) REFERENCES [dbo].[FormResult] ([FormResult_Seq_ID]) ON DELETE CASCADE
);

