CREATE TABLE [dbo].[BlastPlans] (
    [BlastPlanID]   INT          IDENTITY (1, 1) NOT NULL,
    [BlastID]       INT          NULL,
    [CustomerID]    INT          NULL,
    [GroupID]       INT          NULL,
    [EventType]     VARCHAR (50) NULL,
    [Period]        FLOAT (53)   NULL,
    [BlastDay]      INT          NULL,
    [PlanType]      VARCHAR (50) NULL,
    [CreatedDate]   DATETIME     CONSTRAINT [DF_BlastPlans_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT          NULL,
    [IsDeleted]     BIT          CONSTRAINT [DF_BlastPlans_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME     NULL,
    [UpdatedUserID] INT          NULL,
    CONSTRAINT [PK_BlastPlans] PRIMARY KEY CLUSTERED ([BlastPlanID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[BlastPlans] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[BlastPlans] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastPlans] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[BlastPlans] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[BlastPlans] TO [reader]
    AS [dbo];

