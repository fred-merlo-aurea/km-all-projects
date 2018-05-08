CREATE TABLE [dbo].[AcsMailerInfo]
(
	[AcsMailerInfoId] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [AcsMailerId] VARCHAR(9) NOT NULL, 
    [ImbSeqCounter] INT NULL DEFAULT (1), 
    [DateCreated] DATETIME NULL, 
    [DateUpdated] DATETIME NULL, 
    [CreatedByUserID] INT NULL, 
    [UpdatedByUserID] INT NULL
)
