CREATE PROCEDURE [dbo].[e_BlastPlans_Select_BlastID]   
@BlastID int,
@EventType varchar(50)
AS
	SELECT * FROM BlastPlans WITH (NOLOCK) WHERE BlastID = @BlastID and EventType = @EventType and IsDeleted = 0