CREATE TABLE [dbo].[LandingPageOption] (
    [LPOID]       INT           IDENTITY (1, 1) NOT NULL,
    [LPID]        INT           NULL,
    [Name]        VARCHAR (50)  NULL,
    [Description] VARCHAR (255) NULL,
    [IsActive]    BIT           NULL,
    CONSTRAINT [LandingPageOption_PK] PRIMARY KEY CLUSTERED ([LPOID] ASC) WITH (FILLFACTOR = 80)
);

