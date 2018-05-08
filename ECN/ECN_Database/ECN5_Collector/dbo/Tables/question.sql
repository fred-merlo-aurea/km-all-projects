CREATE TABLE [dbo].[question] (
    [QuestionID]        INT          IDENTITY (1, 1) NOT NULL,
    [SurveyID]          INT          NOT NULL,
    [number]            INT          NOT NULL,
    [PageID]            INT          NOT NULL,
    [format]            VARCHAR (25) NOT NULL,
    [grid_control_type] CHAR (1)     NULL,
    [QuestionText]      TEXT         NULL,
    [preface]           CHAR (1)     CONSTRAINT [DF_question_preface] DEFAULT ('N') NULL,
    [maxlength]         INT          NULL,
    [Required]          BIT          NULL,
    [GridValidation]    INT          NULL,
    [ShowTextControl]   BIT          NULL,
    [CreatedDate]       DATETIME     NULL,
    [CreatedUserID]     INT          NULL,
    [UpdatedDate]       DATETIME     NULL,
    [UpdatedUserID]     INT          NULL,
    CONSTRAINT [PK_question] PRIMARY KEY CLUSTERED ([QuestionID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_question_Page] FOREIGN KEY ([PageID]) REFERENCES [dbo].[Page] ([PageID]),
    CONSTRAINT [FK_question_survey] FOREIGN KEY ([SurveyID]) REFERENCES [dbo].[Survey] ([SurveyID])
);


GO
CREATE NONCLUSTERED INDEX [IX_question_SurveyID]
    ON [dbo].[question]([SurveyID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_question_PageID]
    ON [dbo].[question]([PageID] ASC) WITH (FILLFACTOR = 80);

