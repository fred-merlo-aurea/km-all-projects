CREATE TABLE [dbo].[BlastClickRangeByDate] (
    [ClickDate]         CHAR (10) NULL,
    [MinClickId]        INT       NULL,
    [MaxClickId]        INT       NULL,
    [TotalClicks]       INT       NULL,
    [UniqueEmailClicks] INT       NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastClickRangeByDate] TO [ecn5]
    AS [dbo];

