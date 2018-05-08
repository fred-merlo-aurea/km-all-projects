CREATE TABLE [dbo].[GroupDatafields] (
    [GroupDatafieldsID] INT           IDENTITY (1, 1) NOT NULL,
    [GroupID]           INT           NOT NULL,
    [ShortName]         VARCHAR (50)  NULL,
    [LongName]          VARCHAR (255) NULL,
    [SurveyID]          INT           NULL,
    [DatafieldSetID]    INT           NULL,
    [IsPublic]          CHAR (1)      CONSTRAINT [DF_GroupDatafields_IsPublic] DEFAULT ('N') NULL,
    [IsPrimaryKey]      BIT           CONSTRAINT [DF_GroupDatafields_IsPrimaryKey] DEFAULT ((0)) NULL,
    [CreatedDate]       DATETIME      CONSTRAINT [DF_GroupDatafields_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID]     INT           NULL,
    [IsDeleted]         BIT           CONSTRAINT [DF_GroupDatafields_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]       DATETIME      NULL,
    [UpdatedUserID]     INT           NULL,
    CONSTRAINT [PK_GroupDatafields] PRIMARY KEY CLUSTERED ([GroupDatafieldsID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_GroupID]
    ON [dbo].[GroupDatafields]([GroupID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_SurveyID]
    ON [dbo].[GroupDatafields]([SurveyID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_GroupID_SurveyID]
    ON [dbo].[GroupDatafields]([GroupID] ASC, [SurveyID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [_dta_index_GroupDatafields_13_341576255__K2_K1_3_4_5_6]
    ON [dbo].[GroupDatafields]([GroupID] ASC, [GroupDatafieldsID] ASC)
    INCLUDE([ShortName], [LongName], [SurveyID], [DatafieldSetID]) WITH (FILLFACTOR = 80);


GO
CREATE STATISTICS [_dta_stat_341576255_8_2]
    ON [dbo].[GroupDatafields]([IsPrimaryKey], [GroupID]);


GO
CREATE STATISTICS [_dta_stat_341576255_2_1_3]
    ON [dbo].[GroupDatafields]([GroupID], [GroupDatafieldsID], [ShortName]);


GO
GRANT DELETE
    ON OBJECT::[dbo].[GroupDatafields] TO [ecn5writer]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[GroupDatafields] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GroupDatafields] TO [ecn5writer]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[GroupDatafields] TO [ecn5writer]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GroupDatafields] TO [reader]
    AS [dbo];

