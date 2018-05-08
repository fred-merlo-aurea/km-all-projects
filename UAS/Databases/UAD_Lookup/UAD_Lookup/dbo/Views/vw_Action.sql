CREATE view vw_Action
as
	select 
			a.ActionID, 
			a.ActionTypeID , 
			cc.CategoryCodeID, 
			cc.CategoryCodeValue, 
			cc.CategoryCodeName,  
			tc.TransactionCodeID,
			tc.TransactionCodeValue,
			tc.TransactionCodeName,
			tc.IsKill,
			cct.CategoryCodeTypeID,
			cct.CategoryCodeTypeName,
			cct.IsFree as isFreeCategoryCodeType,
			tct.TransactionCodeTypeName,
			tct.isFree as isFreeTransactionCodeType
	from 
			Action a 
			join TransactionCode tc on a.transactionCodeID = tc.TransactionCodeID 
			join CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
			join TransactionCodeType tct on tct.TransactionCodeTypeID = tc.TransactionCodeTypeID
			join CategoryCodeType cct on cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
	where
		 a.IsActive = 1 and tc.IsActive = 1 and cc.IsActive = 1 and tct.IsActive = 1 and cct.IsActive = 1 
 

GO
