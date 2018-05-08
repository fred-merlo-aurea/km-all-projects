CREATE TABLE [dbo].[FormStatisticLog] (
    [FormStatisticLog_Seq_ID] BIGINT   IDENTITY (1, 1) NOT NULL,
    [FormStatistic_Seq_ID]    BIGINT   NULL,
    [PageNumber]              INT      NULL,
    [StartPage]               DATETIME NULL,
    [FinishPage]              DATETIME NULL,
    CONSTRAINT [PK_FormStatisticLog] PRIMARY KEY CLUSTERED ([FormStatisticLog_Seq_ID] ASC),
    CONSTRAINT [FK_FormStatisticLog_FormStatistic] FOREIGN KEY ([FormStatistic_Seq_ID]) REFERENCES [dbo].[FormStatistic] ([FormStatistic_Seq_ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

