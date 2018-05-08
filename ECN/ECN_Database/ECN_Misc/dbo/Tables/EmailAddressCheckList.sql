CREATE TABLE [dbo].[EmailAddressCheckList] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [EmailAddress] VARCHAR (250) NULL,
    [IsProcessed]  VARCHAR (1)   CONSTRAINT [DF_EmailAddressCheckList_IsProcessed] DEFAULT ('N') NULL,
    [IsValid]      VARCHAR (1)   CONSTRAINT [DF_EmailAddressCheckList_IsValid] DEFAULT ('N') NULL,
    [Response]     TEXT          NULL,
    [ZipCode]      VARCHAR (5)   NULL,
    [CreationDate] DATETIME      NULL,
    [Source]       VARCHAR (150) NULL,
    [UpdatedDate]  DATETIME      NULL,
    [Name]         VARCHAR (50)  NULL,
    CONSTRAINT [PK_EmailAddressCheckList] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 80)
);

