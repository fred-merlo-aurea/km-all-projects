CREATE proc [dbo].[sp_GetQuestionsWithFilterCount]
(
	@SurveyID int,
	@Filterstr varchar(1000)
)
as
Begin
	create table #tempEmailIDs (EmailID int)

	insert into #tempEmailIDs
	exec sp_SurveyFilterResults @SurveyID, @Filterstr, 0

	Select	QuestionID,
			number,
			format,
			convert(varchar(255),QuestionText) as QuestionText,
			count(distinct edv.emailID) as 'totalcount' 
	from 
			Survey s join 
			question q on s.SurveyID = q.SurveyID left outer join  
			ecn5_communicator..groupdatafields gdf on s.SurveyID = gdf.SurveyID and shortname = convert(varchar,s.SurveyID) + '_' + convert(varchar,q.number) left outer join  
			ecn5_communicator..emaildatavalues edv on gdf.groupdatafieldsID = edv.groupdatafieldsID and 
			edv.emailID in  
				(	
					select emailID from #tempEmailIDs
				)	
	where s.SurveyID = @SurveyID group by QuestionID,number,format,convert(varchar(255),QuestionText) 
	ORDER BY number

	drop table #tempEmailIDs

End
