﻿CREATE TABLE [dbo].[MTAIP] (
    [IPID]      INT           IDENTITY (1, 1) NOT NULL,
    [MTAID]     INT           NOT NULL,
    [IPAddress] VARCHAR (100) NOT NULL,
    [HostName]  VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_MTAIP] PRIMARY KEY CLUSTERED ([IPID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK_MTAIP_MTA] FOREIGN KEY ([MTAID]) REFERENCES [dbo].[MTA] ([MTAID]),
    CONSTRAINT [IX_MTAIPHostName] UNIQUE NONCLUSTERED ([HostName] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [IX_MTAIPIPAddress] UNIQUE NONCLUSTERED ([IPAddress] ASC) WITH (FILLFACTOR = 80)
);

