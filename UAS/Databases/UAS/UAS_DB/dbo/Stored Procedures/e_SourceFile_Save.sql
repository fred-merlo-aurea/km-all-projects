CREATE PROCEDURE [dbo].[e_SourceFile_Save]
@SourceFileID int,
@FileRecurrenceTypeId int,
@DatabaseFileTypeId int,
@FileName varchar(100),
@ClientID int,
@PublicationID int,
@IsDeleted bit,
@IsIgnored bit,
@FileSnippetID int,
@Extension varchar(10),
@IsDQMReady bit = 'true',
@Delimiter varchar(10),
@IsTextQualifier bit,
@ServiceID int,
@ServiceFeatureID int,
@MasterGroupID int,
@UseRealTimeGeocoding bit = 'false',
@IsSpecialFile bit,
@ClientCustomProcedureID int,
@SpecialFileResultID int,
@DateCreated datetime,
@DateUpdated datetime = null,
@CreatedByUserID int,
@UpdatedByUserID int = null,
@QDateFormat varchar(20),
@BatchSize int = 2500,
@IsPasswordProtected bit = 'false', 
@ProtectionPassword varchar(50) = '',
@TotalSteps int = 12,
@NotifyEmailList varchar(1000) = '',
@IsBillable bit = 'true',
@Notes varchar(max) = null,
@defaultRules bit = 'true',
@IsFullFile bit = 'false'
AS
BEGIN

	set nocount on

	if @DatabaseFileTypeId = 0
		begin
			set @DatabaseFileTypeId = (select CodeId from UAD_Lookup..Code where CodeName = 'Audience Data' and CodeTypeId = 7)
		end

	IF @SourceFileID > 0
		  BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END				
			
			UPDATE SourceFile
			SET   FileName = @FileName,
				  FileRecurrenceTypeId = @FileRecurrenceTypeId,
				  DatabaseFileTypeId = @DatabaseFileTypeId,
				  ClientID = @ClientID,
				  PublicationID = @PublicationID,
				  IsDeleted = @IsDeleted,  
				  IsIgnored = @IsIgnored,
				  FileSnippetID = @FileSnippetID,  
				  Extension = @Extension,        
				  IsDQMReady = @IsDQMReady,                       
				  DateUpdated = @DateUpdated,
				  UpdatedByUserID = @UpdatedByUserID,
				  Delimiter = @Delimiter,
				  IsTextQualifier = @IsTextQualifier,
				  IsSpecialFile = @IsSpecialFile,
				  ClientCustomProcedureID = @ClientCustomProcedureID,
				  SpecialFileResultID = @SpecialFileResultID,
				  ServiceID = @ServiceID,
				  ServiceFeatureID = @ServiceFeatureID,
				  MasterGroupID = @MasterGroupID,
				  UseRealTimeGeocoding = @UseRealTimeGeocoding,
				  QDateFormat = @QDateFormat,
				  BatchSize = @BatchSize,
				  IsPasswordProtected = @IsPasswordProtected,
				  ProtectionPassword = @ProtectionPassword,
				  TotalSteps = @TotalSteps,
				  NotifyEmailList = @NotifyEmailList,
				  IsBillable = @IsBillable,
				  Notes = @Notes,
				  IsFullFile = @IsFullFile	  
			WHERE SourceFileID = @SourceFileID;

			update RuleSet_File_Map set FileTypeId = @DatabaseFileTypeId where SourceFileId = @SourceFileID

			SELECT @SourceFileID;
		  END
	ELSE
		  BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO SourceFile (FileName, FileRecurrenceTypeId, DatabaseFileTypeId, ClientID, PublicationID, IsDeleted, IsIgnored, FileSnippetID, Extension, IsDQMReady, DateCreated,CreatedByUserID, Delimiter, IsTextQualifier,IsSpecialFile,ClientCustomProcedureID,SpecialFileResultID,ServiceID,ServiceFeatureID,MasterGroupID,UseRealTimeGeocoding,QDateFormat,BatchSize,IsPasswordProtected,ProtectionPassword,TotalSteps,NotifyEmailList, IsBillable,Notes,IsFullFile)
			VALUES(@FileName, @FileRecurrenceTypeId, @DatabaseFileTypeId, @ClientID, @PublicationID, @IsDeleted, @IsIgnored, @FileSnippetID,@Extension,@IsDQMReady,@DateCreated,@CreatedByUserID, @Delimiter, @IsTextQualifier,@IsSpecialFile,@ClientCustomProcedureID,@SpecialFileResultID,@ServiceID,@ServiceFeatureID,@MasterGroupID,@UseRealTimeGeocoding,@QDateFormat,@BatchSize,@IsPasswordProtected,@ProtectionPassword,@TotalSteps,@NotifyEmailList,@IsBillable,@Notes,@IsFullFile);
			declare @sfId int = (select @@IDENTITY)

			--------setup default rules for the filetype
			--adding a default true bit parameter @defaultRules = true
			if(@defaultRules = 'true')
				begin
					--declare @CustomImportRuleId int
					--declare @ctCIR int = (select codeTypeId from UAD_Lookup..CodeType where CodeTypeName = 'Custom Import Rule')
					--set @CustomImportRuleId = (select CodeId from UAD_Lookup..Code where CodeTypeid=@ctCIR and CodeName = 'ADMS')
					
					--insert into RuleSet_File_Map (RuleSetId,ExecutionPointId,ExecutionOrder,IsGlobal,FileTypeId,IsSystem,IsActive,DateCreated,CreatedByUserId,SourceFileId)

					--select rsm.RuleSetId,rsm.ExecutionPointId,rsm.ExecutionOrder,'false',@DatabaseFileTypeId,'false','true',getdate(),1,@sfId
					--from RuleSet rs with(nolock)--RuleSet_File_Map rsm with(nolock)
					--where rs.IsSystem ='true'
					--and rs.IsGlobal = 'true'
					--and rs.IsActive ='true'
					--and rs.ClientId = 0
					----and rs.SourceFileId = 0
					--and rs.CustomImportRuleId = @CustomImportRuleId
					--and rs.RuleSetId not in (6,7)

					declare @CustomImportRuleId int
					declare @ctCIR int = (select codeTypeId from UAD_Lookup..CodeType where CodeTypeName = 'Custom Import Rule')
					set @CustomImportRuleId = (select CodeId from UAD_Lookup..Code where CodeTypeid=@ctCIR and CodeName = 'ADMS')

					insert into RuleSet_File_Map (RuleSetId,ExecutionPointId,ExecutionOrder,IsGlobal,FileTypeId,IsSystem,IsActive,DateCreated,CreatedByUserId,SourceFileId)
					select rsm.RuleSetId,rsm.ExecutionPointId,rsm.ExecutionOrder,'false',@DatabaseFileTypeId,
							'false','true',getdate(),1,@sfId
					from RuleSet_File_Map rsm with(nolock)
					join RuleSet rs with(nolock) on rsm.RuleSetId = rs.RuleSetId
					where 
							rs.ClientId = 0
						and rs.CustomImportRuleId = @CustomImportRuleId
						and rs.RuleSetId not in (6,7)
						and	rs.IsSystem ='true'
						and rs.IsGlobal = 'true'
						and rs.IsActive ='true'

						and	rsm.IsSystem ='true'
						and rsm.IsGlobal = 'true'
						and rsm.IsActive ='true'
						and rsm.SourceFileId = 0
				end

			select @sfId;
		  END

END
GO