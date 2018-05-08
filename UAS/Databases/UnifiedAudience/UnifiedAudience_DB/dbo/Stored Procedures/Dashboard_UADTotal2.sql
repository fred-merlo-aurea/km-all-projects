CREATE PROCEDURE [dbo].[Dashboard_UADTotal2]
@brandID int = 0
AS
Begin

	select [type], counts
	from	summary_data with (NOLOCK)
	where 
		entityName = 'Breakdown' and isnull(brandID ,0) = @brandID

End
GO