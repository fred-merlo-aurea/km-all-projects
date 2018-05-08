CREATE TABLE [dbo].[grid_statements] (
    [GridStatementID] INT           IDENTITY (1, 1) NOT NULL,
    [QuestionID]      INT           NULL,
    [GridStatement]   VARCHAR (255) NULL,
    CONSTRAINT [PK_grid_option] PRIMARY KEY CLUSTERED ([GridStatementID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_grid_statements_question] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[question] ([QuestionID])
);


GO
CREATE NONCLUSTERED INDEX [IX_grid_statements_QuestionID]
    ON [dbo].[grid_statements]([QuestionID] ASC) WITH (FILLFACTOR = 80);

