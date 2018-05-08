CREATE TABLE [dbo].[MailingCompany] (
    [MailingCompanyID]   INT          NOT NULL,
    [ClientID]           INT          NULL,
    [Name]               VARCHAR (50) NULL,
    [FTPSite]            VARCHAR (70) NULL,
    [UserName]           VARCHAR (50) NULL,
    [Password]           VARCHAR (50) NULL,
    [DownloadFileFolder] VARCHAR (50) NULL,
    [UploadFileFolder]   VARCHAR (50) NULL,
    [LogFile]            VARCHAR (10) NULL
);

