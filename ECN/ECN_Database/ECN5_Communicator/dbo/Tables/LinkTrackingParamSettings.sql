CREATE TABLE [dbo].[LinkTrackingParamSettings] (
    [LTPSID]        INT          IDENTITY (1, 1) NOT NULL,
    [LTPID]         INT          NOT NULL,
    [CustomerID]    INT          NULL,
    [BaseChannelID] INT          NULL,
    [DisplayName]   VARCHAR (50) NOT NULL,
    [AllowCustom]   BIT          NOT NULL,
    [IsRequired]    BIT          NOT NULL,
    [CreatedUserID] INT          NULL,
    [CreatedDate]   DATETIME     NULL,
    [UpdatedUserID] INT          NULL,
    [UpdatedDate]   DATETIME     NULL,
    [IsDeleted]     BIT          NOT NULL,
    CONSTRAINT [PK_LinkTrackingParamSettings] PRIMARY KEY CLUSTERED ([LTPSID] ASC)
);

