CREATE TABLE [dbo].[jobBrandProducts] (
    [BrandProductID] INT          IDENTITY (1, 1) NOT NULL,
    [BrandCode]      VARCHAR (50) NOT NULL,
    [Pubcode]        VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_jobBrandProducts] PRIMARY KEY CLUSTERED ([BrandProductID] ASC) WITH (FILLFACTOR = 90)
);

