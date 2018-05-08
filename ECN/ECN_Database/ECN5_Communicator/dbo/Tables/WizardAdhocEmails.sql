CREATE TABLE [dbo].[WizardAdhocEmails] (
    [AdhocEmailID] INT            IDENTITY (1, 1) NOT NULL,
    [WizardID]     INT            NULL,
    [GroupID]      INT            NULL,
    [EmailAddress] VARCHAR (4000) NULL,
    CONSTRAINT [PK_WizardAdhocEmails] PRIMARY KEY CLUSTERED ([AdhocEmailID] ASC) WITH (FILLFACTOR = 80)
);

