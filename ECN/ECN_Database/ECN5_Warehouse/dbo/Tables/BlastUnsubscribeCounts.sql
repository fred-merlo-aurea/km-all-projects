CREATE TABLE [dbo].[BlastUnsubscribeCounts] (
    [BlastId]            INT          NOT NULL,
    [UnsubscribeCode]    VARCHAR (30) NOT NULL,
    [TotalUnsubscribed]  INT          NULL,
    [UniqueUnsubscribed] INT          NULL,
    [CreatedDate]        DATETIME     NULL,
    [UpdatedDate]        DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([BlastId] ASC, [UnsubscribeCode] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastUnsubscribeCounts] TO [ecn5]
    AS [dbo];

