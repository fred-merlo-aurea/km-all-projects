CREATE TABLE [dbo].[ControlCategory]
(
	[ControlCategoryID] INT IDENTITY (1, 1) NOT NULL, 
    [Control_ID] INT NOT NULL, 
    [LabelHTML] VARCHAR(MAX) NULL, 
    [Order] INT NULL,
	CONSTRAINT [PK_ControlCategory] PRIMARY KEY CLUSTERED ([ControlCategoryID] ASC),
	CONSTRAINT [FK_Control_ControlCategory] FOREIGN KEY ([Control_ID]) REFERENCES [dbo].[Control] ([Control_ID]) ON DELETE CASCADE
)
