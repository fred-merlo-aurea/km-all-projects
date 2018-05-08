CREATE TABLE [dbo].[WizardBlasts] (
    [WizardID]        INT            NOT NULL,
    [BlastID]         VARCHAR (2000) NOT NULL,
    [GroupID]         VARCHAR (2000) NOT NULL,
    [BlastType]       VARCHAR (50)   NOT NULL,
    [LayoutID]        INT            NOT NULL,
    [SuppressionList] VARCHAR (2000) NULL,
    CONSTRAINT [PK_WizardBlastGroups] PRIMARY KEY CLUSTERED ([WizardID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_WizardBlastGroups_Wizard] FOREIGN KEY ([WizardID]) REFERENCES [dbo].[Wizard] ([WizardID])
);

