CREATE TABLE [dbo].[Profiles] (
    [ProfileID]    INT           IDENTITY (1, 1) NOT NULL,
    [EmailAddress] VARCHAR (50)  NULL,
    [CustomerID]   INT           NULL,
    [SegmentValue] VARCHAR (50)  NULL,
    [Data]         VARCHAR (255) NULL,
    [Referer]      VARCHAR (255) NULL,
    [ProfileDate]  DATETIME      NULL,
    CONSTRAINT [PK_Profiles] PRIMARY KEY CLUSTERED ([ProfileID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[Profiles] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Profiles] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Profiles] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Profiles] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Profiles] TO [reader]
    AS [dbo];

