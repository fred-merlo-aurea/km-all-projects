CREATE TABLE [dbo].[response] (
    [ResponseID]      INT           IDENTITY (1, 1) NOT NULL,
    [ParticipantID]   INT           NOT NULL,
    [QuestionID]      INT           NOT NULL,
    [response]        VARCHAR (255) NULL,
    [ResponseDate]    DATETIME      NULL,
    [GridStatementID] INT           NULL,
    CONSTRAINT [PK_response] PRIMARY KEY CLUSTERED ([ResponseID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_response_grid_statements] FOREIGN KEY ([GridStatementID]) REFERENCES [dbo].[grid_statements] ([GridStatementID]),
    CONSTRAINT [FK_response_participant] FOREIGN KEY ([ParticipantID]) REFERENCES [dbo].[participant] ([participant_id]),
    CONSTRAINT [FK_response_question1] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[question] ([QuestionID])
);


GO
CREATE NONCLUSTERED INDEX [IX_response_GridStatementID]
    ON [dbo].[response]([GridStatementID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_response_ParticipantID]
    ON [dbo].[response]([ParticipantID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_response_QuestionID]
    ON [dbo].[response]([QuestionID] ASC) WITH (FILLFACTOR = 80);

