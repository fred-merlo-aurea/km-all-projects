CREATE TABLE [dbo].[Users] (
    [UserID]              INT           NOT NULL,
    [Permission]          VARCHAR (50)  NULL,
    [ExportPermissionIDs] VARCHAR (200) NULL,
    [ShowSalesView]       BIT           CONSTRAINT [DF_Users_ShowSalesMenu] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC) WITH (FILLFACTOR = 90)
);



