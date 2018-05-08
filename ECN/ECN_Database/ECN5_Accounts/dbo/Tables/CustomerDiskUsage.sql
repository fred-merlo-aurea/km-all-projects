CREATE TABLE [dbo].[CustomerDiskUsage] (
    [UsageID]       INT          IDENTITY (1, 1) NOT NULL,
    [ChannelID]     INT          NULL,
    [CustomerID]    INT          NULL,
    [SizeInBytes]   VARCHAR (50) NULL,
    [DateMonitored] DATETIME     CONSTRAINT [DF_CustomerDiskUsage_DateMonitored] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CustomerDiskUsage] PRIMARY KEY CLUSTERED ([UsageID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_CustomerDiskUsage_CustomerID]
    ON [dbo].[CustomerDiskUsage]([CustomerID] ASC)
    INCLUDE([ChannelID], [SizeInBytes]) WITH (FILLFACTOR = 80);

