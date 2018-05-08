CREATE TABLE [dbo].[EmailClients] (
    [EmailClientID]   INT          NOT NULL,
    [EmailClientName] VARCHAR (30) NOT NULL,
    CONSTRAINT [PK_BlastActivityEmailClientLookUp] PRIMARY KEY CLUSTERED ([EmailClientID] ASC) WITH (FILLFACTOR = 80)
);

