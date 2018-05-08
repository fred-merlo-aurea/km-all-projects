create procedure e_Subscription_SaveBulkXml
@xml xml
as
BEGIN
	set nocount on  	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[subscription_id] [int] NOT NULL,
		[account_id] [int] NOT NULL,
		[publication_id] [int] NULL,
		[mailing_address_id] [int] NULL,
		[billing_address_id] [int] NULL,
		[issues] [int] NULL,
		[copies] [int] NULL,
		[paid_issues_left] [int] NULL,
		[unearned_revenue] [float] NULL,
		[type] [varchar](50) NULL,
		[date_created] [datetime] NULL,
		[date_expired] [datetime] NULL,
		[date_last_renewed] [datetime] NULL,
		[last_purchase_bundle_id] [int] NULL,
		[renew_campaign_active] [bit] NULL,
		[audit_classification] [varchar](50) NULL,
		[audit_request_type] [varchar](50) NULL
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		subscription_id,account_id,publication_id,mailing_address_id,billing_address_id,issues,copies,paid_issues_left,unearned_revenue,
		type,date_created,date_expired,date_last_renewed,last_purchase_bundle_id,renew_campaign_active,audit_classification,audit_request_type
	)  
	
	select subscription_id,account_id,publication_id,mailing_address_id,billing_address_id,issues,copies,paid_issues_left,unearned_revenue,
			type,date_created,date_expired,date_last_renewed,last_purchase_bundle_id,renew_campaign_active,audit_classification,audit_request_type
	from openxml(@docHandle,N'/XML/Subscription')
	with
	(
		subscription_id int 'subscription_id',
		account_id int 'account_id',
		publication_id int 'publication_id',
		mailing_address_id int 'mailing_address_id',
		billing_address_id int 'billing_address_id',
		issues int 'issues',
		copies int 'copies',
		paid_issues_left int 'paid_issues_left',
		unearned_revenue float 'unearned_revenue',
		type varchar(50) 'type',
		date_created datetime 'date_created',
		date_expired datetime 'date_expired',
		date_last_renewed datetime 'date_last_renewed',
		last_purchase_bundle_id int 'last_purchase_bundle_id',
		renew_campaign_active bit 'renew_campaign_active',
		audit_classification varchar(50) 'audit_classification',
		audit_request_type varchar(50) 'audit_request_type'
	)
	
	exec sp_xml_removedocument @docHandle

	update @import
	set type = replace(type, '&amp;', '&')

	update @import
	set type = replace(type, '&quot;', '"' )

	update @import
	set type = replace(type, '&lt;', '<')

	update @import
	set type = replace(type, '&gt;', '>')

	update @import
	set type = replace(type, '&apos;', '''')

	update @import
	set audit_classification = replace(audit_classification, '&amp;', '&')

	update @import
	set audit_classification = replace(audit_classification, '&quot;', '"' )

	update @import
	set audit_classification = replace(audit_classification, '&lt;', '<')

	update @import
	set audit_classification = replace(audit_classification, '&gt;', '>')

	update @import
	set audit_classification = replace(audit_classification, '&apos;', '''')

	update @import
	set audit_request_type = replace(audit_request_type, '&amp;', '&')

	update @import
	set audit_request_type = replace(audit_request_type, '&quot;', '"' )

	update @import
	set audit_request_type = replace(audit_request_type, '&lt;', '<')

	update @import
	set audit_request_type = replace(audit_request_type, '&gt;', '>')

	update @import
	set audit_request_type = replace(audit_request_type, '&apos;', '''')

	insert into Subscription(subscription_id,account_id,publication_id,mailing_address_id,billing_address_id,issues,copies,paid_issues_left,unearned_revenue,
						type,date_created,date_expired,date_last_renewed,last_purchase_bundle_id,renew_campaign_active,audit_classification,audit_request_type)
	select i.subscription_id,i.account_id,i.publication_id,i.mailing_address_id,i.billing_address_id,i.issues,i.copies,i.paid_issues_left,i.unearned_revenue,
		i.type,i.date_created,i.date_expired,i.date_last_renewed,i.last_purchase_bundle_id,i.renew_campaign_active,i.audit_classification,i.audit_request_type
	from @import i
	left join Subscription x on i.subscription_id = x.subscription_id
	where x.subscription_id is null

	update x
	set x.account_id = i.account_id,
		x.publication_id = i.publication_id,
		x.mailing_address_id = i.mailing_address_id,
		x.billing_address_id = i.billing_address_id,
		x.issues = i.issues,
		x.copies = i.copies,
		x.paid_issues_left = i.paid_issues_left,
		x.unearned_revenue = i.unearned_revenue,
		x.type = i.type,
		x.date_created = i.date_created,
		x.date_expired = i.date_expired,
		x.date_last_renewed = i.date_last_renewed,
		x.last_purchase_bundle_id = i.last_purchase_bundle_id,
		x.renew_campaign_active = i.renew_campaign_active,
		x.audit_classification = i.audit_classification,
		x.audit_request_type = i.audit_request_type
	from Subscription x
	join @import i on i.subscription_id = x.subscription_id

END
GO