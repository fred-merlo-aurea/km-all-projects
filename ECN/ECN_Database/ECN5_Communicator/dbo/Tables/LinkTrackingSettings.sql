CREATE TABLE [dbo].[LinkTrackingSettings] (
    [LTSID]         INT            IDENTITY (1, 1) NOT NULL,
    [LTID]          INT            NOT NULL,
    [CustomerID]    INT            NULL,
    [BaseChannelID] INT            NULL,
    [XMLConfig]     VARCHAR (1000) NULL,
    [CreatedUserID] INT            NULL,
    [CreatedDate]   DATETIME       NULL,
    [UpdatedUserID] INT            NULL,
    [UpdatedDate]   DATETIME       NULL,
    [IsDeleted]     BIT            NOT NULL,
    CONSTRAINT [PK_LinkTrackingSettings] PRIMARY KEY CLUSTERED ([LTSID] ASC)
);

