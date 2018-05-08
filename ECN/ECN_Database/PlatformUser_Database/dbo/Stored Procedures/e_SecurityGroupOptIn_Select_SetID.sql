
CREATE PROCEDURE [dbo].[e_SecurityGroupOptIn_Select_SetID]
	@SetID uniqueidentifier
AS
BEGIN
	SELECT * FROM SecurityGroupOptIn sgoi with(nolock)
	where sgoi.SetID = @SetID and ISNULL(sgoi.IsDeleted,0) = 0
END