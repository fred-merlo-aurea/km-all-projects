CREATE TABLE [dbo].[Page] (
    [PageID]        INT           IDENTITY (1, 1) NOT NULL,
    [SurveyID]      INT           NOT NULL,
    [PageHeader]    VARCHAR (500) NULL,
    [PageDesc]      VARCHAR (500) NULL,
    [number]        INT           NOT NULL,
    [CreatedDate]   DATETIME      NULL,
    [CreatedUserID] INT           NULL,
    [UpdatedDate]   DATETIME      NULL,
    [UpdatedUserID] INT           NULL,
    CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED ([PageID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Page_survey] FOREIGN KEY ([SurveyID]) REFERENCES [dbo].[Survey] ([SurveyID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Page_SurveyID]
    ON [dbo].[Page]([SurveyID] ASC) WITH (FILLFACTOR = 80);

