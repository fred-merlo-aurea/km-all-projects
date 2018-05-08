CREATE TABLE [dbo].[FilterPenetrationReports] (
    [ReportID]      INT          IDENTITY (1, 1) NOT NULL,
    [ReportName]    VARCHAR (50) NULL,
    [CreatedUserID] INT          NULL,
    [CreatedDate]   DATETIME     NULL,
    [UpdatedUserID] INT          NULL,
    [UpdatedDate]   DATETIME     NULL,
    [BrandID]       INT          NULL,
    [IsDeleted]     BIT          CONSTRAINT [DF_FilterPenetrationReports_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_FilterPenetrationReports] PRIMARY KEY CLUSTERED ([ReportID] ASC)
);

