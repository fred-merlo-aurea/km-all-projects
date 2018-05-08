CREATE TABLE [dbo].[ProductAudit] (
    [ProductAuditId]                 INT           IDENTITY (1, 1) NOT NULL,
    [ProductId]                      INT           NOT NULL,
    [AuditField]                     VARCHAR (255) NOT NULL,
    [FieldMappingTypeId]             INT           NOT NULL,
    [ResponseGroupID]                INT           NULL,
    [SubscriptionsExtensionMapperID] INT           NULL,
    [IsActive]                       BIT           NOT NULL,
    [DateCreated]                    DATETIME      CONSTRAINT [DF_ProductAudit_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]                    DATETIME      NULL,
    [CreatedByUserID]                INT           NOT NULL,
    [UpdatedByUserID]                INT           NULL,
    CONSTRAINT [PK_ProductAudit_ProductAuditID] PRIMARY KEY CLUSTERED ([ProductAuditId] ASC) WITH (FILLFACTOR = 90)
);