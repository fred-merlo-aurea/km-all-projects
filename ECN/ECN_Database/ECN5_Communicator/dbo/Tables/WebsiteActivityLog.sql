CREATE TABLE [dbo].[WebsiteActivityLog] (
    [LogID]        INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]   INT           NULL,
    [UserID]       INT           NULL,
    [WebPageURL]   VARCHAR (500) NULL,
    [DateAccessed] DATETIME      CONSTRAINT [DF_LogWebsiteActivity_DateAccessed] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_LogWebsiteActivity] PRIMARY KEY CLUSTERED ([LogID] ASC) WITH (FILLFACTOR = 80)
);

