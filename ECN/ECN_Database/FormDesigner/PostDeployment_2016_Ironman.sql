-- PostDeployment_2016_Ironman

-- FieldName and FieldLabel population
/* Common */
update Control
set FieldLabelHTML = FieldLabel
 where 
 (Type_Seq_ID = 101 or
 Type_Seq_ID = 102 or
 Type_Seq_ID = 103 or
 Type_Seq_ID = 104 or
 Type_Seq_ID = 105 or
 Type_Seq_ID = 106 or
 Type_Seq_ID = 107 or
 Type_Seq_ID = 108 or 
 Type_Seq_ID = 109 or
 Type_Seq_ID = 110 or
 Type_Seq_ID = 111 or
 Type_Seq_ID = 112)
 
update c
set c.FieldLabel = ct.Name
from Control c with(nolock)
inner join ControlType ct with(nolock)
on c.Type_Seq_ID = ct.ControlType_Seq_ID
 where
 (Type_Seq_ID = 101 or
 Type_Seq_ID = 102 or
 Type_Seq_ID = 103 or
 Type_Seq_ID = 104 or
 Type_Seq_ID = 105 or
 Type_Seq_ID = 106 or
 Type_Seq_ID = 107 or
 Type_Seq_ID = 108 or 
 Type_Seq_ID = 109 or
 Type_Seq_ID = 110 or
 Type_Seq_ID = 111 or
 Type_Seq_ID = 112)

 /* Uncommon */
update Control
set FieldLabelHTML = FieldLabel
 where
 (Type_Seq_ID = 201 or
 Type_Seq_ID = 202 or
 Type_Seq_ID = 203 or
 Type_Seq_ID = 204 or
 Type_Seq_ID = 205 or
 Type_Seq_ID = 206 or
 Type_Seq_ID = 207 or
 Type_Seq_ID = 208 or 
 Type_Seq_ID = 209 or
 Type_Seq_ID = 210 or
 Type_Seq_ID = 211 or
 Type_Seq_ID = 212 or
 Type_Seq_ID = 213 or
 Type_Seq_ID = 214 or
 Type_Seq_ID = 215 or
 Type_Seq_ID = 216 or
 Type_Seq_ID = 217)

update c
set c.FieldLabel = ct.Name
from Control c with(nolock)
inner join ControlType ct with(nolock)
on c.Type_Seq_ID = ct.ControlType_Seq_ID
 where
 (Type_Seq_ID = 201 or
 Type_Seq_ID = 202 or
 Type_Seq_ID = 203 or
 Type_Seq_ID = 204 or
 Type_Seq_ID = 205 or
 Type_Seq_ID = 206 or
 Type_Seq_ID = 207 or
 Type_Seq_ID = 208 or 
 Type_Seq_ID = 209 or
 Type_Seq_ID = 210 or
 Type_Seq_ID = 211 or
 Type_Seq_ID = 212 or
 Type_Seq_ID = 213 or
 Type_Seq_ID = 214 or
 Type_Seq_ID = 215 or
 Type_Seq_ID = 216 or
 Type_Seq_ID = 217)

/* Custom */
update Control
set FieldLabelHTML = FieldLabel
 where
 (Type_Seq_ID = 1 or
 Type_Seq_ID = 2 or
 Type_Seq_ID = 3 or
 Type_Seq_ID = 4 or
 Type_Seq_ID = 5 or
 Type_Seq_ID = 6 or
 Type_Seq_ID = 7 or
 Type_Seq_ID = 10)

update c
set c.FieldLabel = a.ShortName
from Control c with(nolock)
inner join [ECN5_COMMUNICATOR].[dbo].[GroupDatafields] a with(nolock)
on c.FieldID = a.GroupDatafieldsID
 where
 (c.Type_Seq_ID = 1 or
 c.Type_Seq_ID = 2 or
 c.Type_Seq_ID = 3 or
 c.Type_Seq_ID = 4 or
 c.Type_Seq_ID = 5 or
 c.Type_Seq_ID = 6 or
 c.Type_Seq_ID = 7 or
 c.Type_Seq_ID = 9)

update c
set c.FieldLabel = g.GroupName
from Control c with(nolock)
inner join FormControlProperty fcp with(nolock)
on c.Control_ID = fcp.Control_ID and fcp.ControlProperty_ID = 1020
inner join [ECN5_COMMUNICATOR].[dbo].[Groups] g with(nolock)
on g.GroupID = fcp.Value
 where
 (c.Type_Seq_ID = 10)

-- Adding 'Colums' property to Newsletter Control

INSERT INTO ControlProperty (Type_ID,PropertyName,PropertyType)
VALUES (10,'Number of Columns',1);

-- Main NL Controls migration queries	
INSERT INTO NewsletterGroup(Control_ID, CustomerID, GroupID, [Order], IsPreSelected, LabelHTML)
	select c.Control_ID, Value, 0, 0, 0, 'Subscribe'
	from FormControlProperty fcp with(nolock)
	join ControlProperty cp with(nolock) on fcp.ControlProperty_ID = cp.ControlProperty_Seq_ID
	join Control c with(nolock) on fcp.Control_ID = c.Control_ID
	where cp.Type_ID = 10 and cp.PropertyName = 'customerid'
		
update NewsletterGroup
	set GroupID = Value				
	from FormControlProperty fcp with(nolock)
	join ControlProperty cp with(nolock) on fcp.ControlProperty_ID = cp.ControlProperty_Seq_ID
	join Control c with(nolock) on fcp.Control_ID = c.Control_ID
	join NewsletterGroup ng with(Nolock) on c.Control_ID = ng.Control_ID
	where cp.Type_ID = 10 and cp.PropertyName = 'GroupID'
	
-- Updating RULES related to NL controls
update co
	set Value = ng.GroupID
	from NewsletterGroup ng with(Nolock)
	join Control c with(nolock) on ng.Control_ID = c.Control_ID
	join Condition co with(nolock) on co.Control_ID = c.Control_ID
	where c.Type_Seq_ID = 10

