CREATE proc [dbo].[sp_NEBook_SurveyStatistics]
(
	@surveyID int,
	@userID int,
	@startDate varchar(10),
	@endDate   varchar(10)
)
as
Begin
	Declare @Surveys TABLE (SURVEYID int)
/*
select * from nebook_regions
select * from nebook_superregions
select * from nebook_region_superregion
select * from nebook_customers

	if exists (select top 1 * from nebook_superregions where userID = @userID)
		insert into @surveys
		select	surveyID 
		from	NEBook_CopiedSurveys ncs join	
				ecn_collector..surveys	s on s.survey_ID = ncs.copiedsurveyID join
				ecncustomers
	else if exists (select top 1 * from nebook_regions where userID = @userID)
	
	else 
	insert into @surveys
		select @surveyID union all
		select CopiedSurveyID from ecn_misc..NEBook_CopiedSurveys ncs where ncs.surveyID = @surveyID
*/

	select	c.customerID, 
			c.customername, 
			replace(contacttitle, 'STOREID:','') as storename,
			s.surveyTitle as survey_Title, 
			r.RegionID,
			r.RegionName, 
			sr.SuperRegionID,
			sr.SuperRegionName,
			nc.goals,
			(
				select COUNT(DISTINCT(EmailID)) FROM ecn5_communicator..EmailDataValues 
				WHERE isnull(DataValue,'') <> '' 
				and GroupDatafieldsID in 
					(	
						select	groupdatafieldsID 
						from	ecn5_communicator..groupdatafields gdf 
						where	shortname=Convert(varchar,s.surveyID)+ '_completionDt' and 
								gdf.surveyID = s.surveyID and 
								gdf.groupID = s.groupID 
					) and convert(datetime, convert(varchar(10), convert(datetime, datavalue),101))  between @startDate and @endDate 
			) as responses,
			isnull(dbo.getScoreForSurvey(s.Surveyid, @startDate, @endDate),0) as score
	from	ecn5_collector..survey s join 
			ecn5_accounts..customer c on s.customerID = c.customerID join
			ecn_misc..nebook_customers nc on c.customerID = nc.customerID join
			ecn_misc..nebook_regions r on nc.regionID = r.regionID join		
			ecn_misc..NEBOOK_Region_SuperRegion rsr on rsr.regionID = r.regionID join		
			ecn_misc..nebook_superregions sr on sr.superregionID = rsr.superregionID 
	where 
			s.SurveyID = @surveyID or s.SurveyID in (select CopiedSurveyID from ecn_misc..NEBook_CopiedSurveys ncs where ncs.surveyID = @surveyID)
End
