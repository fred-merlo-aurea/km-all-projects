create PROCEDURE [dbo].[e_BlastEnvelopes_Select_BlastEnvelopeID]   
@BlastEnvelopeID int
AS
	SELECT * FROM BlastEnvelopes WITH (NOLOCK) WHERE BlastEnvelopeID = @BlastEnvelopeID and IsDeleted = 0