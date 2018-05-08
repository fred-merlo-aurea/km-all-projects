CREATE TABLE [dbo].[SurveyBranching] (
    [SurveyID]   INT NOT NULL,
    [QuestionID] INT NOT NULL,
    [OptionID]   INT NOT NULL,
    [PageID]     INT NULL,
    [EndSurvey]  BIT NULL,
    CONSTRAINT [FK_survey_branching_Page] FOREIGN KEY ([PageID]) REFERENCES [dbo].[Page] ([PageID]),
    CONSTRAINT [FK_survey_branching_question] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[question] ([QuestionID]),
    CONSTRAINT [FK_survey_branching_response_options] FOREIGN KEY ([OptionID]) REFERENCES [dbo].[response_options] ([OptionID]),
    CONSTRAINT [FK_survey_branching_survey] FOREIGN KEY ([SurveyID]) REFERENCES [dbo].[Survey] ([SurveyID])
);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyBranching_SurveyID]
    ON [dbo].[SurveyBranching]([SurveyID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyBranching_PageID]
    ON [dbo].[SurveyBranching]([PageID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyBranching_QuestionID]
    ON [dbo].[SurveyBranching]([QuestionID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyBranching_OptionID]
    ON [dbo].[SurveyBranching]([OptionID] ASC) WITH (FILLFACTOR = 80);

