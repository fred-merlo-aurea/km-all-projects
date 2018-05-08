Create PROCEDURE [dbo].[e_Suffix_Select_SuffixCodeTypeID]
@SuffixCodeTypeID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM Suffix With(NoLock)
	WHERE SuffixCodeTypeID = @SuffixCodeTypeID

END