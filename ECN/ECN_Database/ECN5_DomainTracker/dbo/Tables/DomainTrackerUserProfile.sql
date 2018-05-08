CREATE TABLE [dbo].[DomainTrackerUserProfile] (
    [ProfileID]     INT           IDENTITY (1, 1) NOT NULL,
    [EmailAddress]  VARCHAR (500) NULL,
    [CustomerID]    INT           NULL,
    [CreatedDate]   DATETIME      NULL,
    [CreatedUserID] INT           NULL,
    [UpdatedDate]   DATETIME      NULL,
    [UpdatedUserID] INT           NULL,
    [IsDeleted]     BIT           NULL,
    [BaseChannelID] INT           DEFAULT ((-1)) NOT NULL,
    [ConvertedDate] DATETIME NULL, 
    [IsKnown] BIT NULL, 
    CONSTRAINT [PK_DomainTrackerUserProfiles] PRIMARY KEY CLUSTERED ([ProfileID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IDX_DomainTrackerUserProfile_EmailAddress]
    ON [dbo].[DomainTrackerUserProfile]([EmailAddress] ASC);

