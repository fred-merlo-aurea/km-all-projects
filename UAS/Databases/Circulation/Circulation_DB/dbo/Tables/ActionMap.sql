CREATE TABLE [dbo].[ActionMap] (
    [FromActionID] INT NOT NULL,
    [ToActionID]   INT NOT NULL,
    CONSTRAINT [PK_ActionMap] PRIMARY KEY CLUSTERED ([FromActionID] ASC, [ToActionID] ASC) WITH (FILLFACTOR = 80)
);

