CREATE TABLE [dbo].[ServiceFeature] (
    [ServiceFeatureID]        INT             IDENTITY (1, 1) NOT NULL,
    [ServiceID]               INT             NOT NULL,
    [SFName]                  VARCHAR (100)   NOT NULL,
    [Description]             VARCHAR (500)   NULL,
    [SFCode]                  VARCHAR (50)    NULL,
    [DisplayOrder]            INT             NOT NULL,
    [IsEnabled]               BIT             NOT NULL,
    [IsAdditionalCost]        BIT             NOT NULL,
    [DefaultRate]             DECIMAL (14, 2) NOT NULL,
    [DefaultDurationInMonths] INT             NOT NULL,
    [KMAdminOnly]             BIT             NOT NULL,
    [DateCreated]             DATETIME        NOT NULL,
    [DateUpdated]             DATETIME        NULL,
    [CreatedByUserID]         INT             NOT NULL,
    [UpdatedByUserID]         INT             NULL,
    CONSTRAINT [PK_ServiceFeature] PRIMARY KEY CLUSTERED ([ServiceFeatureID] ASC),
    CONSTRAINT [FK_ServiceFeature_ServiceID] FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Service] ([ServiceID])
);


GO
CREATE NONCLUSTERED INDEX [IDX_ServiceFeature_ServiceID]
    ON [dbo].[ServiceFeature]([ServiceID] ASC) WITH (FILLFACTOR = 90);

