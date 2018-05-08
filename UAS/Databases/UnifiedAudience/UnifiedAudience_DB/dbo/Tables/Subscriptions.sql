CREATE TABLE [dbo].[Subscriptions] (
    [SubscriptionID]    INT              IDENTITY (1, 1) NOT NULL,
    Sequence int not null,
	FName varchar(100) null,
	LName varchar(100) null,
	Title varchar(100) null,
	Company	varchar(100) null,
	Address	varchar(255) null,
	MailStop varchar(255) null,
	City varchar(50) null,
	State varchar(50) null,
	Zip	varchar(10) null,
	Plus4 varchar(4) null,
	ForZip varchar(50) null,
	County varchar(50) null,
	Country varchar(100) null,
	CountryID int null,
	Phone varchar(100) null,
	PhoneExists	bit null,
	Fax	varchar(100) null,
	FaxExists bit null,
	Email varchar(100) null,
	EmailExists	bit null,
	CategoryID int null,
	TransactionID int null,
	TransactionDate smalldatetime null,
	QDate datetime null,
	QSourceID int null,
	RegCode	varchar(5) null,
	Verified varchar(100) null,
	SubSrc varchar(25) null,
	OrigsSrc varchar(25) null,
	Par3C varchar(10) null,
	Source varchar(50) null,
	Priority varchar(4) null,
	IGrp_Cnt int null,
	CGrp_No UNIQUEIDENTIFIER null,
	CGrp_Cnt int null,
	StatList bit null,
	Sic varchar(8) null,
	SicCode varchar(20) null,
	Gender varchar(1024) null,
	IGrp_Rank varchar(2) null,
	CGrp_Rank varchar(2) null,
	Address3 varchar(255) null,
	Home_Work_Address varchar(10) null,
	PubIDs varchar(2000) null,
	Demo7 varchar(1) null,
	IsExcluded bit null,
	Mobile varchar(30) null,
	Latitude decimal(18,15) null,
	Longitude decimal(18,15) null,
	IsLatLonValid bit null,
	LatLonMsg varchar(1000) null,
	Score int null,
	IGrp_No uniqueidentifier not null,
	Notes varchar(2000) null,
	[DateCreated]             DATETIME CONSTRAINT [DF_Subscriptions_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]             DATETIME         NULL,
    [CreatedByUserID]         INT              NULL,
    [UpdatedByUserID]         INT         NULL,
	AddressTypeCodeId int null,
	AddressLastUpdatedDate datetime null,
	AddressUpdatedSourceTypeCodeId int null,
	IsActive BIT NULL,
	IsMailable BIT NULL,
	ExternalKeyId INT NULL,
	AccountNumber VARCHAR (50) NULL,
	EmailID INT NULL,
    [MailPermission] BIT NULL, 
    [FaxPermission] BIT NULL, 
    [PhonePermission] BIT NULL, 
    [OtherProductsPermission] BIT NULL, 
    [ThirdPartyPermission] BIT NULL, 
    [EmailRenewPermission] BIT NULL, 
    [TextPermission] BIT NULL, 
    [OriginatedPubID]  INT NULL,
    CONSTRAINT [PK_Subscriptions] PRIMARY KEY CLUSTERED ([SubscriptionID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE NONCLUSTERED INDEX [IDX_Subscriptions_CategoryID]
    ON [dbo].[Subscriptions]([CategoryID] ASC) WITH (FILLFACTOR = 70);
GO

CREATE NONCLUSTERED INDEX [IDX_Subscriptions_CountryID]
    ON [dbo].[Subscriptions]([CountryID] ASC) WITH (FILLFACTOR = 70);
GO

-- recreated with new include
--CREATE NONCLUSTERED INDEX [IDX_Subscriptions_Email]
--    ON [dbo].[Subscriptions]([Email] ASC) WITH (FILLFACTOR = 70);
--GO

CREATE INDEX [IDX_Subscriptions_Email_includes] 
	ON [dbo].[Subscriptions] ([EMAIL]) INCLUDE ([FNAME], [LNAME], [COMPANY], [ADDRESS], [PHONE], [IGRP_NO])  WITH (FILLFACTOR = 50);
GO

CREATE NONCLUSTERED INDEX [DIX_Subscriptions_EmailExists]
    ON [dbo].[Subscriptions]([EmailExists] ASC) WITH (FILLFACTOR = 70);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IDX_Subscriptions_IGRP_NO]
    ON [dbo].[Subscriptions]([IGRP_NO] ASC) WITH (FILLFACTOR = 70);
GO

CREATE NONCLUSTERED INDEX [IDX_Subscriptions_IsLatLonValid_Latitude_Longitude]
    ON [dbo].[Subscriptions]([IsLatLonValid] ASC, [Latitude] ASC, [Longitude] ASC) WITH (FILLFACTOR = 70);
GO

CREATE NONCLUSTERED INDEX [IDX_Subscriptions_TransactionID]
    ON [dbo].[Subscriptions]([TransactionID] ASC) WITH (FILLFACTOR = 70);
GO

CREATE NONCLUSTERED INDEX [IDX_Subscritions_QsourceID]
    ON [dbo].[Subscriptions]([QSourceID] ASC) WITH (FILLFACTOR = 70);
GO

CREATE NONCLUSTERED INDEX [IDX_Subscriptions_Address_includes] 
	ON [dbo].[Subscriptions] ([ADDRESS]) INCLUDE ([FNAME], [LNAME], [TITLE], [COMPANY], [IGRP_NO])  WITH (FILLFACTOR = 50);
GO

CREATE NONCLUSTERED INDEX [IDX_Subscriptions_company_includes] 
	ON [dbo].[Subscriptions] ([COMPANY]) INCLUDE ([EmailExists], [QDate])  WITH (FILLFACTOR = 50);
GO

CREATE NONCLUSTERED INDEX [IDX_Subscriptions_FName_LName_includes] 
	ON [dbo].[Subscriptions] ([FNAME], [LNAME]) INCLUDE ([COMPANY], [ADDRESS], [PHONE], [EMAIL], [IGRP_NO]) WITH (FILLFACTOR = 50);
GO

CREATE NONCLUSTERED INDEX [IDX_Subscriptions_Phone_includes] 
	ON [dbo].[Subscriptions] ([PHONE]) INCLUDE ([FNAME], [LNAME], [IGRP_NO]) WITH (FILLFACTOR = 50);
GO
