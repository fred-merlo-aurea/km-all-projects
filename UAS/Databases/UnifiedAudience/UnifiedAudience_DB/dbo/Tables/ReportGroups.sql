CREATE TABLE [dbo].[ReportGroups] (
    [ReportGroupID]   INT          IDENTITY (1, 1) NOT NULL,
    [ResponseGroupID] INT          NULL,
    [DisplayName]     VARCHAR (50) NULL,
    [DisplayOrder]    INT          NULL,
    CONSTRAINT [PK_ReportGroups_ReportGroupID] PRIMARY KEY CLUSTERED ([ReportGroupID] ASC) WITH (FILLFACTOR = 90)
);