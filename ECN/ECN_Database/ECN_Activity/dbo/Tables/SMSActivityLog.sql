CREATE TABLE [dbo].[SMSActivityLog] (
    [SAID]         INT          IDENTITY (1, 1) NOT NULL,
    [EmailID]      INT          NULL,
    [BlastID]      INT          NULL,
    [SendStatus]   VARCHAR (50) NULL,
    [SendTime]     DATETIME     NULL,
    [Notes]        VARCHAR (50) NULL,
    [IsWelcomeMsg] BIT          NULL,
    CONSTRAINT [PK_SMSActivityLog] PRIMARY KEY CLUSTERED ([SAID] ASC) WITH (FILLFACTOR = 80)
);

