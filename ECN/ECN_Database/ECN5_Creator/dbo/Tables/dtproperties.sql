CREATE TABLE [dbo].[dtproperties] (
    [id]       INT            IDENTITY (1, 1) NOT NULL,
    [objectid] INT            NULL,
    [property] VARCHAR (64)   NOT NULL,
    [value]    VARCHAR (255)  NULL,
    [uvalue]   NVARCHAR (255) NULL,
    [lvalue]   IMAGE          NULL,
    [version]  INT            CONSTRAINT [DF__dtpropert__versi__0CBAE877] DEFAULT (0) NOT NULL,
    CONSTRAINT [pk_dtproperties] PRIMARY KEY CLUSTERED ([id] ASC, [property] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[dtproperties] TO PUBLIC
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dtproperties] TO PUBLIC
    AS [dbo];


GO
GRANT REFERENCES
    ON OBJECT::[dbo].[dtproperties] TO PUBLIC
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dtproperties] TO PUBLIC
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dtproperties] TO PUBLIC
    AS [dbo];

