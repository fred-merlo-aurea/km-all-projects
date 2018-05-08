CREATE TABLE [dbo].[NewsletterGroup]
(
	[NewsletterGroupID] INT IDENTITY (1, 1) NOT NULL, 
	[Control_ID] INT NOT NULL, 
    [CustomerID] INT NOT NULL, 
	[GroupID] INT NOT NULL,
    [ControlCategoryID] INT NULL, 
    [Order] INT NULL, 
    [IsPreSelected] BIT NOT NULL, 
    [LabelHTML] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_NewsletterGroup] PRIMARY KEY CLUSTERED ([NewsletterGroupID] ASC),
    CONSTRAINT [FK_Control_NewsletterGroup] FOREIGN KEY ([Control_ID]) REFERENCES [dbo].[Control] ([Control_ID]) ON DELETE CASCADE
)
