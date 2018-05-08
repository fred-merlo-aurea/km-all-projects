create procedure e_SubscriptionPaid_ImportFromSubGen
@ProcessCode varchar(50)
as
BEGIN
	
	SET NOCOUNT ON
	
	----need a process to insert from SubGenData.Bundle to UAD.PriceCode for now just use -1 for PriceCodeId
	insert into SubscriptionPaid (PubSubscriptionID,PriceCodeID,StartIssueDate,ExpireIssueDate,CPRate,Amount,AmountPaid,BalanceDue,PaidDate,TotalIssues,
		CheckNumber,CCNumber,CCExpirationMonth,CCExpirationYear,CCHolderName,CreditCardTypeID,PaymentTypeID,DeliverID,GraceIssues,WriteOffAmount,OtherType,
		DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,Frequency,Term)

	select ps.PubSubscriptionID,-1,'1/1/1900','1/1/1900',0,b.price,p.amount,b.price - p.amount,p.date_created,b.issues,
		null,null,null,null,null,null,
		(select CodeId from uad_lookup..Code where CodeTypeId=16 and CodeName like p.type + '%'),
		(select CodeId from uad_lookup..Code where CodeTypeId=46 and CodeName = b.type),
		null,null,null,getdate(),null,1,null,null,null
	from [10.10.41.198].SubGenData.dbo.Payment p
		join [10.10.41.198].SubGenData.dbo.Bundle b on p.bundle_id = b.bundle_id
		join SubscriberFinal sf on p.STRecordIdentifier = sf.STRecordIdentifier
		join PubSubscriptions ps on sf.SFRecordIdentifier = ps.SFRecordIdentifier
	where sf.ProcessCode = @ProcessCode

END
go