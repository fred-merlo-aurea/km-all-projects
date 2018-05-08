CREATE PROCEDURE [dbo].[e_FilterScheduleIntegration_Save]
@FilterScheduleID int,
@IntegrationParamName varchar(50),
@IntegrationParamValue varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	insert into FilterScheduleIntegration (FilterScheduleID, IntegrationParamName, IntegrationParamValue) 
	values (@FilterScheduleID, @IntegrationParamName, @IntegrationParamValue)
	Select @@IDENTITY;

END