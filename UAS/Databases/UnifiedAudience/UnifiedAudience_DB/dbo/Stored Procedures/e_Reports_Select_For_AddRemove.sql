CREATE PROCEDURE [dbo].[e_Reports_Select_For_AddRemove]
@PubID int
AS	
	DECLARE @TypeID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Report')
	SELECT *
	FROM Reports r With(NoLock)	
	JOIN UAD_Lookup..Code c ON c.CodeId = r.ReportTypeID
	WHERE C.CodeTypeId = 26
	AND C.CodeName in ('Cross Tab', 'Qualification BreakDown', 'Geo Domestic BreakDown', 'SubSourceSummary')
	AND r.ProductID = @PubID