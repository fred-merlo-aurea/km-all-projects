CREATE TABLE [dbo].[WizardBlastFields] (
    [WizardID] INT           NOT NULL,
    [Field3]   VARCHAR (255) NULL,
    [Field4]   VARCHAR (255) NULL,
    [Field5]   VARCHAR (255) NULL,
    [Field1]   VARCHAR (255) NULL,
    [Field2]   VARCHAR (255) NULL,
    CONSTRAINT [PK_WizardBlastFields] PRIMARY KEY CLUSTERED ([WizardID] ASC) WITH (FILLFACTOR = 80)
);

