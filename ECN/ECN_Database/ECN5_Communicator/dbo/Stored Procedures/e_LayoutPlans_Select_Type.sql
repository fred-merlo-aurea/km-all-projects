CREATE PROCEDURE [dbo].[e_LayoutPlans_Select_Type]   
@LayoutID int,
@CustomerID int,
@EventType varchar(10)
AS
	SELECT * FROM LayoutPlans WITH (NOLOCK) WHERE CustomerID = @CustomerID and LayoutID = @LayoutID and EventType = @EventType and Isnull(Status,'Y') = 'Y' and IsDeleted = 0