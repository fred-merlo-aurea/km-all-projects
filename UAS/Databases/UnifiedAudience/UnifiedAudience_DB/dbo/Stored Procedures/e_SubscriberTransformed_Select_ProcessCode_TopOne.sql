CREATE PROCEDURE [dbo].[e_SubscriberTransformed_Select_ProcessCode_TopOne]
	@ProcessCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON;

	select top 1 * 
	from SubscriberTransformed with(nolock) 
	where ProcessCode = @ProcessCode

END
GO