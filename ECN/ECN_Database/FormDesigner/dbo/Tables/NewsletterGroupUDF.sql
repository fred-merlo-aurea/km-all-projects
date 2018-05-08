CREATE TABLE [dbo].[NewsletterGroupUDF]
(
	[NewsletterGroupUDFID] INT IDENTITY (1, 1) NOT NULL, 
    [NewsletterGroupID] INT NOT NULL, 
    [FormGroupDataFieldID] INT NOT NULL, 
    [NewsletterDataFieldID] INT NOT NULL, 
    CONSTRAINT [PK_NewsletterGroupUDF] PRIMARY KEY CLUSTERED ([NewsletterGroupUDFID] ASC),
    CONSTRAINT [FK_NewsletterGroup_NewsletterGroupUDF] FOREIGN KEY ([NewsletterGroupID]) REFERENCES [dbo].[NewsletterGroup] ([NewsletterGroupID]) ON DELETE CASCADE
)
