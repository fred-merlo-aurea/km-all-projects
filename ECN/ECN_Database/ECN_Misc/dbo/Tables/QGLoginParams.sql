CREATE TABLE [dbo].[QGLoginParams] (
    [EmailID]         INT           NOT NULL,
    [AutoLoginParams] VARCHAR (255) NULL,
    [Publisher]       VARCHAR (50)  NOT NULL,
    [Magazine]        VARCHAR (50)  NOT NULL,
    [Issue]           VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_QGLoginParams] PRIMARY KEY CLUSTERED ([EmailID] ASC, [Publisher] ASC, [Magazine] ASC, [Issue] ASC) WITH (FILLFACTOR = 80)
);

