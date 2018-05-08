CREATE TABLE [dbo].[Customer] (
    [CustomerID]            INT           IDENTITY (1, 1) NOT NULL,
    [BaseChannelID]         INT           NULL,
    [CommunicatorChannelID] INT           NULL,
    [CollectorChannelID]    INT           NULL,
    [CreatorChannelID]      INT           NULL,
    [PublisherChannelID]    INT           NULL,
    [CharityChannelID]      INT           CONSTRAINT [DF_Customers_CharityChannelID] DEFAULT ((0)) NULL,
    [CustomerName]          VARCHAR (150) NULL,
    [ActiveFlag]            VARCHAR (1)   NULL,
    [CommunicatorLevel]     VARCHAR (50)  NULL,
    [CollectorLevel]        VARCHAR (50)  NULL,
    [CreatorLevel]          VARCHAR (50)  NULL,
    [PublisherLevel]        VARCHAR (50)  NULL,
    [CharityLevel]          VARCHAR (50)  CONSTRAINT [DF_Customers_CharityLevel] DEFAULT ((0)) NULL,
    [AccountsLevel]         VARCHAR (50)  NULL,
    [Address]               VARCHAR (255) NULL,
    [City]                  VARCHAR (255) NULL,
    [State]                 VARCHAR (255) NULL,
    [Country]               VARCHAR (50)  NULL,
    [Zip]                   VARCHAR (50)  NULL,
    [WebAddress]            VARCHAR (255) NULL,
    [Salutation]            VARCHAR (50)  NULL,
    [ContactName]           VARCHAR (255) NULL,
    [FirstName]             VARCHAR (50)  NULL,
    [LastName]              VARCHAR (50)  NULL,
    [ContactTitle]          VARCHAR (255) NULL,
    [Phone]                 VARCHAR (255) NULL,
    [Fax]                   VARCHAR (50)  NULL,
    [Email]                 VARCHAR (255) NULL,
    [TechContact]           VARCHAR (50)  NULL,
    [TechEmail]             VARCHAR (255) NULL,
    [TechPhone]             VARCHAR (255) NULL,
    [SubscriptionsEmail]    VARCHAR (255) NULL,
    [CustomerType]          VARCHAR (50)  NULL,
    [DemoFlag]              VARCHAR (1)   CONSTRAINT [DF_Customers_DemoFlag] DEFAULT ('N') NULL,
    [AccountExecutiveID]    INT           NULL,
    [AccountManagerID]      INT           NULL,
    [IsStrategic]           BIT           CONSTRAINT [DF_Customers_IsStrategic] DEFAULT ((0)) NULL,
    [customer_udf1]         VARCHAR (255) NULL,
    [customer_udf2]         VARCHAR (255) NULL,
    [customer_udf3]         VARCHAR (255) NULL,
    [customer_udf4]         VARCHAR (255) NULL,
    [customer_udf5]         VARCHAR (255) NULL,
    [BlastConfigID]         INT           NULL,
    [BounceThreshold]       INT           NULL,
    [TextPowerKWD]          VARCHAR (50)  NULL,
    [TextPowerWelcomeMsg]   VARCHAR (100) NULL,
    [CreatedUserID]         INT           NULL,
    [CreatedDate]           DATETIME      CONSTRAINT [DF_Customer_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedUserID]         INT           NULL,
    [UpdatedDate]           DATETIME      NULL,
    [IsDeleted]             BIT           CONSTRAINT [DF_Customers_IsDeleted] DEFAULT ((0)) NOT NULL,
    [MSCustomerID]          INT           NULL,
    [ABWinnerType]          VARCHAR (10)  NULL,
    [SoftBounceThreshold]   INT           NULL,
    [DefaultBlastAsTest]    BIT           NULL,
    [TestBlastLimit]        INT           NULL,
    [PlatformClientID]      INT           NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([CustomerID] ASC) WITH (FILLFACTOR = 80)
);






GO
CREATE NONCLUSTERED INDEX [ix_Customers_BlastConfigID]
    ON [dbo].[Customer]([BlastConfigID] ASC)
    INCLUDE([CustomerID]) WITH (FILLFACTOR = 80);

