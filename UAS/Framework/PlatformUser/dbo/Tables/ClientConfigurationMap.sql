CREATE TABLE [dbo].[ClientConfigurationMap] (
    [ClientConfigurationMapId] INT           IDENTITY (1, 1) NOT NULL,
    [ClientID]                 INT           NOT NULL,
    [CodeTypeId]               INT           NOT NULL,
    [CodeId]                   INT           NOT NULL,
    [ClientValue]              VARCHAR (500) NOT NULL,
    [ClientValueMapID]         BIGINT        NULL,
    [IsActive]                 BIT           NOT NULL,
    [DateCreated]              DATETIME      NOT NULL,
    [DateUpdated]              DATETIME      NULL,
    [CreatedByUserID]          INT           NOT NULL,
    [UpdatedByUserID]          INT           NULL,
    CONSTRAINT [PK_ClientConfigurationMap] PRIMARY KEY CLUSTERED ([ClientConfigurationMapId] ASC),
    CONSTRAINT [FK_ClientConfigurationMap_Client] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID]),
    CONSTRAINT [FK_ClientConfigurationMap_ClientConfigurationValueMap] FOREIGN KEY ([ClientValueMapID]) REFERENCES [dbo].[ClientConfigurationValueMap] ([ClientConfigurationValueMapID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Value is the PKey of ConfigurationValueMap if the client has a complex value associated with the configuration codeType for this mapping.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClientConfigurationMap', @level2type = N'COLUMN', @level2name = N'ClientValueMapID';

