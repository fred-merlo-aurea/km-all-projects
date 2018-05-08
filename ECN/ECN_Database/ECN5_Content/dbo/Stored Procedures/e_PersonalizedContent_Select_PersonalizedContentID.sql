CREATE PROCEDURE e_PersonalizedContent_Select_PersonalizedContentID
	-- Add the parameters for the stored procedure here
	@PersonalizedContentID Bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from PersonalizedContent with (NOLOCK)
	where PersonalizedContentID = @PersonalizedContentID
END