CREATE TABLE [dbo].[EmailDataValues] (
    [EmailDataValuesID] BIGINT           IDENTITY (1, 1) NOT NULL,
    [EmailID]           INT              NOT NULL,
    [GroupDatafieldsID] INT              NOT NULL,
    [DataValue]         VARCHAR (500)    NULL,
    [ModifiedDate]      DATETIME         NULL,
    [SurveyGridID]      INT              NULL,
    [EntryID]           UNIQUEIDENTIFIER NULL,
    [CreatedDate]       DATETIME         CONSTRAINT [DF_EmailDataValues1_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID]     INT              NULL,
    [UpdatedUserID]     INT              NULL,
    CONSTRAINT [PK_EmailDataValues1] PRIMARY KEY CLUSTERED ([EmailDataValuesID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_EmailDataValues1_GroupDatafields] FOREIGN KEY ([GroupDatafieldsID]) REFERENCES [dbo].[GroupDatafields] ([GroupDatafieldsID])
);




GO
CREATE NONCLUSTERED INDEX [IX_EmailDataValue1_GroupDatafieldsIDEmailDataValueId]
    ON [dbo].[EmailDataValues]([GroupDatafieldsID] ASC, [EmailDataValuesID] ASC)
    INCLUDE([EmailID], [DataValue], [ModifiedDate], [SurveyGridID], [EntryID]);


GO
CREATE NONCLUSTERED INDEX [IX_EmailDataValue1_GroupDatafieldsID_DataValue]
    ON [dbo].[EmailDataValues]([GroupDatafieldsID] ASC, [DataValue] ASC)
    INCLUDE([EmailID]);


GO



GO
CREATE NONCLUSTERED INDEX [IX_EmailDataValue1_EmailID_GroupDatafieldsID]
    ON [dbo].[EmailDataValues]([EmailID] ASC, [GroupDatafieldsID] ASC);


GO


