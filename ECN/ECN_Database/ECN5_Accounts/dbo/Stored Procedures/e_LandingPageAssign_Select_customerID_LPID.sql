CREATE PROCEDURE dbo.e_LandingPageAssign_Select_customerID_LPID 
@CustomerID int,
@LPID int
AS
	SELECT *
	FROM LandingPageAssign lpa WITH (NOLOCK)
	join LandingPage lp
	on lpa.LPID= lp.LPID
	WHERE CustomerID = @CustomerID and lp.LPID=@LPID