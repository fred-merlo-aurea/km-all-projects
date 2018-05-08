CREATE TABLE [dbo].[Link] (
    [LinkID]        INT           IDENTITY (1, 1) NOT NULL,
    [PageID]        INT           NOT NULL,
    [LinkType]      VARCHAR (50)  NOT NULL,
    [LinkURL]       VARCHAR (500) NOT NULL,
    [x1]            INT           NOT NULL,
    [y1]            INT           NOT NULL,
    [x2]            INT           NOT NULL,
    [y2]            INT           NOT NULL,
    [Alias]         VARCHAR (100) NULL,
    [CreatedUserID] INT           NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_Link_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedUserID] INT           NULL,
    [UpdatedDate]   DATETIME      NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_Link_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Links] PRIMARY KEY CLUSTERED ([LinkID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Links_Pages] FOREIGN KEY ([PageID]) REFERENCES [dbo].[Page] ([PageID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Links]
    ON [dbo].[Link]([PageID] ASC) WITH (FILLFACTOR = 80);

