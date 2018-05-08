CREATE TABLE [dbo].[SocialActivityLog] (
    [SAID]           INT           IDENTITY (1, 1) NOT NULL,
    [RefEmailID]     INT           NOT NULL,
    [RefBlastID]     INT           NOT NULL,
    [ActionTypeCode] VARCHAR (50)  NOT NULL,
    [ActionDate]     DATETIME      NULL,
    [ActionValue]    VARCHAR (500) NULL,
    [ActionNotes]    VARCHAR (255) NULL,
    CONSTRAINT [PK_SocialActivityLog] PRIMARY KEY CLUSTERED ([SAID] ASC) WITH (FILLFACTOR = 80)
);

