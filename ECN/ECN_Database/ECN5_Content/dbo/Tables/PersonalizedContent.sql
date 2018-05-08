CREATE TABLE [dbo].[PersonalizedContent] (
    [PersonalizedContentID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [BlastID]               BIGINT        NOT NULL,
    [EmailAddress]          VARCHAR (255) NOT NULL,
    [EmailSubject]          VARCHAR (255) NULL,
    [HTMLContent]           VARCHAR (MAX) NOT NULL,
    [TEXTContent]           VARCHAR (MAX) NOT NULL,
    [IsValid]               BIT           CONSTRAINT [DF_PersonalizedContent_IsValid] DEFAULT ((1)) NOT NULL,
    [IsProcessed]           BIT           CONSTRAINT [DF_PersonalizedContent_IsProcessed] DEFAULT ((0)) NOT NULL,
    [IsDeleted]             BIT           CONSTRAINT [DF_PersonalizedContent_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedDate]           DATETIME      NOT NULL,
    [CreatedUserID]         INT           NOT NULL,
    [UpdatedDate]           DATETIME      NULL,
    [UpdatedUserID]         INT           NULL,
    [Failed] BIT NULL, 
    CONSTRAINT [PK_PersonalizedContent] PRIMARY KEY CLUSTERED ([PersonalizedContentID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_PersonalizedContent_BlastID_EmailAddress]
    ON [dbo].[PersonalizedContent]([BlastID] ASC, [EmailAddress] ASC);

