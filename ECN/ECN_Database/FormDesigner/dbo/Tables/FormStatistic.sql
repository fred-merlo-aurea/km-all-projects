CREATE TABLE [dbo].[FormStatistic] (
    [FormStatistic_Seq_ID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Form_Seq_ID]          INT           NULL,
    [Email]                NVARCHAR (60) NULL,
    [StartForm]            DATETIME      NULL,
    [FinishForm]           DATETIME      NULL,
    [TotalPages]           INT           DEFAULT ((0)) NOT NULL,
    [IsSubmitted] BIT NULL, 
    CONSTRAINT [PK_FormStatistic] PRIMARY KEY CLUSTERED ([FormStatistic_Seq_ID] ASC),
    CONSTRAINT [FK_FormStatistic_Form] FOREIGN KEY ([Form_Seq_ID]) REFERENCES [dbo].[Form] ([Form_Seq_ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

