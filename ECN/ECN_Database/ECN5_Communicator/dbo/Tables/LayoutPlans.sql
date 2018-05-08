CREATE TABLE [dbo].[LayoutPlans] (
    [LayoutPlanID]  INT             IDENTITY (1, 1) NOT NULL,
    [LayoutID]      INT             NULL,
    [EventType]     VARCHAR (10)    NULL,
    [BlastID]       INT             NULL,
    [Period]        DECIMAL (18, 8) NULL,
    [Criteria]      VARCHAR (255)   NULL,
    [CustomerID]    INT             NULL,
    [ActionName]    VARCHAR (50)    NULL,
    [GroupID]       INT             NULL,
    [Status]        CHAR (1)        NULL,
    [SmartFormID]   INT             DEFAULT ((0)) NULL,
    [CreatedDate]   DATETIME        CONSTRAINT [DF_LayoutPlans_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT             NULL,
    [IsDeleted]     BIT             CONSTRAINT [DF_LayoutPlans_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME        NULL,
    [UpdatedUserID] INT             NULL,
    [CampaignItemID] INT NULL, 
    [TokenUID] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [PK_LayoutEvents] PRIMARY KEY CLUSTERED ([LayoutPlanID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[LayoutPlans] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[LayoutPlans] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[LayoutPlans] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[LayoutPlans] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[LayoutPlans] TO [reader]
    AS [dbo];

