CREATE TABLE [dbo].[participant] (
    [participant_id] INT           IDENTITY (1, 1) NOT NULL,
    [company_id]     INT           NULL,
    [user_name]      VARCHAR (50)  NULL,
    [password]       VARCHAR (50)  NULL,
    [last_name]      VARCHAR (50)  NULL,
    [mid_name]       VARCHAR (50)  NULL,
    [first_name]     VARCHAR (50)  NULL,
    [title]          VARCHAR (25)  NULL,
    [address1]       VARCHAR (50)  NULL,
    [address2]       VARCHAR (50)  NULL,
    [city]           VARCHAR (50)  NULL,
    [state]          VARCHAR (50)  NULL,
    [country]        VARCHAR (50)  NULL,
    [phone1]         VARCHAR (25)  NULL,
    [phone2]         VARCHAR (25)  NULL,
    [fax]            VARCHAR (15)  NULL,
    [email]          VARCHAR (255) NULL,
    CONSTRAINT [PK_participant] PRIMARY KEY CLUSTERED ([participant_id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_participant_company] FOREIGN KEY ([company_id]) REFERENCES [dbo].[company] ([company_id])
);


GO
CREATE NONCLUSTERED INDEX [IX_participant_company_id]
    ON [dbo].[participant]([company_id] ASC) WITH (FILLFACTOR = 80);

