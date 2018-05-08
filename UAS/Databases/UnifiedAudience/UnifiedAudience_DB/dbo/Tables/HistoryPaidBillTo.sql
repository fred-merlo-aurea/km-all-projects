CREATE TABLE [dbo].[HistoryPaidBillTo] (
    [HistoryPaidBillToID]      INT              IDENTITY (1, 1) NOT NULL,
    [PaidBillToID]             INT              NOT NULL,
    [SubscriptionPaidID]       INT              NOT NULL,
    [PubSubscriptionID]           INT              NOT NULL,
    [FirstName]                VARCHAR (50)     NULL,
    [LastName]                 VARCHAR (50)     NULL,
    [Company]                  VARCHAR (100)    NULL,
    [Title]                    VARCHAR (255)    NULL,
    [AddressTypeID]            INT              NULL,
    [Address1]                 VARCHAR (100)    NULL,
    [Address2]                 VARCHAR (100)    NULL,
	[Address3]                 VARCHAR (100)    NULL,
    [City]                     VARCHAR (50)     NULL,
    [RegionCode]               VARCHAR (50)     NULL,
    [RegionID]                 INT              NULL,
    [ZipCode]                  VARCHAR (50)     NULL,
    [Plus4]                    CHAR (10)        NULL,
    [CarrierRoute]             VARCHAR (10)     NULL,
    [County]                   VARCHAR (50)     NULL,
    [Country]                  VARCHAR (50)     NULL,
    [CountryID]                INT              NULL,
    [Latitude]                 DECIMAL (18, 15) NULL,
    [Longitude]                DECIMAL (18, 15) NULL,
    [IsAddressValidated]       BIT              NOT NULL,
    [AddressValidationDate]    DATETIME         NULL,
    [AddressValidationSource]  VARCHAR (50)     NULL,
    [AddressValidationMessage] VARCHAR (MAX)    NULL,
    [Email]                    VARCHAR (255)    NULL,
    [Phone]                    CHAR (50)        NULL,
    [Fax]                      CHAR (50)        NULL,
    [Mobile]                   CHAR (50)        NULL,
    [Website]                  VARCHAR (255)    NULL,
    [DateCreated]              DATETIME         NOT NULL,
    [CreatedByUserID]          INT              NOT NULL,
    CONSTRAINT [PK_HistoryPaidBillTo] PRIMARY KEY CLUSTERED ([HistoryPaidBillToID] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE NONCLUSTERED INDEX [IX_HistoryPaidBillTo_PubSubscriptionID]
    ON [dbo].[HistoryPaidBillTo]([PubSubscriptionID] ASC);
GO

