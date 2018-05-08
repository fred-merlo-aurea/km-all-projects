CREATE TABLE [dbo].[RoleAction] (
    [RoleActionID]  INT      IDENTITY (1, 1) NOT NULL,
    [RoleID]        INT      NULL,
    [ActionID]      INT      NULL,
    [Active]        CHAR (1) CONSTRAINT [DF_RoleActions_Active] DEFAULT ('N') NOT NULL,
    [CreatedUserID] INT      NULL,
    [CreatedDate]   DATETIME CONSTRAINT [DF_RoleAction_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedUserID] INT      NULL,
    [UpdatedDate]   DATETIME NULL,
    [IsDeleted]     BIT      CONSTRAINT [DF_RoleAction_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RoleActions] PRIMARY KEY CLUSTERED ([RoleActionID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_RoleActions_RoleID_ActionID]
    ON [dbo].[RoleAction]([RoleID] ASC, [ActionID] ASC)
    INCLUDE([Active]) WITH (FILLFACTOR = 80);

