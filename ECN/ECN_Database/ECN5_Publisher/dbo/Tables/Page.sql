CREATE TABLE [dbo].[Page] (
    [PageID]        INT         IDENTITY (1, 1) NOT NULL,
    [EditionID]     INT         NOT NULL,
    [PageNumber]    INT         NOT NULL,
    [DisplayNumber] VARCHAR (5) NOT NULL,
    [Width]         INT         NULL,
    [Height]        INT         NULL,
    [TextContent]   TEXT        NULL,
    [CreatedUserID] INT         NULL,
    [CreatedDate]   DATETIME    CONSTRAINT [DF_Page_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedUserID] INT         NULL,
    [UpdatedDate]   DATETIME    NULL,
    [IsDeleted]     BIT         CONSTRAINT [DF_Page_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Pages] PRIMARY KEY CLUSTERED ([PageID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Pages_Editions] FOREIGN KEY ([EditionID]) REFERENCES [dbo].[Edition] ([EditionID])
);

