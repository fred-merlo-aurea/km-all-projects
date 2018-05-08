CREATE PROCEDURE e_PersonalizedContent_Select_Top100_NonProcessed
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Top 100 pc.* 
	from 
			PersonalizedContent pc with (NOLOCK)join 
			ecn5_communicator..Blast b with (NOLOCK) on pc.blastID = b.blastID
	where
			IsProcessed = 0 and Isvalid =1 and isDeleted = 0 and ISNULL(Failed,0) = 0
	order by
			b.sendtime, pc.blastID, pc.personalizedContentID 

END