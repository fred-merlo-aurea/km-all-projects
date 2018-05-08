CREATE TABLE [dbo].[Code] (
    [CodeID]        INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]    INT           NULL,
    [CodeType]      VARCHAR (50)  CONSTRAINT [DF_Codes_CodeType] DEFAULT ('') NULL,
    [CodeValue]     VARCHAR (50)  CONSTRAINT [DF_Codes_CodeValue] DEFAULT ('') NULL,
    [CodeDisplay]   VARCHAR (250) CONSTRAINT [DF_Codes_CodeDisplay] DEFAULT ('') NULL,
    [SortOrder]     INT           CONSTRAINT [DF_Codes_SortOrder] DEFAULT (0) NULL,
    [DisplayFlag]   CHAR (1)      CONSTRAINT [DF_Codes_DisplayFlag] DEFAULT ('Y') NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_Codes_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT           NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_Codes_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME      NULL,
    [UpdatedUserID] INT           NULL,
    CONSTRAINT [PK_Codes] PRIMARY KEY CLUSTERED ([CodeID] ASC) WITH (FILLFACTOR = 80)
);

