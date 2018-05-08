CREATE PROC [dbo].[spDeleteGroup] 
(
	@GroupID INT        
)
AS
BEGIN        
	SET NOCOUNT ON 

	DECLARE 
		@referenceEXISTS BIT,
		@existsInTable VARCHAR(50)
		    
	SET @referenceEXISTS = 0
	SET @existsInTable = ''

	IF EXISTS (SELECT TOP 1 GroupID FROM ecn5_publisher..[PUBLICATION] WHERE GroupID = @GroupID)
	BEGIN
		SET @referenceEXISTS = 1
		SET @existsInTable = 'Digital Editions'
	END

	IF @referenceEXISTS = 0 AND EXISTS (SELECT TOP 1 GroupID FROM ecn5_collector..survey WHERE GroupID = @GroupID)
	BEGIN
		SET @referenceEXISTS = 1
		SET @existsInTable = 'Surveys'
	END

	IF @referenceEXISTS = 0 AND EXISTS (SELECT TOP 1 ECNDefaultGroupID FROM KMPSJointForms..Publications WHERE ECNDefaultGroupID = @GroupID)
	BEGIN
		SET @referenceEXISTS = 1
		SET @existsInTable = 'Publications'
	END

	IF @referenceEXISTS = 0 AND EXISTS (SELECT TOP 1 ECNGroupID FROM KMPSJointForms..PubNewsletters WHERE ECNGroupID = @GroupID)
	BEGIN
		SET @referenceEXISTS = 1
		SET @existsInTable = 'Pub Newsletters'
	END

	IF @referenceEXISTS = 0 AND EXISTS (SELECT TOP 1 GroupID FROM ecn5_communicator..[BLAST] WHERE GroupID = @GroupID and StatusCode <> 'deleted')
	BEGIN
		SET @referenceEXISTS = 1
		SET @existsInTable = 'Blast'
	END
	
	IF @referenceEXISTS = 0 AND EXISTS (SELECT TOP 1 GroupID FROM ecn5_communicator..Groups WHERE GroupID = @GroupID and MasterSupression = 1)
	BEGIN
		SET @referenceEXISTS = 1
		SET @existsInTable = 'Master Suppression'
	END
		
		
	IF @referenceEXISTS = 0
	BEGIN
		BEGIN TRANSACTION	
		BEGIN TRY	
			DELETE FROM ecn5_communicator..EmailDatavalues WHERE GroupDataFieldsID IN (SELECT GroupDataFieldsID FROM ecn5_communicator..GroupDataFields WHERE GroupID = @GroupID)
			DELETE FROM ecn5_communicator..GroupDataFields WHERE GroupID = @GroupID
			DELETE FROM ecn5_communicator..DatafieldSets WHERE GroupID = @GroupID
			DELETE FROM ecn5_communicator..EmailGroups WHERE GroupID = @GroupID
			DELETE FROM ecn5_communicator..[FILTER] WHERE GroupID = @GroupID
			DELETE FROM ecn5_communicator..DeptItemReferences WHERE Item = 'GRP' AND ItemID = @GroupID
			DELETE FROM ecn5_communicator..Groups WHERE GroupID = @GroupID
			--IF @@error <> 0 
			--BEGIN 
			--	ROLLBACK TRANSACTION 
			--	RETURN 'ERROR DELETING Groups. Rolling back transaction.' 
			--END
			
			COMMIT TRANSACTION	
		END TRY
		BEGIN CATCH 
			ROLLBACK TRANSACTION
			
			RAISERROR('ERROR DELETING', 13, 1)
		END CATCH
	END
	ELSE
	BEGIN
		SET @existsInTable = 'CANNOT DELETE. This group is used in ' + @existsInTable
		RAISERROR(@existsInTable, 13, 1)
	END

END
