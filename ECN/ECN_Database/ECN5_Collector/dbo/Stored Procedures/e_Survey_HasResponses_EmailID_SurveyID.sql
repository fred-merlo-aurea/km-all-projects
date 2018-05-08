CREATE PROCEDURE [dbo].[e_Survey_HasResponses_EmailID_SurveyID]
	@EmailID int,
	@SurveyID int
AS
	IF exists (SELECT Top 1 * from ECN5_Communicator..EmailDataValues edv with(nolock) 
								JOIN ECN5_Communicator..GroupDataFields gdf with(nolock) on edv.GroupDataFieldsID = gdf.GroupDataFieldsID
								WHERE gdf.SurveyID = @SurveyID and edv.EmailID = @EmailID)
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
