CREATE PROCEDURE [dbo].[e_LayoutPlans_Select_TokenUID]   
@TokenUID uniqueidentifier
AS
	SELECT * FROM LayoutPlans WITH (NOLOCK) WHERE TokenUID = @TokenUID and Isnull(Status,'Y') = 'Y' and IsDeleted = 0