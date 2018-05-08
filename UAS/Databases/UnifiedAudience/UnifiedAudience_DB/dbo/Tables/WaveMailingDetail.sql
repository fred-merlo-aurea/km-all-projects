CREATE TABLE [dbo].[WaveMailingDetail]
(
	[WaveMailingDetailID]     INT				IDENTITY (1, 1) NOT NULL, 
	[WaveMailingID]			   INT				NOT NULL,
	[PubSubscriptionID]			   INT				NOT NULL,
	[SubscriptionID]		   INT				NOT NULL,
	[Demo7]		   VARCHAR(2)				NULL,
	[PubCategoryID]         INT				NULL,
    [PubTransactionID]        INT				NULL,
	[Copies]                   INT				NULL,
	[FirstName]				   VARCHAR(50)		NULL,
	[LastName]				   VARCHAR(50)		NULL,
	[Title]					   VARCHAR(255)		NULL,
	[Company]				   VARCHAR(100)		NULL,
	[AddressTypeID]			   INT				NULL,
	[Address1]				   VARCHAR(100)		NULL,
	[Address2]                 VARCHAR (100)    NULL,
	[Address3]                 VARCHAR (100)    NULL,
    [City]                     VARCHAR (50)     NULL,
    [RegionCode]               VARCHAR (50)     NULL,
    [RegionID]                 INT              NULL,
    [ZipCode]                  VARCHAR (50)     NULL,
    [Plus4]                    VARCHAR (10)     NULL,
    [County]                   VARCHAR (50)     NULL,
    [Country]                  VARCHAR (50)     NULL,
    [CountryID]                INT              NULL,
	[Email]                    VARCHAR (255)    NULL,
    [Phone]                    VARCHAR (25)     NULL,
    [Fax]                      VARCHAR (25)     NULL,
    [Mobile]                   VARCHAR (25)     NULL,
	[DateCreated]              DATETIME         NOT NULL,
    [DateUpdated]              DATETIME         NULL,
    [CreatedByUserID]          INT              NOT NULL,
    [UpdatedByUserID]          INT              NULL, 
    [SubscriptionStatusID] INT NULL, 
    [IsSubscribed] BIT NULL, 
    [IsPaid] BIT NULL, 
    [PhoneExt] VARCHAR(25) NULL
)
GO

CREATE NONCLUSTERED INDEX [IX_WaveMailingDetail_PubSubscriptionID]
    ON [dbo].[WaveMailingDetail]([PubSubscriptionID] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_WaveMailingDetail_SubscriptionID]
    ON [dbo].[WaveMailingDetail]([SubscriptionID] ASC);
GO