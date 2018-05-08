create proc [dbo].[sp_getSurveyResponseScore]
(   
    @SurveyID int,
	@emailID int
)
as
BEGIN    

	select avg(score) as score from
		(
			select gdf.surveyID, emailID, q.QuestionID,  convert(decimal(10,2),sum(convert(decimal, score))/convert(decimal,count(emailID))) as score
			from	ecn5_communicator..groupdatafields gdf join 
					ecn5_communicator..emaildatavalues edv on gdf.groupdatafieldsID = edv.groupdatafieldsID join
					ecn5_collector..question q on gdf.longname = q.number and q.SurveyID = gdf.surveyID join
					ecn5_collector..response_options ro on ro.QuestionID = q.QuestionID and edv.datavalue = ro.OptionValue 
			where gdf.surveyID = @SurveyID and longname <> 'completionDt' and score <> 0 and 
				  emailID = @emailID
			group by gdf.surveyID,EmailID, q.QuestionID
		) inn
END
