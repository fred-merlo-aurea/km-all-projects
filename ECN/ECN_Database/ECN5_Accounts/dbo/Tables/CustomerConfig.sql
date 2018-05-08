CREATE TABLE [dbo].[CustomerConfig] (
    [CustomerConfigID] INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT           NULL,
    [ProductID]        INT           NULL,
    [ConfigName]       VARCHAR (50)  NULL,
    [ConfigValue]      VARCHAR (255) NULL,
    [CreatedUserID]    INT           NULL,
    [CreatedDate]      DATETIME      CONSTRAINT [DF_CustomerConfig_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedUserID]    INT           NULL,
    [UpdatedDate]      DATETIME      NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_CustomerConfig_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CustomerConfig] PRIMARY KEY CLUSTERED ([CustomerConfigID] ASC) WITH (FILLFACTOR = 80)
);

