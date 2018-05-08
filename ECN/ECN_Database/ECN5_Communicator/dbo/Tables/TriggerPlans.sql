CREATE TABLE [dbo].[TriggerPlans] (
    [TriggerPlanID] INT             IDENTITY (1, 1) NOT NULL,
    [RefTriggerID]  INT             NULL,
    [EventType]     VARCHAR (10)    CONSTRAINT [DF_TriggerPlans_EventType] DEFAULT ('noopen') NULL,
    [BlastID]       INT             NULL,
    [Period]        DECIMAL (18, 8) NULL,
    [Criteria]      VARCHAR (50)    NULL,
    [CustomerID]    INT             NULL,
    [ActionName]    VARCHAR (50)    NULL,
    [GroupID]       INT             CONSTRAINT [DF_TriggerPlans_GroupID] DEFAULT (0) NULL,
    [Status]        CHAR (1)        NULL,
    [CreatedDate]   DATETIME        CONSTRAINT [DF_TriggerPlans_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT             NULL,
    [IsDeleted]     BIT             CONSTRAINT [DF_TriggerPlans_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME        NULL,
    [UpdatedUserID] INT             NULL,
    CONSTRAINT [PK_TriggerPlans] PRIMARY KEY CLUSTERED ([TriggerPlanID] ASC) WITH (FILLFACTOR = 80)
);

