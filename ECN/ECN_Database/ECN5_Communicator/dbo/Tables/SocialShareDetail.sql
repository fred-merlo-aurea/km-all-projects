CREATE TABLE [dbo].[SocialShareDetail] (
    [SocialShareDetailID] INT            IDENTITY (1, 1) NOT NULL,
    [ContentID]           INT            NULL,
    [Title]               VARCHAR (500)  NULL,
    [Description]         VARCHAR (1000) NULL,
    [Image]               VARCHAR (200)  NULL,
    [CreatedUserID]       INT            NULL,
    [CreatedDate]         DATETIME       NULL,
    [UpdatedUserID]       INT            NULL,
    [UpdatedDate]         DATETIME       NULL,
    [IsDeleted]           BIT            NULL,
    CONSTRAINT [PK_SocialShareDetail] PRIMARY KEY CLUSTERED ([SocialShareDetailID] ASC)
);

