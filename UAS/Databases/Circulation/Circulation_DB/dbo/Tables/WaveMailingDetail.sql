﻿CREATE TABLE [dbo].[WaveMailingDetail]
(
	[WaveMailingDetailID]     INT				IDENTITY (1, 1) NOT NULL, 
	[WaveMailingID]			   INT				NOT NULL,
	[SubscriberID]			   INT				NOT NULL,
	[SubscriptionID]		   INT				NOT NULL,
	[DeliverabilityID]		   INT				NULL,
	[ActionID_Current]         INT				NULL,
    [ActionID_Previous]        INT				NULL,
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
    [UpdatedByUserID]          INT              NULL
)