CREATE TABLE [dbo].[Campaign] (
    [CampaignID]    INT           IDENTITY (1, 1) NOT NULL,
    [CreatedUserID] INT           NULL,
    [CustomerID]    INT           NULL,
    [CampaignName]  VARCHAR (100) NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_Campaign_CREATEDDATE] DEFAULT (getdate()) NULL,
    [UpdatedUserID] INT           NULL,
    [UpdatedDate]   DATETIME      NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_Campaign_IsDeleted] DEFAULT ((0)) NULL,
    [DripDesign]    TEXT          NULL,
    [IsArchived] BIT NULL, 
    CONSTRAINT [PK_Campaigns] PRIMARY KEY CLUSTERED ([CampaignID] ASC) WITH (FILLFACTOR = 80)
);

