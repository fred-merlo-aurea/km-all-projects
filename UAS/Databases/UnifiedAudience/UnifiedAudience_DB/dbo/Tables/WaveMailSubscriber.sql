CREATE TABLE [dbo].[WaveMailSubscriber]
(
	[WaveMailingID] INT NOT NULL, 
    [PubSubscriptionID] INT NOT NULL, 
    [WaveNumber] INT NOT NULL, 
	CONSTRAINT [PK_WaveMailSubscriber] PRIMARY KEY CLUSTERED ([WaveMailingID] ASC, [PubSubscriptionID] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE NONCLUSTERED INDEX [IX_WaveMailSubscriber_PubSubscriptionID]
    ON [dbo].[WaveMailSubscriber]([PubSubscriptionID] ASC);
GO
