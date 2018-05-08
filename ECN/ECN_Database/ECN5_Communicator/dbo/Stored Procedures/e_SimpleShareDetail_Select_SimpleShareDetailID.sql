CREATE PROCEDURE [dbo].[e_SimpleShareDetail_Select_SimpleShareDetailID]
	@SimpleShareDetailID int
AS
	SELECT * FROM SimpleShareDetail ssd with(nolock)
	WHERE ssd.SimpleShareDetailID = @SimpleShareDetailID
