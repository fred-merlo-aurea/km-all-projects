CREATE PROCEDURE [dbo].[e_CrossTabReport_Exists_ByCrossTabReportName]
@CrossTabReportID int,
@CrossTabReportName varchar(50)
AS
BEGIN     		
	IF EXISTS  (
				Select Top 1 CrossTabReportID 
				from CrossTabReport with(nolock)
				where IsDeleted = 0 and 
					CrossTabReportID <> @CrossTabReportID and 
					CrossTabReportName = @CrossTabReportName
				) SELECT 1 ELSE SELECT 0

END