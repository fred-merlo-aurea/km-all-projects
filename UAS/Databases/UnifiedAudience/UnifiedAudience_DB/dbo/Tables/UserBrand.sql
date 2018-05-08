CREATE TABLE [dbo].[UserBrand] (
    [UserID]  INT NOT NULL,
    [BrandID] INT NOT NULL,
    CONSTRAINT [PK_UserBrand] PRIMARY KEY CLUSTERED ([UserID] ASC, [BrandID] ASC),
    CONSTRAINT [FK_UserBrand_Brand] FOREIGN KEY ([BrandID]) REFERENCES [dbo].[Brand] ([BrandID])
);

