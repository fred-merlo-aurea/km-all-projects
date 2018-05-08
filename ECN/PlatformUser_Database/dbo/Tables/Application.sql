CREATE TABLE [dbo].[Application] (
    [ApplicationID]     INT           IDENTITY (1, 1) NOT NULL,
    [ApplicationName]   VARCHAR (50)  NOT NULL,
    [Description]       VARCHAR (500) NULL,
    [ApplicationCode]   VARCHAR (10)  NOT NULL,
    [DefaultView]       VARCHAR (250) NULL,
    [IsActive]          BIT           CONSTRAINT [DF_Application_IsActive] DEFAULT ((0)) NOT NULL,
    [IconFullName]      VARCHAR (250) NULL,
    [DateCreated]       DATETIME      NOT NULL,
    [DateUpdated]       DATETIME      NULL,
    [CreatedByUserID]   INT           NOT NULL,
    [UpdatedByUserID]   INT           NULL,
    [FromEmailAddress]  VARCHAR (250) NULL,
    [ErrorEmailAddress] VARCHAR (250) NULL,
    CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED ([ApplicationID] ASC)
);



