CREATE TABLE [dbo].[SocialActivityCodes] (
    [SocialActivityCodeID] INT          IDENTITY (1, 1) NOT NULL,
    [SocialActivityCode]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SocialActivityCodes] PRIMARY KEY CLUSTERED ([SocialActivityCodeID] ASC) WITH (FILLFACTOR = 80)
);

