CREATE proc [dbo].[spClean_ActivityTables] 
as
Begin
	print 'commented'
	--DELETE ecn_activity.dbo.BlastActivityBounces
	--DELETE ecn_activity.dbo.BlastActivityClicks
	--DELETE ecn_activity.dbo.BlastActivityConversion
	--DELETE ecn_activity.dbo.BlastActivityOpens
	--DELETE ecn_activity.dbo.BlastActivityRefer
	--DELETE ecn_activity.dbo.BlastActivityResends
	--DELETE ecn_activity.dbo.BlastActivitySends
	--DELETE ecn_activity.dbo.BlastActivitySuppressed
	--DELETE ecn_activity.dbo.BlastActivityUnSubscribes
	--DELETE ecn_activity.dbo.BlastSummary


	----SELECT * FROM dbo.BlastActivityBounces
	----SELECT * FROM dbo.BlastActivityClicks
	----SELECT * FROM dbo.BlastActivityConversion
	----SELECT * FROM dbo.BlastActivityOpens
	----SELECT * FROM dbo.BlastActivityRefer
	----SELECT * FROM dbo.BlastActivityResends
	----SELECT * FROM dbo.BlastActivitySends
	----SELECT * FROM dbo.BlastActivitySuppressed
	----SELECT * FROM dbo.BlastActivityUnSubscribes
	----SELECT * FROM dbo.BlastSummary
	
	
----DBCC CHECKIDENT (BlastActivityBounces, reseed, -2147482455)
----DBCC CHECKIDENT (BlastActivityClicks, reseed, -2147482455)
----DBCC CHECKIDENT (BlastActivityConversion, reseed, -2147482455)
----DBCC CHECKIDENT (BlastActivityOpens, reseed, -2147482455)
----DBCC CHECKIDENT (BlastActivityRefer, reseed, -2147482455)
----DBCC CHECKIDENT (BlastActivityResends, reseed, -2147482455)
----DBCC CHECKIDENT (BlastActivitySends, reseed, -2147482455)
----DBCC CHECKIDENT (BlastActivitySuppressed, reseed, -2147482455)
----DBCC CHECKIDENT (BlastActivityUnSubscribes, reseed, -2147482455)


End
