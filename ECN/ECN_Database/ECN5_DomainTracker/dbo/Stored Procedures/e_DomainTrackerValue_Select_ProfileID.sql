CREATE PROCEDURE dbo.e_DomainTrackerValue_Select_ProfileID

@ProfileID INT,
@DomainTrackerID INT

AS

SET NOCOUNT ON

--DECLARE @Loc_ProfileID INT
--DECLARE @Loc_DomainTrackerID INT

--SET @Loc_ProfileID = @ProfileID
--SET @Loc_DomainTrackerID = @DomainTrackerID

SELECT DISTINCT 
	dtf.FieldName, 
	dtv.Value, 
	dta.PageURL 
FROM 
	DomainTrackerUserProfile dtup WITH (NOLOCK)
	INNER JOIN DomainTrackerActivity dta  WITH (NOLOCK) ON dtup.ProfileID= dta.ProfileID
	INNER JOIN DomainTrackerValue dtv  WITH (NOLOCK) ON dtv.DomainTrackerActivityID= dta.DomainTrackerActivityID
	INNER JOIN DomainTrackerFields dtf  WITH (NOLOCK) ON dtv.DomainTrackerFieldsID= dtf.DomainTrackerFieldsID
WHERE 
	dta.DomainTrackerID = @DomainTrackerID 
	AND dtup.ProfileID = @ProfileID
	AND dtup.IsDeleted =0  
	AND dtv.IsDeleted = 0 
	AND dtf.IsDeleted = 0 
	AND dtup.IsDeleted =0
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerValue_Select_ProfileID] TO [ecn5]
    AS [dbo];

