CREATE Procedure [dbo].[ccp_Northstar_CreateTempCMSTables]
AS
BEGIN

	set nocount on

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tempNorthstarWebPersonGroup]') AND type in (N'U'))
	DROP TABLE [dbo].[tempNorthstarWebPersonGroup]

	CREATE TABLE [tempNorthstarWebPersonGroup]
	(
		[PubCode] varchar(500) null,
		[GlobalUserKey] int null,
		[Email] varchar(500) null,
		[LoginDate] varchar(500) null,
		[UpdateDate] varchar(500) null,
		[IsLegacy] varchar(500) null,	
		[FirstName] varchar(500) null,	
		[LastName] varchar(500) null,	
		[AccountNbr] varchar(500) null,	
		[Status] varchar(500) null,	
		[Title] varchar(500) null,	
		[Industry] varchar(500) null,	
		[CompanyName] varchar(500) null,	
		[Address] varchar(500) null,	
		[City] varchar(500) null,	
		[State] varchar(500) null,	
		[Country] varchar(500) null,	
		[Zip] varchar(500) null,	
		[SalesVolume] varchar(500) null,	
		[Purchased] varchar(500) null,	
		[DestBooked] varchar(500) null,	
		[Affiliations] varchar(500) null,	
		[Services] varchar(500) null,	
		[JobResp] varchar(500) null,	
		[PlanResp] varchar(500) null,	
		[FacType] varchar(500) null,	
		[HoldOffSite] varchar(500) null,	
		[HoldOffSiteAreas] varchar(500) null,	
		[EstAttendance] varchar(500) null,	
		[PrimBusiness] varchar(500) null,		
		--PubCode	
		--GlobalUserKey	
		[GroupId] varchar(500) null,	
		[IsRecent] varchar(500) null,	
		[AddDate] varchar(500) null,	
		[DropDate] varchar(500) null
	)

END
GO