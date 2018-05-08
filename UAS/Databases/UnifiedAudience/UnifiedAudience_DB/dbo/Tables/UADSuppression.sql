CREATE TABLE [dbo].[UADSuppression] (
    [UADSuppressionID]    INT           IDENTITY (1, 1) NOT NULL,
    [fname]               VARCHAR (100) NULL,
    [lname]               VARCHAR (100) NULL,
    [Title]               VARCHAR (100) NULL,
    [Company]             VARCHAR (100) NULL,
    [Address]             VARCHAR (255) NULL,
    [MailStop]            VARCHAR (255) NULL,
    [City]                VARCHAR (50)  NULL,
    [State]               VARCHAR (50)  NULL,
    [Zip]                 VARCHAR (50)  NULL,
    [Plus4]               VARCHAR (50)  NULL,
    [Phone]               VARCHAR (100) NULL,
    [Fax]                 VARCHAR (100) NULL,
    [Email]               VARCHAR (100) NULL,
    [SuppressionFileName] VARCHAR (100) NOT NULL,
    [isActive]            BIT           NULL,
    [DateCreated]         DATETIME      CONSTRAINT [DF_UADSuppression_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]         DATETIME      NULL,
    [CreatedByUserID]     INT           NOT NULL,
    [UpdatedByUserID]     INT           NULL
);

