CREATE PROCEDURE [dbo].[v_Blast_GetBlastInfoForAbuse] 
	@BlastID int
AS     
BEGIN     		
	select c.customerName, bc.basechannelname, b.sendtime, b.EmailSubject 
	from
		blast b with (nolock)
		join ecn5_accounts..customer c with (nolock) on b.customerID = c.customerID
        join ecn5_accounts..basechannel bc with (nolock) on bc.basechannelID = c.basechannelID 
    where 
		blastID = @BlastID and 
		b.StatusCode <> 'Deleted' and 
		c.IsDeleted = 0 and 
		bc.IsDeleted = 0
END