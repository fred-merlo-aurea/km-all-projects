CREATE TABLE [dbo].[DomainTracking] (
    [DomainTrackingID] INT           IDENTITY (1, 1) NOT NULL,
    [DomainName]       VARCHAR (200) NULL,
    [DateCreated]      DATETIME      CONSTRAINT [DF_DomainTracking_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]      DATETIME      NULL,
    [CreatedByUserID]  INT           NULL,
    [UpdatedByUserID]  INT           NULL,
    CONSTRAINT [PK_DomainTracking] PRIMARY KEY CLUSTERED ([DomainTrackingID] ASC) WITH (FILLFACTOR = 90)
);



