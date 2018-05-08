CREATE TABLE [dbo].[BlastBounceByDay] (
    [BlastId]       INT          NULL,
    [Date]          DATE         NULL,
    [BounceCode]    VARCHAR (50) NULL,
    [TotalBounces]  INT          NULL,
    [UniqueBounces] INT          NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastBounceByDay] TO [ecn5]
    AS [dbo];

