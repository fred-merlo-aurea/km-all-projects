CREATE FUNCTION [dbo].[getScoreForSurvey](@SurveyID int,  @startDate varchar(10), @endDate varchar(10))     
RETURNS  decimal(10,2) 
AS         
    
BEGIN         
	declare @score decimal(10,2),
			@groupID int,
			@groupdatefieldsID int

	select @groupID = group_ID from ecn5_collector..survey where survey_ID = @SurveyID
	select @groupdatefieldsID = groupdatafieldsID from	ecn5_communicator..groupdatafields gdf 
	where	shortname=Convert(varchar,@SurveyID)+ '_completionDt' and gdf.surveyID = @SurveyID and gdf.groupID = @GroupID
	
	select  @score = avg(score) from
	(
		select surveyID, emailID, avg(score) as score from
			(
				select gdf.surveyID, emailID, q.question_id,  convert(decimal(10,2),sum(convert(decimal, score))/convert(decimal,count(emailID))) as score
				from	ecn5_communicator..groupdatafields gdf join 
						ecn5_communicator..emaildatavalues edv on gdf.groupdatafieldsID = edv.groupdatafieldsID join
						ecn5_collector..question q on gdf.longname = q.number and q.survey_id = gdf.surveyID join
						ecn5_collector..response_options ro on ro.question_id = q.question_id and edv.datavalue = ro.option_value 
				where gdf.surveyID = @SurveyID and longname <> 'completionDt' and score <> 0 and
					  emailID in 
							(
								select	DISTINCT(EmailID) 
								FROM ecn5_communicator..EmailDataValues 
								WHERE	GroupDatafieldsID = @groupdatefieldsID and
										isnull(DataValue,'') <> '' and 
										convert(datetime, convert(varchar(10), convert(datetime, datavalue),101))  between @startDate and @endDate 
							)
					
				group by gdf.surveyID,EmailID, q.question_id
			) inn
		group by surveyID, emailID
	) inn1
	group by surveyID
		
	return @score
END