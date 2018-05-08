CREATE TABLE [dbo].[SubscriberOpenActivity] (
    [OpenActivityID]    INT  IDENTITY (1, 1) NOT NULL,
    [PubSubscriptionID] INT  NULL,
    [BlastID]           INT  NOT NULL,
    [ActivityDate]      DATE NOT NULL,
    [SubscriptionID]    INT  NULL,
    [DateNumber]        INT  NULL,
    CONSTRAINT [PK_SubscriberOpenActivity] PRIMARY KEY CLUSTERED ([OpenActivityID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SubscriberOpenActivity_PubSubscriptions] FOREIGN KEY ([PubSubscriptionID]) REFERENCES [dbo].[PubSubscriptions] ([PubSubscriptionID])
);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberOpenActivity_BlastID_ActivityDate]
    ON [dbo].[SubscriberOpenActivity]([BlastID] ASC, [ActivityDate] ASC)
    INCLUDE([PubSubscriptionID], [SubscriptionID]) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberOpenActivity_PubSubscriptionID]
    ON [dbo].[SubscriberOpenActivity]([PubSubscriptionID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberOpenActivity_SubscriptionID]
    ON [dbo].[SubscriberOpenActivity]([SubscriptionID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberOpenActivity_DateNumber_PubSubscriptionID] 
	ON [dbo].[SubscriberOpenActivity] ([DateNumber] ASC,[PubSubscriptionID] ASC)
	INCLUDE ([BlastID],[SubscriptionID])
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberOpenActivity_DateNumber_SubscriptionID] 
	ON [dbo].[SubscriberOpenActivity] ([DateNumber] ASC,[SubscriptionID] ASC)
	INCLUDE ( [PubSubscriptionID],[BlastID])
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberOpenActivity_ActivityDate_PubsubcriptionID] 
	ON [dbo].[SubscriberOpenActivity] ([ActivityDate] ASC,[PubSubscriptionID])
	INCLUDE ([BlastID],[SubscriptionID])
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberOpenActivity_ActivityDate_SubcriptionID] 
	ON [dbo].[SubscriberOpenActivity] ([ActivityDate] ASC,[SubscriptionID])
	INCLUDE ([BlastID],[PubSubscriptionID])
GO

-- create a new trigger
CREATE TRIGGER [dbo].[Trig_Insert_SubscriberOpenActivity]
ON [dbo].[SubscriberOpenActivity]
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
    JOIN dbo.SubscriberOpenActivity soa ON soa.OpenActivityID = i.OpenActivityID
END
GO

CREATE TRIGGER [dbo].[Trig_Update_SubscriberOpenActivity]
ON [dbo].[SubscriberOpenActivity] 
FOR  UPDATE
AS 
BEGIN   
    SET NOCOUNT ON;

    -- update your table, using a set-based approach
    -- from the "Inserted" pseudo table which CAN and WILL
    -- contain multiple rows!
    
    UPDATE soa 
    SET  DateNumber = Master.dbo.fn_GetDateDaysFromDate(i.activityDate) 
    FROM dbo.SubscriberOpenActivity soa
    JOIN Deleted d ON soa.OpenActivityID = d.OpenActivityID
    JOIN Inserted i on soa.OpenActivityID = i.OpenActivityID 
    where d.ActivityDate <> i.ActivityDate
END
GO

