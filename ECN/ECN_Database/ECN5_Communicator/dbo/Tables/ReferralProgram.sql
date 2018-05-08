CREATE TABLE [dbo].[ReferralProgram] (
    [ReferralProgramID]           INT            IDENTITY (1, 1) NOT NULL,
    [GroupID]                     INT            NULL,
    [ReferralProgramName]         VARCHAR (500)  NULL,
    [SmartFormHTML]               TEXT           NULL,
    [SmartFormFields]             VARCHAR (1000) NULL,
    [SmartFormFieldset]           INT            NULL,
    [Referer_Response_FromEmail]  VARCHAR (250)  NULL,
    [Referer_Response_MsgSubject] VARCHAR (250)  NULL,
    [Referer_Response_MsgID]      INT            NULL,
    [Referer_Response_Screen]     VARCHAR (500)  NULL,
    [Referee_Lead_MsgSubject]     VARCHAR (250)  NULL,
    [Referee_Lead_MsgID]          INT            NULL,
    [DateCreated]                 DATETIME       NULL,
    [DateUpdated]                 DATETIME       NULL,
    CONSTRAINT [PK_ReferralProgram] PRIMARY KEY CLUSTERED ([ReferralProgramID] ASC) WITH (FILLFACTOR = 80)
);

