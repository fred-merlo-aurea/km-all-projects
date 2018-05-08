CREATE TABLE [dbo].[BlastSendCounts] (
    [BlastId]     INT      NOT NULL,
    [TotalSends]  INT      NULL,
    [UniqueSends] INT      NULL,
    [CreatedDate] DATETIME NULL,
    [UpdatedDate] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([BlastId] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastSendCounts] TO [ecn5]
    AS [dbo];

