CREATE PROCEDURE [dbo].[e_BlastEnvelope_Select_CustomerID]
@CustomerID int
AS
SELECT *
FROM BlastEnvelopes WITH(NOLOCK)
WHERE CustomerID = @CustomerID and IsDeleted = 0
