CREATE TABLE [dbo].[DatafieldSets] (
    [DatafieldSetID] INT          IDENTITY (1, 1) NOT NULL,
    [GroupID]        INT          NOT NULL,
    [MultivaluedYN]  CHAR (1)     NOT NULL,
    [Name]           VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DatafieldSets] PRIMARY KEY NONCLUSTERED ([DatafieldSetID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE CLUSTERED INDEX [IX_DatafieldSets_GroupID]
    ON [dbo].[DatafieldSets]([GroupID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[DatafieldSets] TO [reader]
    AS [dbo];

