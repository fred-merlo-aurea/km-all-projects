create PROCEDURE [dbo].[e_FilterPenetrationReports_Save]
(
	@UserID int,
	@ReportName varchar(100),
	@BrandID int
)
	
AS
BEGIN

	SET NOCOUNT ON
	
	if (@brandID = 0)
		begin
			set @brandID = null;
		end
	Begin
		insert into FilterPenetrationReports(ReportName, BrandID, CreatedUserID, CreatedDate, UpdatedUserID, UpdatedDate) values(@ReportName, @BrandID, @UserID, GETDATE(), @UserID, GETDATE());
		select @@IDENTITY;
	End

END