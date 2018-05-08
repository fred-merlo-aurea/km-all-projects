CREATE PROCEDURE [dbo].[e_Email_Exists_BaseChannelID]
	@EmailAddress varchar(255),
	@BaseChannelID int
AS
	if exists (select top 1 EmailID 
				from Emails e with(nolock) 
				where e.EmailAddress = @EmailAddress and 
				e.CustomerID in (select CustomerID 
								from ECN5_Accounts..Customer c with(nolock) 
								where c.BaseChannelID = @BaseChannelID)
			)
		select 1
	else
		select 0