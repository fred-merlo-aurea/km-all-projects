CREATE TABLE [dbo].[Template] (
    [TemplateID]          INT           IDENTITY (1, 1) NOT NULL,
    [BaseChannelID]       INT           NULL,
    [TemplateStyleCode]   VARCHAR (50)  NULL,
    [TemplateName]        VARCHAR (50)  NULL,
    [TemplateImage]       VARCHAR (255) NULL,
    [TemplateDescription] VARCHAR (255) NULL,
    [SortOrder]           INT           NULL,
    [SlotsTotal]          INT           NULL,
    [IsActive]            BIT           CONSTRAINT [DF_Template_IsActive] DEFAULT ((1)) NULL,
    [TemplateSource]      TEXT          NULL,
    [TemplateText]        TEXT          NULL,
    [TemplateSubject]     TEXT          NULL,
    [CreatedDate]         DATETIME      CONSTRAINT [DF_Template_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID]       INT           NULL,
    [IsDeleted]           BIT           CONSTRAINT [DF_Template_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]         DATETIME      NULL,
    [UpdatedUserID]       INT           NULL,
	[Category] [varchar](255) NULL,
    CONSTRAINT [Templates_PK] PRIMARY KEY CLUSTERED ([TemplateID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Template] TO [reader]
    AS [dbo];

CREATE NONCLUSTERED INDEX [IDX_StatusSendTimeOnEmailDirect] ON [dbo].[EmailDirect] 
(
                [Status] ASC,
                [SendTime] ASC
)

