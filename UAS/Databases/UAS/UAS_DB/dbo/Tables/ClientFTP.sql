CREATE TABLE [dbo].[ClientFTP] (
    [FTPID]                  INT           IDENTITY (1, 1) NOT NULL,
    [ClientID]               INT           NOT NULL,
    [Server]                 VARCHAR (100) NOT NULL,
    [UserName]               VARCHAR (100) NOT NULL,
    [Password]               VARCHAR (100) NOT NULL,
    [Folder]                 VARCHAR (100) NOT NULL,
    [IsDeleted]              BIT           CONSTRAINT [DF_ClientFTP_IsDeleted] DEFAULT ((0)) NOT NULL,
    [IsExternal]             BIT           NOT NULL,
    [IsActive]               BIT           NOT NULL,
    [FTPConnectionValidated] BIT           NOT NULL,
    [DateCreated]            DATETIME      NOT NULL,
    [DateUpdated]            DATETIME      NULL,
    [CreatedByUserID]        INT           NOT NULL,
    [UpdatedByUserID]        INT           NULL,
    CONSTRAINT [PK_ClientFTP] PRIMARY KEY CLUSTERED ([FTPID] ASC)
);



