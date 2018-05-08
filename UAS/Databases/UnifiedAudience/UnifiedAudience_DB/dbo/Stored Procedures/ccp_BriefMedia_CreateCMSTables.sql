CREATE PROCEDURE [dbo].[ccp_BriefMedia_CreateCMSTables]
WITH EXECUTE AS 'dbo'
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempBriefMediaBMWU]') AND type in (N'U'))
	DROP TABLE [dbo].[tempBriefMediaBMWU]
	
	CREATE TABLE tempBriefMediaBMWU
	(
		DrupalID int null,
		AccessID varchar(max) null,
		FirstName varchar(75) null,
		LastName varchar(75) null,
		Email varchar(150) null,
		TopicCodes varchar(max) null,
		PageID varchar(max) null,
		SearchTerm varchar(max) null	
	)
	
	CREATE NONCLUSTERED INDEX [IDX_DrupalId] ON tempBriefMediaBMWU 
	(
		DrupalID ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]



	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempBriefMediaBMWUFinal]') AND type in (N'U'))
	DROP TABLE [dbo].[tempBriefMediaBMWUFinal]
	
	CREATE TABLE [tempBriefMediaBMWUFinal]
	(
		DrupalID int null,
		AccessID varchar(max) null,
		FirstName varchar(75) null,
		LastName varchar(75) null,
		Email varchar(150) null,
		TopicCodes varchar(max) null,
		PageID varchar(max) null,
		SearchTerm varchar(max) null	
	)
	
	CREATE NONCLUSTERED INDEX [IDX_DrupalIdFinal] ON [tempBriefMediaBMWUFinal] 
	(
		DrupalID ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]

END
GO