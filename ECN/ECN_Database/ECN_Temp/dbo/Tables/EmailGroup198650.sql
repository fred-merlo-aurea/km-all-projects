CREATE TABLE [dbo].[EmailGroup198650] (
    [EmailGroupID]      INT          IDENTITY (1, 1) NOT NULL,
    [EmailID]           INT          NOT NULL,
    [GroupID]           INT          NOT NULL,
    [FormatTypeCode]    VARCHAR (5)  NOT NULL,
    [SubscribeTypeCode] VARCHAR (50) NULL,
    [CreatedOn]         DATETIME     NULL,
    [LastChanged]       DATETIME     NULL,
    [SMSEnabled]        BIT          NULL
);

