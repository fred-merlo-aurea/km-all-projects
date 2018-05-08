CREATE TABLE [dbo].[CustomerLicense] (
    [CLID]            INT          IDENTITY (1, 1) NOT NULL,
    [CustomerID]      INT          NULL,
    [QuoteItemID]     INT          CONSTRAINT [DF_CustomerLicenses_QuoteItemID] DEFAULT ((-1)) NOT NULL,
    [LicenseTypeCode] VARCHAR (50) NULL,
    [LicenseLevel]    CHAR (4)     CONSTRAINT [DF_CustomerLicenses_LicenseLevel] DEFAULT ('CUST') NOT NULL,
    [Quantity]        INT          NULL,
    [Used]            INT          NULL,
    [ExpirationDate]  DATETIME     NULL,
    [AddDate]         DATETIME     NULL,
    [IsActive]        BIT          CONSTRAINT [DF_CustomerLicenses_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedUserID]   INT          NULL,
    [CreatedDate]     DATETIME     CONSTRAINT [DF_CustomerLicense_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedUserID]   INT          NULL,
    [UpdatedDate]     DATETIME     NULL,
    [IsDeleted]       BIT          CONSTRAINT [DF_CustomerLicenses_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CustomerLicenses] PRIMARY KEY CLUSTERED ([CLID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_CustomerLicenses_1]
    ON [dbo].[CustomerLicense]([CustomerID] ASC, [LicenseTypeCode] ASC, [ExpirationDate] ASC, [AddDate] ASC) WITH (FILLFACTOR = 80);

