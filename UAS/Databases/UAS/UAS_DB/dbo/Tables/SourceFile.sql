CREATE TABLE [dbo].[SourceFile] (
    [SourceFileID]     INT           IDENTITY (1, 1) NOT NULL,
	[FileRecurrenceTypeId] INT DEFAULT((0)) NOT NULL,
	[DatabaseFileTypeId] INT DEFAULT((0)) NOT NULL,
    [FileName]         VARCHAR (250) NOT NULL,
    [ClientID]         INT           NOT NULL,
	[PublicationID]    INT           NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_SourceFile_IsDeleted] DEFAULT ((0)) NOT NULL,
    [IsIgnored]        BIT           CONSTRAINT [DF_SourceFile_IsIgnored] DEFAULT ((0)) NOT NULL,
    [FileSnippetID]    INT           NOT NULL,
    [Extension]        VARCHAR (10)  NULL,
    [IsDQMReady]       BIT           NULL,
    [Delimiter]        VARCHAR (10)  NULL,
    [IsTextQualifier]  BIT           NULL,
    [ServiceID]        INT           CONSTRAINT [DF_SourceFile_ServiceID] DEFAULT ((0)) NOT NULL,
    [ServiceFeatureID] INT           CONSTRAINT [DF_SourceFile_ServiceFeatureID] DEFAULT ((0)) NOT NULL,
    [MasterGroupID]    INT           CONSTRAINT [DF_SourceFile_MasterGroupID] DEFAULT ((0)) NOT NULL,
	[UseRealTimeGeocoding] BIT		 NOT NULL DEFAULT((0)),
	[IsSpecialFile]    BIT           CONSTRAINT [DF_SourceFile_IsSpecialFile] DEFAULT ((0)) NOT NULL,
	[ClientCustomProcedureID] INT NULL,
	[SpecialFileResultID] INT NULL,
	[DateCreated]      DATETIME      NOT NULL,
    [DateUpdated]      DATETIME      NULL,
    [CreatedByUserID]  INT           NOT NULL,
    [UpdatedByUserID]  INT           NULL,
	[QDateFormat] VARCHAR(20) NOT NULL DEFAULT('MMDDYYYY'), 
	[BatchSize] INT NOT NULL DEFAULT((2500)), 
    [IsPasswordProtected] BIT NULL DEFAULT ((0)), 
    [ProtectionPassword] VARCHAR(50) NULL,  
	[TotalSteps] int default((12)) not null,
	[NotifyEmailList] VARCHAR(1000) NULL,  
	IsBillable bit default('true') not null,
	Notes varchar(max) null,
	IsFullFile bit default('false') not null,
    CONSTRAINT [PK_SourceFile] PRIMARY KEY CLUSTERED ([SourceFileID] ASC) WITH (FILLFACTOR = 90)
);





