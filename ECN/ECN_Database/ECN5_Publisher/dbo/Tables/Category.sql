CREATE TABLE [dbo].[Category] (
    [CategoryID]   INT          IDENTITY (1, 1) NOT NULL,
    [CategoryName] VARCHAR (50) NOT NULL,
    [IsDeleted]    BIT          CONSTRAINT [DF_Category_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_PublicationCategory] PRIMARY KEY CLUSTERED ([CategoryID] ASC) WITH (FILLFACTOR = 80)
);

