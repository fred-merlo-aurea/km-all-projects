CREATE proc [dbo].[sp_SaveDomainSuppression]
(
	@DomainSuppressionID int,
	@BaseChannelID int,
	@CustomerID int,
	@Domain varchar(100)
)
as
Begin

	if @DomainSuppressionID = 0
	Begin
		INSERT INTO DomainSuppression (BaseChannelID, CustomerID, Domain) VALUES 
			(case when @BaseChannelID = 0 then null else @BaseChannelID end, case when @CustomerID = 0 then null else @CustomerID end, @Domain)
	End
	Else
	Begin
		UPDATE DomainSuppression 
		SET		BaseChannelID = case when @BaseChannelID = 0 then null else @BaseChannelID end, 
				CustomerID = case when @CustomerID = 0 then null else @CustomerID end, 
				Domain = @Domain
		WHERE	DomainSuppressionID = @DomainSuppressionID 
	End

End
