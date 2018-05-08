CREATE TABLE [dbo].[Pricing] (
    [PriceID]     INT              IDENTITY (1, 1) NOT NULL,
    [BrandID]     INT              NOT NULL,
    [RangeStart]  INT              NOT NULL,
    [RangeEnd]    INT              NOT NULL,
    [Price]       DECIMAL (10, 2)  NOT NULL,
    [BGColor]     VARCHAR (10)     NULL,
    [FGColor]     VARCHAR (10)     NULL,
    [UpdatedBy]   UNIQUEIDENTIFIER NOT NULL,
    [UpdatedDate] DATETIME         CONSTRAINT [DF_Pricing_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Pricing] PRIMARY KEY CLUSTERED ([PriceID] ASC),
    CONSTRAINT [FK_Pricing_Brand] FOREIGN KEY ([BrandID]) REFERENCES [dbo].[Brand] ([BrandID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Yellow', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pricing', @level2type = N'COLUMN', @level2name = N'FGColor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Red', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pricing', @level2type = N'COLUMN', @level2name = N'BGColor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'$100.00', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pricing', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'2 years', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pricing', @level2type = N'COLUMN', @level2name = N'RangeEnd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 year', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pricing', @level2type = N'COLUMN', @level2name = N'RangeStart';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Medical Supplies (ID)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pricing', @level2type = N'COLUMN', @level2name = N'BrandID';

