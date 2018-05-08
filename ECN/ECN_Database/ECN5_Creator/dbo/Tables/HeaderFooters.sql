CREATE TABLE [dbo].[HeaderFooters] (
    [HeaderFooterID]   INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT           NULL,
    [HeaderFooterName] VARCHAR (50)  NULL,
    [HeaderCode]       TEXT          NULL,
    [FooterCode]       TEXT          NULL,
    [Keywords]         VARCHAR (255) CONSTRAINT [DF_HeaderFooters_Keywords] DEFAULT ('') NULL,
    [JavaScriptCode]   TEXT          CONSTRAINT [DF_HeaderFooters_JavaScriptCode] DEFAULT ('') NULL,
    [StyleSheet]       TEXT          CONSTRAINT [DF_HeaderFooters_StyleSheet] DEFAULT ('') NULL,
    CONSTRAINT [PK_HeaderFooter] PRIMARY KEY CLUSTERED ([HeaderFooterID] ASC) WITH (FILLFACTOR = 80)
);

