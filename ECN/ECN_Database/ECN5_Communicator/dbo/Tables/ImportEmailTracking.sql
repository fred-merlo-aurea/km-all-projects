CREATE TABLE [dbo].[ImportEmailTracking] (
    [TrackingID] INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID] INT           NOT NULL,
    [GroupID]    INT           NOT NULL,
    [StartTime]  DATETIME      NULL,
    [EndTime]    DATETIME      NULL,
    [EmailCount] INT           NULL,
    [xmlProfile] VARCHAR (MAX) NULL,
    [xmlUDF]     VARCHAR (MAX) NULL,
    [source]     VARCHAR (200) NULL,
    CONSTRAINT [PK_ImportEmailTracking] PRIMARY KEY CLUSTERED ([TrackingID] ASC) WITH (FILLFACTOR = 80)
);



