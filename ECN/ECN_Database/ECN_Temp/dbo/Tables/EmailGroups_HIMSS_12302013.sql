CREATE TABLE [dbo].[EmailGroups_HIMSS_12302013] (
    [EmailGroupID]      INT          NOT NULL,
    [EmailID]           INT          NOT NULL,
    [GroupID]           INT          NOT NULL,
    [FormatTypeCode]    VARCHAR (5)  NOT NULL,
    [SubscribeTypeCode] VARCHAR (50) NULL,
    [CreatedOn]         DATETIME     NULL,
    [LastChanged]       DATETIME     NULL,
    [SMSEnabled]        BIT          NULL
);

