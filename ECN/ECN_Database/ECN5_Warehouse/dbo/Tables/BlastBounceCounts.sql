CREATE TABLE [dbo].[BlastBounceCounts] (
    [BlastId]       INT          NOT NULL,
    [BounceCode]    VARCHAR (30) NOT NULL,
    [TotalBounces]  INT          NULL,
    [UniqueBounces] INT          NULL,
    [CreatedDate]   DATETIME     NULL,
    [UpdatedDate]   DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([BlastId] ASC, [BounceCode] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastBounceCounts] TO [ecn5]
    AS [dbo];

