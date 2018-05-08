CREATE TABLE [dbo].[SubscriberVisitActivity] (
    [VisitActivityID]  INT           IDENTITY (1, 1) NOT NULL,
    [SubscriptionID]   INT           NULL,
    [DomainTrackingID] INT           NULL,
    [URL]              VARCHAR (500) NULL,
    [ActivityDate]     DATETIME      NULL,
	DateNumber int null,
    CONSTRAINT [PK_SubscriberVisitActivity] PRIMARY KEY CLUSTERED ([VisitActivityID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SubscriberVisitActivity_Subscriptions] FOREIGN KEY ([SubscriptionID]) REFERENCES [dbo].[Subscriptions] ([SubscriptionID])
);
GO
CREATE NONCLUSTERED INDEX [IDX_SubscriberVisitActivity_DateNumber_SubscriptionID]
    ON [dbo].[SubscriberVisitActivity]([DateNumber] ASC)
    INCLUDE([SubscriptionID]);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberVisitActivity_SubscriptionID]
    ON [dbo].[SubscriberVisitActivity]([SubscriptionID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IX_SubscriberVisitActivity_ActivityDate_SubscriptionID]
    ON [dbo].[SubscriberVisitActivity]([ActivityDate] ASC)
    INCLUDE([SubscriptionID]);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberVisitActivity_domainTrackingID] 
	ON [dbo].[SubscriberVisitActivity] ([DomainTrackingID]) INCLUDE ([SubscriptionID])  WITH (FILLFACTOR = 90);
GO

-- drop the existing trigger
--DROP TRIGGER [dbo].[Trig_Insert_SubscriberVisitActivity] 
--GO

-- create a new trigger
CREATE TRIGGER [dbo].[Trig_Insert_SubscriberVisitActivity]
ON [dbo].[SubscriberVisitActivity]
AFTER INSERT
AS 
BEGIN   
    SET NOCOUNT ON;

    -- update your table, using a set-based approach
    -- from the "Inserted" pseudo table which CAN and WILL
    -- contain multiple rows!
    
    UPDATE soa 
    SET  DateNumber = Master.dbo.fn_GetDateDaysFromDate(i.activityDate) 
    FROM Inserted i
    JOIN dbo.SubscriberVisitActivity soa ON soa.VisitActivityID = i.VisitActivityID
END
GO

-- drop the existing trigger
--DROP TRIGGER [dbo].[Trig_Update_SubscriberVisitActivity] 
--GO

CREATE TRIGGER [dbo].[Trig_Update_SubscriberVisitActivity]
ON [dbo].[SubscriberVisitActivity] 
FOR  UPDATE
AS 
BEGIN   
    SET NOCOUNT ON;

    -- update your table, using a set-based approach
    -- from the "Inserted" pseudo table which CAN and WILL
    -- contain multiple rows!
    
    UPDATE soa 
    SET  DateNumber = Master.dbo.fn_GetDateDaysFromDate(i.activityDate) 
    FROM dbo.SubscriberVisitActivity soa
    JOIN Inserted i ON soa.VisitActivityID = i.VisitActivityID
    JOIN Deleted d ON soa.VisitActivityID = d.VisitActivityID
    where d.ActivityDate <> i.ActivityDate
END
GO

