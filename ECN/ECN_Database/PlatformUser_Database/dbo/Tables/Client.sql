CREATE TABLE [dbo].[Client] (
    [ClientID]                     INT              IDENTITY (1, 1) NOT NULL,
    [AccessKey]                    UNIQUEIDENTIFIER NOT NULL,
    [ClientName]                   VARCHAR (100)    NOT NULL,
    [DisplayName]                  VARCHAR (100)    NULL,
    [ClientCode]                   VARCHAR (15)     NOT NULL,
    [ClientTestDBConnectionString] VARCHAR (255)    NOT NULL,
    [ClientLiveDBConnectionString] VARCHAR (255)    NOT NULL,
    [IsActive]                     BIT              CONSTRAINT [DF_Client_IsActive] DEFAULT ((1)) NULL,
    [IgnoreUnknownFiles]           BIT              CONSTRAINT [DF_Client_IgnoreUnknownFiles] DEFAULT ((0)) NOT NULL,
    [AccountManagerEmails]         VARCHAR (500)    CONSTRAINT [DF_Client_AccountManagerEmails] DEFAULT ('') NOT NULL,
    [ClientEmails]                 VARCHAR (1000)   CONSTRAINT [DF_Client_ClientEmails] DEFAULT ('') NOT NULL,
    [DateCreated]                  DATETIME         NOT NULL,
    [DateUpdated]                  DATETIME         NULL,
    [CreatedByUserID]              INT              NOT NULL,
    [UpdatedByUserID]              INT              NULL,
    [HasPaid]                      BIT              CONSTRAINT [DF__Client__HasPaid__6754599E] DEFAULT ((0)) NOT NULL,
    [IsKMClient]                   BIT              CONSTRAINT [DF__Client__IsKMClie__68487DD7] DEFAULT ('true') NOT NULL,
    [ParentClientId]               INT              CONSTRAINT [DF__Client__ParentCl__693CA210] DEFAULT ((0)) NOT NULL,
    [HasChildren]                  BIT              CONSTRAINT [DF__Client__HasChild__6A30C649] DEFAULT ('false') NOT NULL,
	[IsAMS]						   BIT              CONSTRAINT [DF__Client__IsAMS] DEFAULT ('false') NOT NULL,
	[FtpFolder]					   VARCHAR(50)		NULL,
    CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED ([ClientID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [IX_ClientAccessKeyUnique] UNIQUE NONCLUSTERED ([AccessKey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AccessKey is unique for Client', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Client', @level2type = N'CONSTRAINT', @level2name = N'IX_ClientAccessKeyUnique';

