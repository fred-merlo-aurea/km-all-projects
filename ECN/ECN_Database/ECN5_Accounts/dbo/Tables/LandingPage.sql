CREATE TABLE [dbo].[LandingPage] (
    [LPID]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)  NULL,
    [Description] VARCHAR (255) NULL,
    [IsActive]    BIT           NULL,
    [BaseChannel] BIT NULL, 
    [Customer] BIT NULL, 
    CONSTRAINT [LandingPage_PK] PRIMARY KEY CLUSTERED ([LPID] ASC) WITH (FILLFACTOR = 80)
);

