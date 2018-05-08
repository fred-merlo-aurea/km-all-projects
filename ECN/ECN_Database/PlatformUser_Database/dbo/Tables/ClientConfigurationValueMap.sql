CREATE TABLE [dbo].[ClientConfigurationValueMap] (
    [ClientConfigurationValueMapID]   BIGINT   IDENTITY (1, 1) NOT NULL,
    [ValueCodeID]                     INT      NOT NULL,
    [ValueBit]                        BIT      NULL,
    [ValueInt]                        INT      NULL,
    [ClientConfigurationValueCDataID] INT      NULL,
    [DateCreated]                     DATETIME CONSTRAINT [DF_ClientConfigurationValuesMap_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdate]                      DATETIME NULL,
    [CreatedByUserID]                 INT      NOT NULL,
    [UpdateByUserID]                  INT      NULL,
    PRIMARY KEY CLUSTERED ([ClientConfigurationValueMapID] ASC),
    CONSTRAINT [FK_ClientConfigurationValueMap_ClientConfigurationValueCData] FOREIGN KEY ([ClientConfigurationValueCDataID]) REFERENCES [dbo].[ClientConfigurationValueCData] ([ClientConfigurationValueCDataID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID from Code Type for the Value Type where corrisponding name might be bit, int, string, a mime/type such as "application/json", etc.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClientConfigurationValueMap', @level2type = N'COLUMN', @level2name = N'ValueCodeID';

