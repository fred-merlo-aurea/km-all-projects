CREATE TABLE [dbo].[BlastConfig] (
    [BlastConfigID] INT           IDENTITY (1, 1) NOT NULL,
    [MTAName]       VARCHAR (25)  NULL,
    [SMTPServer]    NVARCHAR (25) NULL,
    [MTAServer]     NVARCHAR (25) NULL,
    [MTAReset]      NVARCHAR (25) NULL,
    [BounceName]    NVARCHAR (25) NULL,
    CONSTRAINT [PK_BlastConfig] PRIMARY KEY CLUSTERED ([BlastConfigID] ASC) WITH (FILLFACTOR = 80)
);

