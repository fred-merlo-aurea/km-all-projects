CREATE TABLE [dbo].[CodeType]
(
	CodeTypeId int Identity(1,1) NOT NULL,
	CodeTypeName varchar(50) NOT NULL,
	CodeTypeDescription varchar(max) NULL,
    [IsActive]             BIT      NOT NULL,
    [DateCreated]          DATETIME NOT NULL,
    [DateUpdated]          DATETIME NULL,
    [CreatedByUserID]      INT      NOT NULL,
    [UpdatedByUserID]      INT      NULL, 
    CONSTRAINT [PK_CodeType] PRIMARY KEY ([CodeTypeId]), 
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_CategoryCode_CategoryCodeID]
    ON [dbo].[CategoryCode]([CategoryCodeID] ASC);
GO
