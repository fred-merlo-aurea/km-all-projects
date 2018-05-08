CREATE TABLE [dbo].[Log_sp_copyProfilesToGroup] (
    [CopyLogID]          INT           IDENTITY (1, 1) NOT NULL,
    [sourceGroupID]      INT           NULL,
    [DestinationGroupID] INT           NULL,
    [FullSync]           BIT           NULL,
    [Filter]             VARCHAR (MAX) NULL,
    [EmailInsert]        INT           NULL,
    [EmailUpdate]        INT           NULL,
    [EmailgroupInsert]   INT           NULL,
    [EDVInsert]          INT           NULL,
    [EDVUpdate]          INT           NULL,
    [Starttime]          DATETIME      NULL,
    [endtime]            DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([CopyLogID] ASC)
);

