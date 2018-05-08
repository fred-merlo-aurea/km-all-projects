CREATE TABLE [dbo].[SearchType] (
    [SearchTypeID]   INT          IDENTITY (1, 1) NOT NULL,
    [SearchTypeName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SearchType] PRIMARY KEY CLUSTERED ([SearchTypeID] ASC)
);

