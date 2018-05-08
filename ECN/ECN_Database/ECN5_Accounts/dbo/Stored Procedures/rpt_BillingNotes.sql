-- Procedure
CREATE procedure [dbo].[rpt_BillingNotes]
as

Begin
	Set nocount on

	select	b.basechannelID, b.basechannelName, c.customerID, c.customerName, cn.notes, cn.UpdatedBy, cn.UpdatedDate
	from	[CustomerNote] cn join 
			[Customer] c on cn.customerID = c.customerID join
			[BaseChannel] b on b.basechannelID = c.basechannelID 
	where isbillingnotes = 1
	order by c.customerName
End
