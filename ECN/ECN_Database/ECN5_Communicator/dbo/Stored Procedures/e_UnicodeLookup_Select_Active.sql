CREATE PROCEDURE [dbo].[e_UnicodeLookup_Select_Active]
	
AS
	Select * from UnicodeLookup with(nolock)
	where ISNULL(IsEnabled,1) = 1