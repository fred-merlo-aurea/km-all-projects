CREATE TABLE [dbo].[BlastOpenCounts] (
    [BlastId]           INT      NOT NULL,
    [TotalOpens]        INT      NULL,
    [UniqueOpens]       INT      NULL,
    [CreatedDate]       DATETIME NULL,
    [UpdatedDate]       DATETIME NULL,
    [MobileOpens]       INT      NULL,
    [MobileUniqueOpens] INT      NULL,
    PRIMARY KEY CLUSTERED ([BlastId] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastOpenCounts] TO [ecn5]
    AS [dbo];

