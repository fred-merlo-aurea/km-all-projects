CREATE PROCEDURE dbo.e_LandingPageAssign_Select_customerID_LPID 
@CustomerID int,
@LPID int
AS
	SELECT *
	FROM LandingPageAssign lpa WITH (NOLOCK)
	join LandingPage lp
	on lpa.LPID= lp.LPID
	WHERE CustomerID = @CustomerID and IsNull(CustomerDoesOverride, 0) = 1 and IsDeleted = 0
	and lp.LPID=@LPID