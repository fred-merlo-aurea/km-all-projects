﻿CREATE TABLE [dbo].[HistorySubscription] (
    [HistorySubscriptionID]          INT              IDENTITY (1, 1) NOT NULL,
    [SubscriptionID]                 INT              NOT NULL,
    [PublisherID]                    INT              NOT NULL,
    [SubscriberID]                   INT              NOT NULL,
    [PublicationID]                  INT              NOT NULL,
    [ActionID_Current]               INT              NOT NULL,
    [ActionID_Previous]              INT              NULL,
    [SubscriptionStatusID]           INT              NOT NULL,
    [IsPaid]                         BIT              NOT NULL,
    [QSourceID]                      INT              NULL,
    [QSourceDate]                    DATE             NULL,
    [DeliverabilityID]               INT              NULL,
    [IsSubscribed]                   BIT              NOT NULL,
    [SubscriberSourceCode]           varchar(256)     NULL,
    [Copies]                         INT              CONSTRAINT [DF_SubscriptionHistory_Copies] DEFAULT ((1)) NOT NULL,
    [OriginalSubscriberSourceCode]   varchar(256)     NULL,
    [SubscriptionDateCreated]        DATETIME         NOT NULL,
    [SubscriptionDateUpdated]        DATETIME         NULL,
    [SubscriptionCreatedByUserID]    INT              NOT NULL,
    [SubscriptionUpdatedByUserID]    INT              NULL,
	[AccountNumber]					 VARCHAR(50)			  NULL,
	[GraceIssues]					 INT			  NULL,
	[IsNewSubscription]				 INT			  NULL,
	[MemberGroup]					 VARCHAR(256)		  NULL,
	[OnBehalfOf]					 VARCHAR(256)	  NULL,
	[Par3cID]						 INT			  NULL,
	[SequenceID]					 INT			  NULL,
	[SubsrcTypeID]					 INT			  NULL,
	[Verify]					     VARCHAR(256)	  NULL,
	[IsActive]						 BIT			  NULL,
    [ExternalKeyID]                  INT              NULL,
    [FirstName]                      VARCHAR (50)     NULL,
    [LastName]                       VARCHAR (50)     NULL,
    [Company]                        VARCHAR (100)    NULL,
    [Title]                          VARCHAR (255)    NULL,
    [Occupation]                     VARCHAR (50)     NULL,
    [AddressTypeID]                  INT              NULL,
    [Address1]                       VARCHAR (100)    NULL,
    [Address2]                       VARCHAR (100)    NULL,
	[Address3]                       VARCHAR (100)    NULL,
    [City]                           VARCHAR (50)     NULL,
    [RegionCode]                     VARCHAR (50)     NULL,
    [RegionID]                       INT              NULL,
    [ZipCode]                        VARCHAR (50)     NULL,
    [Plus4]                          CHAR (10)        NULL,
    [CarrierRoute]                   VARCHAR (10)     NULL,
    [County]                         VARCHAR (50)     NULL,
    [Country]                        VARCHAR (50)     NULL,
    [CountryID]                      INT              NULL,
    [Latitude]                       DECIMAL (18, 15) NULL,
    [Longitude]                      DECIMAL (18, 15) NULL,
    [IsAddressValidated]             BIT              NOT NULL,
    [AddressValidationDate]          DATETIME         NULL,
    [AddressValidationSource]        VARCHAR (50)     NULL,
    [AddressValidationMessage]       VARCHAR (MAX)    NULL,
    [Email]                          VARCHAR (255)    NULL,
    [Phone]                          VARCHAR(25)        NULL,
    [Fax]                            VARCHAR(25)        NULL,
    [Mobile]                         VARCHAR(25)        NULL,
    [Website]                        VARCHAR (255)    NULL,
    [Birthdate]                      DATE             NULL,
    [Age]                            INT              NULL,
    [Income]                         VARCHAR (50)     NULL,
    [Gender]                         VARCHAR (50)     NULL,
    [SubscriberDateCreated]          DATETIME         NOT NULL,
    [SubscriberDateUpdated]          DATETIME         NULL,
    [SubscriberCreatedByUserID]      INT              NOT NULL,
    [SubscriberUpdatedByUserID]      INT              NULL,
    [DateCreated]                    DATETIME         NOT NULL,
    [CreatedByUserID]                INT              NOT NULL,
	[IsLocked]						 BIT			  NULL,
	[LockDate]						 DATETIME		  NULL,
	[LockDateRelease]				 DATETIME		  NULL,
	[LockedByUserID]				 INT			  NULL,
	[PhoneExt]						 VARCHAR(25)	  NULL,
	[IsUadUpdated]					 BIT			  CONSTRAINT [DF_HistorySubscription_IsUadUpdated] DEFAULT ((0)) NOT NULL,
	[UadUpdatedDate]				 DATETIME		  NULL
    CONSTRAINT [PK_HistorySubscription] PRIMARY KEY CLUSTERED ([HistorySubscriptionID] ASC) WITH (FILLFACTOR = 80)
);

