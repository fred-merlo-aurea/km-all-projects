CREATE TABLE [dbo].[BlastSuppressCounts] (
    [BlastId]          INT          NOT NULL,
    [SuppressedCode]   VARCHAR (30) NOT NULL,
    [TotalSuppressed]  INT          NULL,
    [UniqueSuppressed] INT          NULL,
    [CreatedDate]      DATETIME     NULL,
    [UpdatedDate]      DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([BlastId] ASC, [SuppressedCode] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastSuppressCounts] TO [ecn5]
    AS [dbo];

