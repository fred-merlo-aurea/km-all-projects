CREATE PROCEDURE [dbo].[e_PersonalizedContent_MarkAsFailed]
	@PersonalizedContentID bigint
AS
	Update PersonalizedContent
	set Failed = 1
	where PersonalizedContentID = @PersonalizedContentID
