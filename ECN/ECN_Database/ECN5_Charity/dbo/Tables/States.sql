CREATE TABLE [dbo].[States] (
    [StateID]      INT          IDENTITY (1, 1) NOT NULL,
    [CustomerID]   INT          NULL,
    [StateCode]    VARCHAR (2)  NULL,
    [StateName]    VARCHAR (50) NULL,
    [DefaultState] CHAR (1)     NULL,
    CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED ([StateID] ASC) WITH (FILLFACTOR = 80)
);

