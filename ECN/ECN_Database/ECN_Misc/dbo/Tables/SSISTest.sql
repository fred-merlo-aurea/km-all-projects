CREATE TABLE [dbo].[SSISTest] (
    [ID]         INT         IDENTITY (1, 1) NOT NULL,
    [Zipcode]    VARCHAR (5) NULL,
    [PickupDate] DATETIME    NULL,
    [ClientID]   INT         NOT NULL,
    CONSTRAINT [PK_SSISTest] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 80)
);

