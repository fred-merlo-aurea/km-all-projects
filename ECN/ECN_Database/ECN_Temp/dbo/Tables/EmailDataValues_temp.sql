CREATE TABLE [dbo].[EmailDataValues_temp] (
    [EmailDataValuesID] BIGINT           IDENTITY (1, 1) NOT NULL,
    [EmailID]           INT              NOT NULL,
    [GroupDatafieldsID] INT              NOT NULL,
    [DataValue]         VARCHAR (500)    NULL,
    [ModifiedDate]      DATETIME         NULL,
    [SurveyGridID]      INT              NULL,
    [EntryID]           UNIQUEIDENTIFIER NULL,
    [CreatedDate]       DATETIME         NULL,
    [CreatedUserID]     INT              NULL,
    [UpdatedUserID]     INT              NULL
);

