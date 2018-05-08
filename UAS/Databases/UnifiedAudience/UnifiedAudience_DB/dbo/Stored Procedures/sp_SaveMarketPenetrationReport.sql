CREATE PROCEDURE dbo.sp_SaveMarketPenetrationReport
(
@user int,
@MarketID xml,
@report_name varchar(100),
@BrandID int
)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	if (@brandID = 0)
		begin
			set @brandID = null;
		end
	
	declare @id int;

	if not exists (select top 1 * from 	PenetrationReports where ReportName = @report_name)
		Begin
			insert into PenetrationReports(ReportName, BrandID, CreatedBy, CreateddtStamp, UpdatedBy, UpdateddtStamp) 
			values(@report_name, @BrandID, @user, CURRENT_TIMESTAMP, @user, CURRENT_TIMESTAMP);
			select @id=@@IDENTITY;
			insert into PenetrationReports_Markets 
			SELECT @id, MarketValues.ID.value('./@MarketID','INT')
			FROM @MarketID.nodes('/Markets') as MarketValues(ID) ;
		End
	Else
		Begin
			RAISERROR ('Duplicate Report Name. Please enter a different name.', -- Message text.
				   16, -- Severity.
				   1 -- State.
				   );

		End	
	
END