CREATE TABLE [dbo].[UDFImportSchedule] (
    [ImporterID]        INT            IDENTITY (1, 1) NOT NULL,
    [ChannelID]         INT            NULL,
    [CustomerID]        INT            NULL,
    [ImportGroupID]     INT            NULL,
    [AppendData]        CHAR (1)       CONSTRAINT [DF_DataImportSchedule_AppendData] DEFAULT ('N') NULL,
    [ImportName]        VARCHAR (100)  CONSTRAINT [DF_DataImportSchedule_ImportName] DEFAULT ('') NULL,
    [ImportType]        VARCHAR (50)   CONSTRAINT [DF_DataImportSchedule_ImportType] DEFAULT ('') NULL,
    [SiteAddress]       VARCHAR (100)  CONSTRAINT [DF_DataImportSchedule_SiteAddress] DEFAULT ('') NULL,
    [UserName]          VARCHAR (50)   CONSTRAINT [DF_DataImportSchedule_UserName] DEFAULT ('') NULL,
    [Password]          VARCHAR (50)   CONSTRAINT [DF_DataImportSchedule_Password] DEFAULT ('') NULL,
    [Directory_Query]   VARCHAR (8000) CONSTRAINT [DF_DataImportSchedule_Directory] DEFAULT ('') NULL,
    [Database_FileName] VARCHAR (50)   CONSTRAINT [DF_DataImportSchedule_Table_FileName] DEFAULT ('') NULL,
    [Table_Sheet]       VARCHAR (50)   CONSTRAINT [DF_UDFImportSchedule_Table_Sheet] DEFAULT ('Sheet1') NULL,
    [ImportFrequency]   VARCHAR (50)   CONSTRAINT [DF_UDFImportSchedule_ImportFrequency] DEFAULT ('') NULL,
    [ImportSetting]     VARCHAR (10)   CONSTRAINT [DF_UDFImportSchedule_ImportSetting] DEFAULT ('') NULL,
    [ImportDateTime]    DATETIME       NULL,
    [Active]            CHAR (1)       CONSTRAINT [DF_DataImportSchedule_Active] DEFAULT ('N') NULL,
    [DateAdded]         DATETIME       NULL,
    [DateUpdated]       DATETIME       NULL,
    [SecureConnectionData] VARCHAR(255) NULL, 
	[AdminEmail]		VARCHAR(255) NULL,
    CONSTRAINT [PK_DataImportSchedule] PRIMARY KEY CLUSTERED ([ImporterID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_UDFImportSchedule_Groups] FOREIGN KEY ([ImportGroupID]) REFERENCES [dbo].[Groups] ([GroupID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UDFImportSchedule] TO [reader]
    AS [dbo];

