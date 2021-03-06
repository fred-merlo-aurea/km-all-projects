﻿CREATE TABLE [dbo].[Page] (
    [PageID]         INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]     INT           NULL,
    [HeaderFooterID] INT           NULL,
    [QueryValue]     VARCHAR (50)  NULL,
    [PageTypeCode]   VARCHAR (50)  NULL,
    [PageName]       VARCHAR (255) NULL,
    [SourceCode]     TEXT          CONSTRAINT [DF_Pages_SourceCode] DEFAULT ('') NULL,
    [AddDate]        DATETIME      NULL,
    [HomePageFlag]   VARCHAR (1)   CONSTRAINT [DF_Pages_HomePageFlag] DEFAULT ('N') NULL,
    [DisplayFlag]    VARCHAR (1)   NULL,
    [PageProperties] VARCHAR (500) CONSTRAINT [DF_Pages_PageProperties] DEFAULT ('') NULL,
    [TemplateID]     INT           NULL,
    [ContentSlot1]   INT           NULL,
    [ContentSlot2]   INT           NULL,
    [ContentSlot3]   INT           NULL,
    [ContentSlot4]   INT           NULL,
    [ContentSlot5]   INT           NULL,
    [ContentSlot6]   INT           NULL,
    [ContentSlot7]   INT           NULL,
    [ContentSlot8]   INT           NULL,
    [ContentSlot9]   INT           NULL,
    [FolderID]       INT           NULL,
    [UserID]         INT           NULL,
    [UpdatedDate]    DATETIME      NULL,
    [PageSize]       INT           CONSTRAINT [DF_Pages_PageSize] DEFAULT (0) NULL,
    CONSTRAINT [PK_Pages] PRIMARY KEY CLUSTERED ([PageID] ASC) WITH (FILLFACTOR = 80)
);

