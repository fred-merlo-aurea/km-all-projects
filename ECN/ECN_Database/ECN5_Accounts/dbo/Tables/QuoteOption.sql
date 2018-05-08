CREATE TABLE [dbo].[QuoteOption] (
    [QuoteOptionID]     INT            IDENTITY (1, 1) NOT NULL,
    [BaseChannelID]     INT            NOT NULL,
    [Code]              VARCHAR (50)   NOT NULL,
    [Name]              VARCHAR (100)  NOT NULL,
    [Description]       VARCHAR (500)  NOT NULL,
    [Quantity]          INT            NOT NULL,
    [Rate]              FLOAT (53)     NOT NULL,
    [DiscounRate]       FLOAT (53)     CONSTRAINT [DF_QuoteOptions_DiscounRate] DEFAULT ((0)) NULL,
    [AllowedFrequency]  SMALLINT       NOT NULL,
    [LicenseType]       SMALLINT       NOT NULL,
    [PriceType]         SMALLINT       NOT NULL,
    [IsCustomerCredit]  BIT            NOT NULL,
    [ProductIDs]        VARCHAR (47)   NULL,
    [ProductFeatureIDs] VARCHAR (399)  NULL,
    [Services]          VARCHAR (1000) NULL,
    [CreatedUserID]     INT            NULL,
    [CreatedDate]       DATETIME       CONSTRAINT [DF_QuoteOption_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedUserID]     INT            NULL,
    [UpdatedDate]       DATETIME       NULL,
    [IsDeleted]         BIT            CONSTRAINT [DF_QuoteOption_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_QuoteOptions] PRIMARY KEY CLUSTERED ([QuoteOptionID] ASC) WITH (FILLFACTOR = 80)
);

