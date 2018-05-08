CREATE TABLE [dbo].[BlastOpenRangeByDate] (
    [OpenDate]    CHAR (10) NULL,
    [MinOpenId]   INT       NULL,
    [MaxOpenId]   INT       NULL,
    [TotalOpens]  INT       NULL,
    [UniqueOpens] INT       NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastOpenRangeByDate] TO [ecn5]
    AS [dbo];

