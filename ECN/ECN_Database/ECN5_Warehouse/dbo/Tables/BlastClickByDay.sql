CREATE TABLE [dbo].[BlastClickByDay] (
    [BlastId]      INT            NULL,
    [URL]          VARCHAR (2000) NULL,
    [Date]         DATE           NULL,
    [TotalClicks]  INT            NULL,
    [UniqueClicks] INT            NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastClickByDay] TO [ecn5]
    AS [dbo];

