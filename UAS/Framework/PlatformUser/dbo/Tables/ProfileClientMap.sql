CREATE TABLE [dbo].[ProfileClientMap] (
    [ProfileID]       INT      NOT NULL,
    [ClientID]        INT      NOT NULL,
    [IsActive]        BIT      NOT NULL,
    [DateCreated]     DATETIME NOT NULL,
    [DateUpdated]     DATETIME NULL,
    [CreatedByUserID] INT      NOT NULL,
    [UpdatedByUserID] INT      NULL,
    CONSTRAINT [PK_ProfileClientMap] PRIMARY KEY CLUSTERED ([ProfileID] ASC, [ClientID] ASC)
);

