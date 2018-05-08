CREATE TABLE [dbo].[DKIMKeys] (
    [domainname] VARCHAR (900)  NOT NULL,
    [selector]   VARCHAR (255)  NOT NULL,
    [dkimkey]    VARCHAR (4000) NOT NULL,
    CONSTRAINT [PK_DKIMKeys] PRIMARY KEY CLUSTERED ([domainname] ASC) WITH (FILLFACTOR = 80)
);

