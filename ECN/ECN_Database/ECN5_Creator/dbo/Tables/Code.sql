CREATE TABLE [dbo].[Code] (
    [CodeID]      INT           IDENTITY (1, 1) NOT NULL,
    [CodeType]    VARCHAR (50)  NULL,
    [CodeValue]   VARCHAR (50)  NULL,
    [CodeDisplay] VARCHAR (255) NULL,
    [SortCode]    INT           NULL,
    [SystemFlag]  VARCHAR (1)   CONSTRAINT [DF_Code_SystemFlag] DEFAULT ('N') NULL,
    CONSTRAINT [PK_Codes] PRIMARY KEY CLUSTERED ([CodeID] ASC) WITH (FILLFACTOR = 80)
);

