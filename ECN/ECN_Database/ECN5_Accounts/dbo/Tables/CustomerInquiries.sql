CREATE TABLE [dbo].[CustomerInquiries] (
    [CustomerInquirieID]     INT            IDENTITY (1, 1) NOT NULL,
    [CustomerID]             INT            NOT NULL,
    [LicenseID]              INT            NOT NULL,
    [FirstName]              VARCHAR (50)   NOT NULL,
    [LastName]               VARCHAR (50)   NOT NULL,
    [DateOfInquirie]         DATETIME       NOT NULL,
    [Notes]                  VARCHAR (5000) NOT NULL,
    [CustomerServiceStaffID] INT            NOT NULL,
    CONSTRAINT [PK_CustomerInquiries] PRIMARY KEY CLUSTERED ([CustomerInquirieID] ASC) WITH (FILLFACTOR = 80)
);

