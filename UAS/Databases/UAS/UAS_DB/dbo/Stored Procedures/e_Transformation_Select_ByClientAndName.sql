CREATE PROCEDURE [dbo].[e_Transformation_Select_ByClientAndName]
@TransformationName varchar(100),
@ClientID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM Transformation With(NoLock)
	WHERE TransformationName = @TransformationName
	and ClientID = @ClientID and IsActive = 'true'

END