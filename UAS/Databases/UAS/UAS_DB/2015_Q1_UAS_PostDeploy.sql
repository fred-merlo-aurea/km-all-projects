update uas..Client
set IsActive = 0
where ClientID = 36

update uas..Client set DisplayName = ClientName
update uas..Client set DisplayName = 'DeWitt Publishing - 10 Missions' where ClientID = 1
update uas..Client set DisplayName = 'Athletic Business' where ClientID = 5
update uas..Client set DisplayName = 'Brief Media' where ClientID = 6
update uas..Client set DisplayName = 'Speciality Foods' where ClientID = 15
update uas..Client set DisplayName = 'UBM Advanstar' where ClientID = 2
update uas..Client set DisplayName = 'UBM Canon' where ClientID = 7
update uas..Client set DisplayName = 'Emerald Expositions - GLM' where ClientID = 9
update uas..Client set DisplayName = 'UBM Advanstar VCast' where ClientID = 18
update uas..Client set DisplayName = 'Hawaii Visitor Convention Bureau' where ClientID = 28
update uas..Client set DisplayName = 'UBM Catersource' where ClientID = 35
update uas..Client set DisplayName = '10 Missions (DeWitt)' where ClientID = 1
update uas..Client set DisplayName = 'AHA - Health Forum' where ClientID = 29
update uas..Client set DisplayName = '10 Missions' where ClientID = 1
update uas..Client set DisplayName = 'AHA - Coding' where ClientID = 32
update uas..Client set DisplayName = 'Anthem Media Group' where ClientID = 3
update uas..Client set DisplayName = 'Atlantic Communications' where ClientID = 4
update uas..Client set DisplayName = 'Business Journals, Inc.' where ClientID = 33
update uas..Client set DisplayName = 'France Media' where ClientID = 8
update uas..Client set DisplayName = 'Meister Media Worldwide' where ClientID = 12
update uas..Client set DisplayName = 'Marketing Technology Group' where ClientID = 24
update uas..Client set DisplayName = 'Northstar Publishing' where ClientID = 13
update uas..Client set DisplayName = 'United Publications Inc' where ClientID = 17
update uas..Client set DisplayName = 'Watt Publishing' where ClientID = 19
update uas..Transformation set TransformationTypeID = 69 where TransformationTypeID = 1
update uas..Transformation set TransformationTypeID = 70 where TransformationTypeID = 2
update uas..Transformation set TransformationTypeID = 71 where TransformationTypeID = 3
update uas..Transformation set TransformationTypeID = 72 where TransformationTypeID = 4
update uas..Transformation set TransformationTypeID = 73 where TransformationTypeID = 5
update uas..FieldMapping set FieldMappingTypeID = 37 where FieldMappingTypeID = 3
update uas..FieldMapping set FieldMappingTypeID = 35 where FieldMappingTypeID = 1
update uas..FieldMapping set FieldMappingTypeID = 36 where FieldMappingTypeID = 2
update uas..SourceFile set FileRecurrenceTypeId = 39 where FileRecurrenceTypeId in (-1,0)
update uas..SourceFile set DatabaseFileTypeId = 131 where DatabaseFileTypeId in (-1,0)

-- Client Group Updates
update uas..ClientGroup
set ClientGroupName = 'Marketing Technology Group'
Where ClientGroupName = 'MTG'

update uas..ClientGroup
set ClientGroupName = 'France Media'
Where ClientGroupName = 'France'