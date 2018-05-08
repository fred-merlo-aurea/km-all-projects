CREATE TABLE [dbo].[Subscriber] (
    [SubscriberID]             INT              IDENTITY (1, 1) NOT NULL,
    [ExternalKeyID]            INT              NULL,
    [FirstName]                VARCHAR (50)     NULL,
    [LastName]                 VARCHAR (50)     NULL,
    [Company]                  VARCHAR (100)    NULL,
    [Title]                    VARCHAR (255)    NULL,
    [Occupation]               VARCHAR (50)     NULL,
    [AddressTypeID]            INT              NULL,
    [Address1]                 VARCHAR (100)    NULL,
    [Address2]                 VARCHAR (100)    NULL,
	[Address3]                 VARCHAR (100)    NULL,
    [City]                     VARCHAR (50)     NULL,
    [RegionCode]               VARCHAR (50)     NULL,
    [RegionID]                 INT              NULL,
    [ZipCode]                  VARCHAR (50)     NULL,
    [Plus4]                    VARCHAR (10)        NULL,
    [CarrierRoute]             VARCHAR (10)     NULL,
    [County]                   VARCHAR (50)     NULL,
    [Country]                  VARCHAR (50)     NULL,
    [CountryID]                INT              NULL,
    [Latitude]                 DECIMAL (18, 15) NULL,
    [Longitude]                DECIMAL (18, 15) NULL,
    [IsAddressValidated]       BIT              CONSTRAINT [DF_Subscriber_IsAddressValidated] DEFAULT ((0)) NOT NULL,
    [AddressValidationDate]    DATETIME         NULL,
    [AddressValidationSource]  VARCHAR (50)     NULL,
    [AddressValidationMessage] VARCHAR (MAX)    NULL,
    [Email]                    VARCHAR (255)    NULL,
    [Phone]                    VARCHAR (25)     NULL,
    [Fax]                      VARCHAR (25)     NULL,
    [Mobile]                   VARCHAR (25)     NULL,
    [Website]                  VARCHAR (255)    NULL,
    [Birthdate]                DATE             NULL,
    [Age]                      INT              NULL,
    [Income]                   VARCHAR (50)     NULL,
    [Gender]                   VARCHAR (50)     NULL,
    [DateCreated]              DATETIME         NOT NULL,
    [DateUpdated]              DATETIME         NULL,
    [CreatedByUserID]          INT              NOT NULL,
    [UpdatedByUserID]          INT              NULL,
    [tmpSubscriptionID]        INT              NULL,
    [IsLocked]                 BIT              DEFAULT ((0)) NOT NULL,
	[LockedByUserID]		   INT				NULL,
	[LockDate]				   DATETIME			NULL,
	[LockDateRelease]		   DATETIME         NULL,
    [PhoneExt]				   VARCHAR(25)	    NULL, 
    [IsInActiveWaveMailing]	   BIT				DEFAULT ((0)) NOT NULL,
	AddressTypeCodeId int null,
	AddressLastUpdatedDate datetime null,
	AddressUpdatedSourceTypeCodeId int null,
    [WaveMailingID] INT NULL, 
    [IGrp_No] UNIQUEIDENTIFIER NULL, 
    [SFRecordIdentifier] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [PK_Subscriber] PRIMARY KEY CLUSTERED ([SubscriberID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IDX_Address1]
    ON [dbo].[Subscriber]([Address1] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_City]
    ON [dbo].[Subscriber]([City] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_Company]
    ON [dbo].[Subscriber]([Company] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_Email]
    ON [dbo].[Subscriber]([Email] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_FirstName]
    ON [dbo].[Subscriber]([FirstName] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_LastName]
    ON [dbo].[Subscriber]([LastName] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_ZipCode]
    ON [dbo].[Subscriber]([ZipCode] ASC) WITH (FILLFACTOR = 80);

