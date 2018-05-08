CREATE TABLE [dbo].[LinkTrackingParamOption] (
    [LTPOID]        INT          IDENTITY (1, 1) NOT NULL,
    [DisplayName]   VARCHAR (50) NULL,
    [ColumnName]    VARCHAR (50) NULL,
    [IsActive]      BIT          NULL,
    [BaseChannelID] INT          NULL,
    [CreatedDate]   DATETIME     NULL,
    [CreatedUserID] INT          NULL,
    [CustomerID]    INT          NULL,
    [IsDefault]     BIT          NULL,
    [IsDeleted]     BIT          NULL,
    [IsDynamic]     BIT          NULL,
    [LTPID]         INT          NULL,
    [UpdatedDate]   DATETIME     NULL,
    [UpdatedUserID] INT          NULL,
    [Value]         VARCHAR (50) NULL,
    CONSTRAINT [PK_LinkTrackingParamOption] PRIMARY KEY CLUSTERED ([LTPOID] ASC) WITH (FILLFACTOR = 80)
);

