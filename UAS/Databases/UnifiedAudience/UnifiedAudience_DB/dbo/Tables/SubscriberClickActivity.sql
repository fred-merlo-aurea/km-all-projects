CREATE TABLE [dbo].[SubscriberClickActivity] (
    [ClickActivityID]   INT            IDENTITY (1, 1) NOT NULL,
    [PubSubscriptionID] INT            NULL,
    [BlastID]           INT            NULL,
    [Link]              VARCHAR (2048) NULL,
    [LinkAlias]         VARCHAR (100)  NULL,
    [LinkSource]        VARCHAR (50)   NULL,
    [LinkType]          VARCHAR (50)   NULL,
    [ActivityDate]      DATE           NOT NULL,
	[SubscriptionID]    INT            NULL,
	DateNumber int null,
    CONSTRAINT [PK_SubscriberClickActivity] PRIMARY KEY CLUSTERED ([ClickActivityID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SubscriberClickActivity_PubSubscriptions] FOREIGN KEY ([PubSubscriptionID]) REFERENCES [dbo].[PubSubscriptions] ([PubSubscriptionID])
);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberClickActivity_LinkAlias]
    ON [dbo].[SubscriberClickActivity]([Link] ASC, [LinkAlias] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberClickActivity_PubSubscriptionID]
    ON [dbo].[SubscriberClickActivity]([PubSubscriptionID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberClickActivity_SubscriptionID]
    ON [dbo].[SubscriberClickActivity]([SubscriptionID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberClickActivity_ActivityDate_SubcriptionID] 
	ON [dbo].[SubscriberClickActivity] ([ActivityDate] ASC,[SubscriptionID])
	INCLUDE ([BlastID],[PubSubscriptionID]
)
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberClickActivity_ActivityDate_PubsubcriptionID] 
	ON [dbo].[SubscriberClickActivity] ([ActivityDate] ASC,[PubSubscriptionID])
	INCLUDE ([BlastID],[SubscriptionID])
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberClickActivity_DateNumber_PubSubscriptionID] 
	ON [dbo].[SubscriberClickActivity] ([DateNumber] ASC,[PubSubscriptionID] ASC)
	INCLUDE ([BlastID], [SubscriptionID])
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberClickActivity_DateNumber_SubscriptionID] 
	ON [dbo].[SubscriberClickActivity] ([DateNumber] ASC,[SubscriptionID] ASC)
	INCLUDE ( [PubSubscriptionID], [BlastID])
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberClickActivity_BlastID_ActivityDate] 
	ON [dbo].[SubscriberClickActivity] ([BlastID] ASC,[ActivityDate] ASC)
	INCLUDE ( [PubSubscriptionID], [SubscriptionID])
GO

CREATE NONCLUSTERED INDEX [IDX_SubscriberClickActivity_BlastID_DateNumber] 
	ON [dbo].[SubscriberClickActivity] ([BlastID] ASC,[DateNumber] ASC)
	INCLUDE ( [PubSubscriptionID], [SubscriptionID])
GO

-- drop the existing trigger
--DROP TRIGGER [dbo].[Trig_Insert_SubscriberClickActivity] 
--GO

-- create a new trigger
CREATE TRIGGER [dbo].[Trig_Insert_SubscriberClickActivity]
ON [dbo].[SubscriberClickActivity]
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
    JOIN dbo.SubscriberClickActivity soa ON soa.ClickActivityID = i.ClickActivityID
END
GO

-- drop the existing trigger
--DROP TRIGGER [dbo].[Trig_Update_SubscriberClickActivity] 
--GO

 
CREATE TRIGGER [dbo].[Trig_Update_SubscriberClickActivity]
ON [dbo].[SubscriberClickActivity] 
FOR  UPDATE
AS 
BEGIN   
    SET NOCOUNT ON;

    -- update your table, using a set-based approach
    -- from the "Inserted" pseudo table which CAN and WILL
    -- contain multiple rows!
    
    UPDATE soa 
    SET  soa.DateNumber = master.dbo.fn_GetDateDaysFromDate(i.activityDate) 
    FROM dbo.SubscriberClickActivity  soa
    JOIN Deleted d ON soa.ClickActivityID = d.ClickActivityID
    JOIN Inserted i ON soa.ClickActivityID = i.ClickActivityID
    where d.ActivityDate <> i.ActivityDate
END
GO