CREATE TABLE [dbo].[UserActions] (
    [UserActionID] INT      IDENTITY (1, 1) NOT NULL,
    [UserID]       INT      NULL,
    [ActionID]     INT      NULL,
    [Active]       CHAR (1) CONSTRAINT [DF_UserActions_Active] DEFAULT ('N') NOT NULL,
    CONSTRAINT [PK_UserActions] PRIMARY KEY CLUSTERED ([UserActionID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_UserActions_userID_ActionID]
    ON [dbo].[UserActions]([UserID] ASC, [ActionID] ASC)
    INCLUDE([UserActionID]) WITH (FILLFACTOR = 80);

