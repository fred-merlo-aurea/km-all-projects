CREATE procedure [dbo].[o_Product_Select]
AS
BEGIN

SET NOCOUNT ON;

	Select PubID as ProductID,PubName as ProductName,PubCode as ProductCode,ClientID,IsActive,IsUAD,IsCirc,
	isnull(AllowDataEntry,'true') as 'AllowDataEntry', 
	isnull(UseSubGen,'false') as 'UseSubGen',
	isnull(KMImportAllowed, 'true') as 'KMImportAllowed',
    isnull(ClientImportAllowed, 'true') as 'ClientImportAllowed',
    isnull (AddRemoveAllowed, 'true') as 'AddRemoveAllowed'
	from Pubs with(nolock)

END