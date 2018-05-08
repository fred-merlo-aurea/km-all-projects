CREATE TABLE [dbo].[response_options] (
    [OptionID]    INT           IDENTITY (1, 1) NOT NULL,
    [QuestionID]  INT           NOT NULL,
    [OptionValue] VARCHAR (500) NULL,
    [score]       INT           NULL,
    CONSTRAINT [PK_response_options] PRIMARY KEY CLUSTERED ([OptionID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_response_options_question1] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[question] ([QuestionID])
);


GO
CREATE NONCLUSTERED INDEX [IX_response_options_QuestionID]
    ON [dbo].[response_options]([QuestionID] ASC) WITH (FILLFACTOR = 80);

