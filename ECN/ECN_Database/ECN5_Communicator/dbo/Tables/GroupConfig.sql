CREATE TABLE [dbo].[GroupConfig] (
    [GroupConfigID] INT           IDENTITY (1, 1) NOT NULL,
    [ShortName]     VARCHAR (200) NULL,
    [CustomerID]    INT           NULL,
    [CreatedUserID] INT           NULL,
    [CreatedDate]   DATETIME      NULL,
    [UpdatedDate]   DATETIME      NULL,
    [UpdatedUserID] INT           NULL,
    [IsDeleted]     BIT           NULL,
    [IsPublic]      CHAR (1)      NULL,
    CONSTRAINT [PK_GroupConfig] PRIMARY KEY CLUSTERED ([GroupConfigID] ASC) WITH (FILLFACTOR = 80)
);

