CREATE PROCEDURE [dbo].[Dashboard_UADGrossTotal]
@brandID int = 0
AS
Begin

	select [type], counts
	from	summary_data with (NOLOCK)
	where 
		entityName = 'Gross' and isnull(brandID ,0) = @brandID

End
go