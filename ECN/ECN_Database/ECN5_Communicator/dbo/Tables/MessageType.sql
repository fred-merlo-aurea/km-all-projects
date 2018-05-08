CREATE TABLE [dbo].[MessageType] (
    [MessageTypeID]  INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50)  NOT NULL,
    [Description]    VARCHAR (255) NULL,
    [Threshold]      BIT           NOT NULL,
    [Priority]       BIT           NOT NULL,
    [PriorityNumber] INT           NULL,
    [BaseChannelID]  INT           NOT NULL,
    [SortOrder]      INT           NULL,
    [IsActive]       BIT           CONSTRAINT [DF_MessageTypes_IsActive] DEFAULT ((1)) NOT NULL,
    [CustomerID]     INT           NULL,
    [CreatedDate]    DATETIME      CONSTRAINT [DF_MessageType_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID]  INT           NULL,
    [IsDeleted]      BIT           CONSTRAINT [DF_MessageType_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]    DATETIME      NULL,
    [UpdatedUserID]  INT           NULL,
    CONSTRAINT [PK_MessageTypes] PRIMARY KEY CLUSTERED ([MessageTypeID] ASC) WITH (FILLFACTOR = 80)
);

