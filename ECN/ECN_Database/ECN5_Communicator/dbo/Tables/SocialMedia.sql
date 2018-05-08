CREATE TABLE [dbo].[SocialMedia] (
    [SocialMediaId]   INT           IDENTITY (1, 1) NOT NULL,
    [DisplayName]     VARCHAR (50)  NULL,
    [IsActive]        BIT           NULL,
    [MatchString]     VARCHAR (20)  NULL,
    [ImagePath]       VARCHAR (255) NULL,
    [ShareLink]       VARCHAR (255) NULL,
    [CanShare]        BIT           NULL,
    [CanPublish]      BIT           NULL,
    [DateAdded]       DATETIME      NULL,
    [ReportImagePath] VARCHAR (255) NULL
);

