CREATE TABLE [dbo].[NBC_Wizard] (
    [WizardID]         INT           IDENTITY (1, 1) NOT NULL,
    [MessageName]      VARCHAR (255) NULL,
    [MessageDesc]      VARCHAR (500) NULL,
    [CustomerID]       INT           NULL,
    [CustomerIDs]      VARCHAR (500) NULL,
    [Filter]           VARCHAR (100) NULL,
    [Subject]          VARCHAR (500) NULL,
    [FromEmailAddress] VARCHAR (100) NULL,
    [FromEmailName]    VARCHAR (100) NULL,
    [TemplateID]       INT           NULL,
    [ContentText]      TEXT          NULL,
    [UserID]           INT           NULL,
    [Status]           VARCHAR (50)  NULL,
    [DateCreated]      DATETIME      NULL,
    [DateUpdated]      DATETIME      NULL,
    CONSTRAINT [PK_NBC_Wizard] PRIMARY KEY CLUSTERED ([WizardID] ASC) WITH (FILLFACTOR = 80)
);

