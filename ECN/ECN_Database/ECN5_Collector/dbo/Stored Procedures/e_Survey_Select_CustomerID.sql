-- =============================================
-- Author:		Sunil
-- Create date: 12/31/2012
-- Description:	Get Survey list by CustomerID
-- =============================================
CREATE PROCEDURE dbo.e_Survey_Select_CustomerID
	-- Add the parameters for the stored procedure here
	@CustomerID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	SELECT s.*, 
			(select COUNT(DISTINCT(EmailID)) FROM ecn5_communicator.dbo.EmailDataValues WHERE GroupDatafieldsID = (select groupdatafieldsID from ecn5_communicator.dbo.groupdatafields gdf where shortname=Convert(varchar,s.surveyID)+ '_completionDt' and gdf.surveyID = s.surveyID and gdf.groupID = s.groupID) AND DataValue <> '') as responsecount,  
			'surveyID=' + convert(varchar,s.SurveyID) + '&chID=' + convert(varchar(10),c.basechannelID) + '&cuID=' + convert(varchar(10),c.customerID)  as SurveyURL 
	FROM 
			survey s join 
			ecn5_accounts.dbo.customer c on s.customerID = c.customerID 
	WHERE s.CustomerID = @CustomerID and isnull(s.IsDeleted,0) = 0
END