CREATE TABLE [dbo].[BlastSendRangeByDate] (
    [SendDate]    CHAR (10) NULL,
    [MinSendId]   INT       NULL,
    [MaxSendId]   INT       NULL,
    [TotalSends]  INT       NULL,
    [UniqueSends] INT       NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastSendRangeByDate] TO [ecn5]
    AS [dbo];

