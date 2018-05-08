CREATE TABLE [dbo].[SecurityGroupOptIn] (
    [SecurityGroupOptInID] INT              IDENTITY (1, 1) NOT NULL,
    [UserID]               INT              NOT NULL,
    [SecurityGroupID]      INT              NOT NULL,
    [ClientID]             INT              NOT NULL,
    [ClientGroupID]        INT              NULL,
    [SendTime]             DATETIME         NOT NULL,
    [HasAccepted]          BIT              NOT NULL,
    [DateAccepted]         DATETIME         NULL,
    [SetID]                UNIQUEIDENTIFIER NOT NULL,
    [CreatedByUserID]      INT              NOT NULL,
    [CreatedDate]          DATETIME         NOT NULL,
    [IsDeleted]            BIT              NULL,
    CONSTRAINT [PK_SecurityGroupOptIn] PRIMARY KEY CLUSTERED ([SecurityGroupOptInID] ASC)
);

