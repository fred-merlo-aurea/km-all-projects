CREATE TABLE [dbo].[Notification] (
    [Notification_Seq_ID]   INT            IDENTITY (1, 1) NOT NULL,
    [ConditionGroup_Seq_ID] INT            NULL,
    [IsConfirmation]        BIT            NOT NULL,
    [IsInternalUser]        BIT            NOT NULL,
    [FromName]              NVARCHAR (100) NULL,
    [ToEmail]               NVARCHAR (200) NULL,
    [Subject]               NVARCHAR (100) NULL,
    [Message]               NVARCHAR (MAX) NOT NULL,
    [Form_Seq_ID]           INT            NOT NULL,
    [LandingPage]           NVARCHAR (MAX) NULL,
    [IsDoubleOptIn]         BIT            NOT NULL,
    CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED ([Notification_Seq_ID] ASC),
    CONSTRAINT [FK_ConditionGroup_Notification] FOREIGN KEY ([ConditionGroup_Seq_ID]) REFERENCES [dbo].[ConditionGroup] ([ConditionGroup_Seq_ID]),
    CONSTRAINT [FK_Form_Notification] FOREIGN KEY ([Form_Seq_ID]) REFERENCES [dbo].[Form] ([Form_Seq_ID]) ON DELETE CASCADE
);

