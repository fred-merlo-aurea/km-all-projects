CREATE TABLE [dbo].[UserDataMask]
(
	[UserDataMaskID] INT IDENTITY (1, 1) NOT NULL,
	[UserID] INT NOT NULL , 
    [MaskField] VARCHAR(100) NULL, 
    [CreatedDate] DATETIME NULL, 
    [CreatedUserID] INT NULL, 
    
    CONSTRAINT [PK_UserDataMask] PRIMARY KEY ([UserDataMaskID])
)
