CREATE TABLE [dbo].[pharmalive_TW_and_PI_in_MasterGroup_backup] (
    [EmailDataValuesID] INT              IDENTITY (1, 1) NOT NULL,
    [EmailID]           INT              NOT NULL,
    [GroupDatafieldsID] INT              NOT NULL,
    [DataValue]         VARCHAR (500)    NULL,
    [ModifiedDate]      DATETIME         NULL,
    [SurveyGridID]      INT              NULL,
    [EntryID]           UNIQUEIDENTIFIER NULL
);

