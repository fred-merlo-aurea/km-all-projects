CREATE PROCEDURE [dbo].[e_DomainTrackerFields_Exists_Name_DomainTrackerID]
	@FieldName varchar(255),
	@DomainTrackerID int
AS
	if exists (select top 1 * from DomainTrackerFields dtf with(nolock) where dtf.DomainTrackerID = @DomainTrackerID and dtf.FieldName = @FieldName and ISNULL(dtf.IsDeleted, 0) = 0)
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		Select 0
	END
