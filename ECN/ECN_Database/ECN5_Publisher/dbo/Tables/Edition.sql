CREATE TABLE [dbo].[Edition] (
    [EditionID]         INT           IDENTITY (1, 1) NOT NULL,
    [EditionName]       VARCHAR (100) NOT NULL,
    [PublicationID]     INT           NOT NULL,
    [EnableDate]        DATETIME      NULL,
    [DisableDate]       DATETIME      NULL,
    [Theme]             VARCHAR (50)  NULL,
    [Background]        VARCHAR (500) NULL,
    [FileName]          VARCHAR (100) NOT NULL,
    [Pages]             INT           NOT NULL,
    [ThumbnailPage]     VARCHAR (255) NULL,
    [Status]            VARCHAR (10)  NULL,
    [xmlTOC]            TEXT          NULL,
    [IsSearchable]      BIT           CONSTRAINT [DF_Editions_IsSearchable] DEFAULT ((0)) NULL,
    [CreatedDate]       DATETIME      CONSTRAINT [DF_Editions_CreateDate] DEFAULT (getdate()) NULL,
    [UpdatedDate]       DATETIME      CONSTRAINT [DF_Editions_UpdateDate] DEFAULT (getdate()) NULL,
    [IsLoginRequired]   BIT           CONSTRAINT [DF_Editions_IsLoginRequired] DEFAULT ((0)) NULL,
    [OriginalEditionID] INT           NULL,
    [Version]           INT           NULL,
    [IsDeleted]         BIT           CONSTRAINT [DF_Editions_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedUserID]     INT           NULL,
    [UpdatedUserID]     INT           NULL,
    CONSTRAINT [PK_Editions] PRIMARY KEY CLUSTERED ([EditionID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Editions_Magazines] FOREIGN KEY ([PublicationID]) REFERENCES [dbo].[Publication] ([PublicationID])
);


GO
CREATE NONCLUSTERED INDEX [ix_Editions_PublicationID]
    ON [dbo].[Edition]([PublicationID] ASC) WITH (FILLFACTOR = 80);

