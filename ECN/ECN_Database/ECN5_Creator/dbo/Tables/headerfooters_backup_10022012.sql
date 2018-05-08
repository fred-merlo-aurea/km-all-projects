CREATE TABLE [dbo].[headerfooters_backup_10022012] (
    [HeaderFooterID]   INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT           NULL,
    [HeaderFooterName] VARCHAR (50)  NULL,
    [HeaderCode]       TEXT          NULL,
    [FooterCode]       TEXT          NULL,
    [Keywords]         VARCHAR (255) NULL,
    [JavaScriptCode]   TEXT          NULL,
    [StyleSheet]       TEXT          NULL
);

