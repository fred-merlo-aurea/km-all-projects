CREATE TABLE [dbo].[AcsMailerInfo] (
    [AcsMailerInfoId] INT         IDENTITY (1, 1) NOT NULL,
    [AcsCode]         VARCHAR (9) NOT NULL,
    [ImbSeqCounter]   INT         DEFAULT ((1)) NULL,
    [DateCreated]     DATETIME    NULL,
    [DateUpdated]     DATETIME    NULL,
    [CreatedByUserID] INT         NULL,
    [UpdatedByUserID] INT         NULL,
    [MailerID]        INT         NULL,
    CONSTRAINT [PK_AcsMailerInfo_AcsMailerInfoId] PRIMARY KEY CLUSTERED ([AcsMailerInfoId] ASC) WITH (FILLFACTOR = 90)
);

