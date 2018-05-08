CREATE TABLE [dbo].[BlastClickCounts] (
    [BlastId]      INT           NOT NULL,
    [URL]          VARCHAR (896) NOT NULL,
    [TotalClicks]  INT           NULL,
    [UniqueClicks] INT           NULL,
    [CreatedDate]  DATETIME      NULL,
    [UpdatedDate]  DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([BlastId] ASC, [URL] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastClickCounts] TO [ecn5]
    AS [dbo];

