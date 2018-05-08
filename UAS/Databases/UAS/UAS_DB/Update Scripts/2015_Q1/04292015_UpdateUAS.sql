/*
Deployment script for UAD

BACKUP DATABASE UASDeployTest to DISK = '\\10.10.41.250\Backups\MAF\UASDeployTest.bak' WITH COMPRESSION, INIT

use master
go
RESTORE DATABASE UASDeployTest FROM  DISK = '\\10.10.41.250\Backups\MAF\UASDeployTest.bak' WITH REPLACE, RECOVERY

*/
GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;
SET NUMERIC_ROUNDABORT OFF;

GO

USE UASDeployTest;

GO
/*
The column FileStatusTypeID on table [dbo].[FileStatus] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

IF EXISTS (select top 1 1 from [dbo].[FileStatus]) RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[SourceFile]) RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[DataCompareCost])  RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[DataCompareCostToClient]) RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[DataCompareCostToUser])   RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[DataCompareOption])     RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[DataCompareOptionCodeMap])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[DataCompareResult])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[DataCompareResultQue])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[DataCompareUserLikeCriteria])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[DataCompareUserMatchCriteria])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[DataCompareUserMatrix])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[FieldMappingType])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[FileSnippetType])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[FileStatusType])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[RegionMap_04142015])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[Rules])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[RulesMap])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[SourceFileType])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[SpecialFileResult])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[tmpCountry])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[tmpDQMQue_Mtg])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[tmpRegion])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[tmpSubscriptionsLatLon])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[TransformationType])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
IF EXISTS (select top 1 1 from [dbo].[UserLogType])    RAISERROR (N'Rows were detected. The schema update is terminating because data loss might occur.', 16, 127) WITH NOWAIT
*/
GO
ALTER TABLE [dbo].[Client] DROP CONSTRAINT [DF_Client_AccountManagerEmails];


GO
PRINT N'Dropping DF_Client_ClientEmails...';


GO
ALTER TABLE [dbo].[Client] DROP CONSTRAINT [DF_Client_ClientEmails];


GO
PRINT N'Dropping DF_Client_IgnoreUnknownFiles...';


GO
ALTER TABLE [dbo].[Client] DROP CONSTRAINT [DF_Client_IgnoreUnknownFiles];


GO
PRINT N'Dropping DF_Client_IsActive...';


GO
ALTER TABLE [dbo].[Client] DROP CONSTRAINT [DF_Client_IsActive];


GO
PRINT N'Dropping DF_FileStatus_FileStatusTypeID...';


GO
ALTER TABLE [dbo].[FileStatus] DROP CONSTRAINT [DF_FileStatus_FileStatusTypeID];


GO
PRINT N'Dropping DF__tmp_ms_xx__Sourc__7BF04F28...';


GO
ALTER TABLE [dbo].[SourceFile] DROP CONSTRAINT [DF__tmp_ms_xx__Sourc__7BF04F28];


GO
PRINT N'Dropping DF__SourceFil__QDate__79D2FC8C...';


GO
ALTER TABLE [dbo].[SourceFile] DROP CONSTRAINT [DF__SourceFil__QDate__79D2FC8C];


GO
PRINT N'Dropping DF_SourceFile_ServiceFeatureID...';


GO
ALTER TABLE [dbo].[SourceFile] DROP CONSTRAINT [DF_SourceFile_ServiceFeatureID];


GO
PRINT N'Dropping DF_SourceFile_ServiceID...';


GO
ALTER TABLE [dbo].[SourceFile] DROP CONSTRAINT [DF_SourceFile_ServiceID];


GO
PRINT N'Dropping DF_SourceFile_IsIgnored...';


GO
ALTER TABLE [dbo].[SourceFile] DROP CONSTRAINT [DF_SourceFile_IsIgnored];


GO
PRINT N'Dropping DF_SourceFile_IsDeleted...';


GO
ALTER TABLE [dbo].[SourceFile] DROP CONSTRAINT [DF_SourceFile_IsDeleted];


GO
PRINT N'Dropping DF_SourceFile_MasterGroupID...';


GO
ALTER TABLE [dbo].[SourceFile] DROP CONSTRAINT [DF_SourceFile_MasterGroupID];


GO
PRINT N'Dropping DF_SourceFile_IsSpecialFile...';


GO
ALTER TABLE [dbo].[SourceFile] DROP CONSTRAINT [DF_SourceFile_IsSpecialFile];


GO
PRINT N'Dropping DF__tmp_ms_xx__UseRe__01A9287E...';


GO
ALTER TABLE [dbo].[SourceFile] DROP CONSTRAINT [DF__tmp_ms_xx__UseRe__01A9287E];


GO
PRINT N'Dropping DF__UserAutho__IsAut__29B719D8...';


GO
ALTER TABLE [dbo].[UserAuthorizationLog] DROP CONSTRAINT [DF__UserAutho__IsAut__29B719D8];


GO
PRINT N'Dropping DF__DataCompa__Summa__677F4227...';


GO
ALTER TABLE [dbo].[DataCompareCost] DROP CONSTRAINT [DF__DataCompa__Summa__677F4227];


GO
PRINT N'Dropping DF__DataCompa__Match__68736660...';


GO
ALTER TABLE [dbo].[DataCompareCost] DROP CONSTRAINT [DF__DataCompa__Match__68736660];


GO
PRINT N'Dropping DF__DataCompa__Match__69678A99...';


GO
ALTER TABLE [dbo].[DataCompareCost] DROP CONSTRAINT [DF__DataCompa__Match__69678A99];


GO
PRINT N'Dropping DF__DataCompa__LikeP__6A5BAED2...';


GO
ALTER TABLE [dbo].[DataCompareCost] DROP CONSTRAINT [DF__DataCompa__LikeP__6A5BAED2];


GO
PRINT N'Dropping DF__DataCompa__LikeD__6B4FD30B...';


GO
ALTER TABLE [dbo].[DataCompareCost] DROP CONSTRAINT [DF__DataCompa__LikeD__6B4FD30B];


GO
PRINT N'Dropping DF__DataCompa__Summa__6C43F744...';


GO
ALTER TABLE [dbo].[DataCompareCostToClient] DROP CONSTRAINT [DF__DataCompa__Summa__6C43F744];


GO
PRINT N'Dropping DF__DataCompa__CostP__6D381B7D...';


GO
ALTER TABLE [dbo].[DataCompareCostToClient] DROP CONSTRAINT [DF__DataCompa__CostP__6D381B7D];


GO
PRINT N'Dropping DF__DataCompa__Summa__6E2C3FB6...';


GO
ALTER TABLE [dbo].[DataCompareCostToUser] DROP CONSTRAINT [DF__DataCompa__Summa__6E2C3FB6];


GO
PRINT N'Dropping DF__DataCompa__CostP__6F2063EF...';


GO
ALTER TABLE [dbo].[DataCompareCostToUser] DROP CONSTRAINT [DF__DataCompa__CostP__6F2063EF];


GO
PRINT N'Dropping DF__DataCompa__Displ__70148828...';


GO
ALTER TABLE [dbo].[DataCompareOption] DROP CONSTRAINT [DF__DataCompa__Displ__70148828];


GO
PRINT N'Dropping DF__DataCompa__IsAct__7108AC61...';


GO
ALTER TABLE [dbo].[DataCompareOption] DROP CONSTRAINT [DF__DataCompa__IsAct__7108AC61];


GO
PRINT N'Dropping DF__DataCompa__IsAdd__71FCD09A...';


GO
ALTER TABLE [dbo].[DataCompareOption] DROP CONSTRAINT [DF__DataCompa__IsAdd__71FCD09A];


GO
PRINT N'Dropping DF__DataCompa__HasPr__72F0F4D3...';


GO
ALTER TABLE [dbo].[DataCompareOption] DROP CONSTRAINT [DF__DataCompa__HasPr__72F0F4D3];


GO
PRINT N'Dropping DF__DataCompa__HasSe__73E5190C...';


GO
ALTER TABLE [dbo].[DataCompareOption] DROP CONSTRAINT [DF__DataCompa__HasSe__73E5190C];


GO
PRINT N'Dropping DF__DataCompa__Match__74D93D45...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__Match__74D93D45];


GO
PRINT N'Dropping DF__DataCompa__Match__75CD617E...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__Match__75CD617E];


GO
PRINT N'Dropping DF__DataCompa__LikeP__76C185B7...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__LikeP__76C185B7];


GO
PRINT N'Dropping DF__DataCompa__LikeD__77B5A9F0...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__LikeD__77B5A9F0];


GO
PRINT N'Dropping DF__DataCompa__Match__78A9CE29...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__Match__78A9CE29];


GO
PRINT N'Dropping DF__DataCompa__Match__799DF262...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__Match__799DF262];


GO
PRINT N'Dropping DF__DataCompa__LikeP__7A92169B...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__LikeP__7A92169B];


GO
PRINT N'Dropping DF__DataCompa__LikeD__7B863AD4...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__LikeD__7B863AD4];


GO
PRINT N'Dropping DF__DataCompa__Total__7C7A5F0D...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__Total__7C7A5F0D];


GO
PRINT N'Dropping DF__DataCompa__Match__7D6E8346...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__Match__7D6E8346];


GO
PRINT N'Dropping DF__DataCompa__Match__7E62A77F...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__Match__7E62A77F];


GO
PRINT N'Dropping DF__DataCompa__LikeP__7F56CBB8...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__LikeP__7F56CBB8];


GO
PRINT N'Dropping DF__DataCompa__LikeD__004AEFF1...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__LikeD__004AEFF1];


GO
PRINT N'Dropping DF__DataCompa__Match__013F142A...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__Match__013F142A];


GO
PRINT N'Dropping DF__DataCompa__Match__02333863...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__Match__02333863];


GO
PRINT N'Dropping DF__DataCompa__LikeP__03275C9C...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__LikeP__03275C9C];


GO
PRINT N'Dropping DF__DataCompa__LikeD__041B80D5...';


GO
ALTER TABLE [dbo].[DataCompareResult] DROP CONSTRAINT [DF__DataCompa__LikeD__041B80D5];


GO
PRINT N'Dropping DF__tmp_ms_xx__IsCon__294D0584...';


GO
ALTER TABLE [dbo].[DataCompareResultQue] DROP CONSTRAINT [DF__tmp_ms_xx__IsCon__294D0584];


GO
PRINT N'Dropping DF__DataCompa__Group__06F7ED80...';


GO
ALTER TABLE [dbo].[DataCompareUserLikeCriteria] DROP CONSTRAINT [DF__DataCompa__Group__06F7ED80];


GO
PRINT N'Dropping DF__DataCompa__IsGro__0603C947...';


GO
ALTER TABLE [dbo].[DataCompareUserLikeCriteria] DROP CONSTRAINT [DF__DataCompa__IsGro__0603C947];


GO
PRINT N'Dropping DF__DataCompa__Group__08E035F2...';


GO
ALTER TABLE [dbo].[DataCompareUserMatchCriteria] DROP CONSTRAINT [DF__DataCompa__Group__08E035F2];


GO
PRINT N'Dropping DF__DataCompa__IsGro__07EC11B9...';


GO
ALTER TABLE [dbo].[DataCompareUserMatchCriteria] DROP CONSTRAINT [DF__DataCompa__IsGro__07EC11B9];


GO
PRINT N'Dropping DF__DataCompa__IsAct__09D45A2B...';


GO
ALTER TABLE [dbo].[DataCompareUserMatrix] DROP CONSTRAINT [DF__DataCompa__IsAct__09D45A2B];


GO
PRINT N'Dropping DF_SourceFileType_IsDeleted...';


GO
ALTER TABLE [dbo].[SourceFileType] DROP CONSTRAINT [DF_SourceFileType_IsDeleted];


GO
PRINT N'Dropping FK_Transformation_Client_ClientID...';


GO
ALTER TABLE [dbo].[Transformation] DROP CONSTRAINT [FK_Transformation_Client_ClientID];


GO
PRINT N'Dropping FK_FieldMapping_SourceFile...';


GO
ALTER TABLE [dbo].[FieldMapping] DROP CONSTRAINT [FK_FieldMapping_SourceFile];


GO
PRINT N'Dropping FK_TransFieldMap_SourceFile...';


GO
ALTER TABLE [dbo].[TransformationFieldMap] DROP CONSTRAINT [FK_TransFieldMap_SourceFile];


GO
PRINT N'Dropping FK_TransFieldMultiMap_SourceFile...';


GO
ALTER TABLE [dbo].[TransformationFieldMultiMap] DROP CONSTRAINT [FK_TransFieldMultiMap_SourceFile];


GO
PRINT N'Dropping FK_SourceFile_SourceFileType...';


GO
ALTER TABLE [dbo].[SourceFile] DROP CONSTRAINT [FK_SourceFile_SourceFileType];


GO
PRINT N'Dropping [dbo].[RegionMap_04142015]...';


GO
DROP TABLE [dbo].[RegionMap_04142015];


GO
PRINT N'Dropping [dbo].[tmpCountry]...';


GO
DROP TABLE [dbo].[tmpCountry];


GO
PRINT N'Dropping [dbo].[tmpDQMQue_Mtg]...';


GO
DROP TABLE [dbo].[tmpDQMQue_Mtg];


GO
PRINT N'Dropping [dbo].[tmpRegion]...';


GO
DROP TABLE [dbo].[tmpRegion];


GO
PRINT N'Dropping [dbo].[tmpSubscriptionsLatLon]...';


GO
DROP TABLE [dbo].[tmpSubscriptionsLatLon];


GO
PRINT N'Dropping [dbo].[fn_Levenshtein]...';


GO
DROP FUNCTION [dbo].[fn_Levenshtein];


GO
PRINT N'Dropping [dbo].[vwProducts]...';


GO
DROP VIEW [dbo].[vwProducts];


GO
PRINT N'Dropping [dbo].[ccp_TenMissions_DONO_Suppression]...';


GO
DROP PROCEDURE [dbo].[ccp_TenMissions_DONO_Suppression];


GO
PRINT N'Dropping [dbo].[e_DataCompareCost_Select]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareCost_Select];


GO
PRINT N'Dropping [dbo].[e_DataCompareCostToClient_Select_ClientId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareCostToClient_Select_ClientId];


GO
PRINT N'Dropping [dbo].[e_DataCompareCostToUser_Select_UserId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareCostToUser_Select_UserId];


GO
PRINT N'Dropping [dbo].[e_DataCompareOption_Select]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareOption_Select];


GO
PRINT N'Dropping [dbo].[e_DataCompareOptionCodeMap_Select]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareOptionCodeMap_Select];


GO
PRINT N'Dropping [dbo].[e_DataCompareOptionCodeMap_Select_DataCompareOptionId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareOptionCodeMap_Select_DataCompareOptionId];


GO
PRINT N'Dropping [dbo].[e_DataCompareResult_Select_ClientId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareResult_Select_ClientId];


GO
PRINT N'Dropping [dbo].[e_DataCompareResult_Select_SourceFileId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareResult_Select_SourceFileId];


GO
PRINT N'Dropping [dbo].[e_DataCompareResult_Select_UserId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareResult_Select_UserId];


GO
PRINT N'Dropping [dbo].[e_DataCompareResultQue_Save]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareResultQue_Save];


GO
PRINT N'Dropping [dbo].[e_DataCompareResultQue_Select_ClientId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareResultQue_Select_ClientId];


GO
PRINT N'Dropping [dbo].[e_DataCompareResultQue_Select_SourceFileId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareResultQue_Select_SourceFileId];


GO
PRINT N'Dropping [dbo].[e_DataCompareResultQue_Select_UserId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareResultQue_Select_UserId];


GO
PRINT N'Dropping [dbo].[e_DataCompareResultQue_SelectNotQued_SourceFileId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareResultQue_SelectNotQued_SourceFileId];


GO
PRINT N'Dropping [dbo].[e_DataCompareUserLikeCriteria_Select_DataCompareResultQueId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareUserLikeCriteria_Select_DataCompareResultQueId];


GO
PRINT N'Dropping [dbo].[e_DataCompareUserLikeCriteria_Select_SourceFileId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareUserLikeCriteria_Select_SourceFileId];


GO
PRINT N'Dropping [dbo].[e_DataCompareUserMatchCriteria_Select_DataCompareResultQueId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareUserMatchCriteria_Select_DataCompareResultQueId];


GO
PRINT N'Dropping [dbo].[e_DataCompareUserMatchCriteria_Select_SourceFileId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareUserMatchCriteria_Select_SourceFileId];


GO
PRINT N'Dropping [dbo].[e_DataCompareUserMatrix_Select_SourceFileId]...';


GO
DROP PROCEDURE [dbo].[e_DataCompareUserMatrix_Select_SourceFileId];


GO
PRINT N'Dropping [dbo].[e_FieldMappingType_Save]...';


GO
DROP PROCEDURE [dbo].[e_FieldMappingType_Save];


GO
PRINT N'Dropping [dbo].[e_FieldMappingType_Select]...';


GO
DROP PROCEDURE [dbo].[e_FieldMappingType_Select];


GO
PRINT N'Dropping [dbo].[e_FieldMappingType_Select_Name]...';


GO
DROP PROCEDURE [dbo].[e_FieldMappingType_Select_Name];


GO
PRINT N'Dropping [dbo].[e_FileLog_Select_TopOne_ProcessCode]...';


GO
DROP PROCEDURE [dbo].[e_FileLog_Select_TopOne_ProcessCode];


GO
PRINT N'Dropping [dbo].[e_FileSnippetType_Save]...';


GO
DROP PROCEDURE [dbo].[e_FileSnippetType_Save];


GO
PRINT N'Dropping [dbo].[e_FileSnippetType_Select]...';


GO
DROP PROCEDURE [dbo].[e_FileSnippetType_Select];


GO
PRINT N'Dropping [dbo].[e_FileStatusType_Select]...';


GO
DROP PROCEDURE [dbo].[e_FileStatusType_Select];


GO
PRINT N'Dropping [dbo].[e_FileStatusType_Select_FileStatusName]...';


GO
DROP PROCEDURE [dbo].[e_FileStatusType_Select_FileStatusName];


GO
PRINT N'Dropping [dbo].[e_FileStatusType_Select_FileStatusTypeID]...';


GO
DROP PROCEDURE [dbo].[e_FileStatusType_Select_FileStatusTypeID];


GO
PRINT N'Dropping [dbo].[e_Rules_Save]...';


GO
DROP PROCEDURE [dbo].[e_Rules_Save];


GO
PRINT N'Dropping [dbo].[e_Rules_Select]...';


GO
DROP PROCEDURE [dbo].[e_Rules_Select];


GO
PRINT N'Dropping [dbo].[e_RulesMap_Delete_SourceFileByID]...';


GO
DROP PROCEDURE [dbo].[e_RulesMap_Delete_SourceFileByID];


GO
PRINT N'Dropping [dbo].[e_RulesMap_Save]...';


GO
DROP PROCEDURE [dbo].[e_RulesMap_Save];


GO
PRINT N'Dropping [dbo].[e_RulesMap_Select]...';


GO
DROP PROCEDURE [dbo].[e_RulesMap_Select];


GO
PRINT N'Dropping [dbo].[e_SourceFileType_Select]...';


GO
DROP PROCEDURE [dbo].[e_SourceFileType_Select];


GO
PRINT N'Dropping [dbo].[e_SourceFileType_Select_SourceFileTypeID]...';


GO
DROP PROCEDURE [dbo].[e_SourceFileType_Select_SourceFileTypeID];


GO
PRINT N'Dropping [dbo].[e_SpecialFileResult_Select]...';


GO
DROP PROCEDURE [dbo].[e_SpecialFileResult_Select];


GO
PRINT N'Dropping [dbo].[e_SpecialFileResult_Select_SpecialFileResultID]...';


GO
DROP PROCEDURE [dbo].[e_SpecialFileResult_Select_SpecialFileResultID];


GO
PRINT N'Dropping [dbo].[e_SpecialFileResult_Select_SpecialFileResultName]...';


GO
DROP PROCEDURE [dbo].[e_SpecialFileResult_Select_SpecialFileResultName];


GO
PRINT N'Dropping [dbo].[e_TransformationType_Save]...';


GO
DROP PROCEDURE [dbo].[e_TransformationType_Save];


GO
PRINT N'Dropping [dbo].[e_TransformationType_Select]...';


GO
DROP PROCEDURE [dbo].[e_TransformationType_Select];


GO
PRINT N'Dropping [dbo].[e_TransformationType_Select_TransformationTypeID]...';


GO
DROP PROCEDURE [dbo].[e_TransformationType_Select_TransformationTypeID];


GO
PRINT N'Dropping [dbo].[e_UserLogType_Save]...';


GO
DROP PROCEDURE [dbo].[e_UserLogType_Save];


GO
PRINT N'Dropping [dbo].[e_UserLogType_Select]...';


GO
DROP PROCEDURE [dbo].[e_UserLogType_Select];


GO
PRINT N'Dropping [dbo].[job_DataCompareOptionCodeMap_UpdateWithCodes]...';


GO
DROP PROCEDURE [dbo].[job_DataCompareOptionCodeMap_UpdateWithCodes];


GO
PRINT N'Dropping [dbo].[job_DataCompareUserLike_Save]...';


GO
DROP PROCEDURE [dbo].[job_DataCompareUserLike_Save];


GO
PRINT N'Dropping [dbo].[job_DataCompareUserMatch_Save]...';


GO
DROP PROCEDURE [dbo].[job_DataCompareUserMatch_Save];


GO
PRINT N'Dropping [dbo].[job_DataCompareUserMatrix_DisableAllDemographics]...';


GO
DROP PROCEDURE [dbo].[job_DataCompareUserMatrix_DisableAllDemographics];


GO
PRINT N'Dropping [dbo].[job_DataCompareUserMatrix_InsertAllDemographics]...';


GO
DROP PROCEDURE [dbo].[job_DataCompareUserMatrix_InsertAllDemographics];


GO
PRINT N'Dropping [dbo].[job_DataCompareUserMatrix_InsertAllProfileAttributes]...';


GO
DROP PROCEDURE [dbo].[job_DataCompareUserMatrix_InsertAllProfileAttributes];


GO
PRINT N'Dropping [dbo].[job_DataCompareUserMatrix_InsertSelectedDemographics]...';


GO
DROP PROCEDURE [dbo].[job_DataCompareUserMatrix_InsertSelectedDemographics];


GO
PRINT N'Dropping [dbo].[job_DataCompareUserMatrix_InsertSelectedProfileAttributes]...';


GO
DROP PROCEDURE [dbo].[job_DataCompareUserMatrix_InsertSelectedProfileAttributes];


GO
PRINT N'Dropping [dbo].[o_DataCompareResultQue_FileNameExist]...';


GO
DROP PROCEDURE [dbo].[o_DataCompareResultQue_FileNameExist];


GO
PRINT N'Dropping [dbo].[o_DataCompareResultQue_SetQued]...';


GO
DROP PROCEDURE [dbo].[o_DataCompareResultQue_SetQued];


GO
PRINT N'Dropping [dbo].[o_DataCompareResultQueGrid_Select_ClientId]...';


GO
DROP PROCEDURE [dbo].[o_DataCompareResultQueGrid_Select_ClientId];


GO
PRINT N'Dropping [dbo].[DataCompareCost]...';


GO
DROP TABLE [dbo].[DataCompareCost];


GO
PRINT N'Dropping [dbo].[DataCompareCostToClient]...';


GO
DROP TABLE [dbo].[DataCompareCostToClient];


GO
PRINT N'Dropping [dbo].[DataCompareCostToUser]...';


GO
DROP TABLE [dbo].[DataCompareCostToUser];


GO
PRINT N'Dropping [dbo].[DataCompareOption]...';


GO
DROP TABLE [dbo].[DataCompareOption];


GO
PRINT N'Dropping [dbo].[DataCompareOptionCodeMap]...';


GO
DROP TABLE [dbo].[DataCompareOptionCodeMap];


GO
PRINT N'Dropping [dbo].[DataCompareResult]...';


GO
DROP TABLE [dbo].[DataCompareResult];


GO
PRINT N'Dropping [dbo].[DataCompareResultQue]...';


GO
DROP TABLE [dbo].[DataCompareResultQue];


GO
PRINT N'Dropping [dbo].[DataCompareUserLikeCriteria]...';


GO
DROP TABLE [dbo].[DataCompareUserLikeCriteria];


GO
PRINT N'Dropping [dbo].[DataCompareUserMatchCriteria]...';


GO
DROP TABLE [dbo].[DataCompareUserMatchCriteria];


GO
PRINT N'Dropping [dbo].[DataCompareUserMatrix]...';


GO
DROP TABLE [dbo].[DataCompareUserMatrix];


GO
PRINT N'Dropping [dbo].[FieldMappingType]...';


GO
DROP TABLE [dbo].[FieldMappingType];


GO
PRINT N'Dropping [dbo].[FileSnippetType]...';


GO
DROP TABLE [dbo].[FileSnippetType];


GO
PRINT N'Dropping [dbo].[FileStatusType]...';


GO
DROP TABLE [dbo].[FileStatusType];


GO
PRINT N'Dropping [dbo].[Rules]...';


GO
DROP TABLE [dbo].[Rules];


GO
PRINT N'Dropping [dbo].[RulesMap]...';


GO
DROP TABLE [dbo].[RulesMap];


GO
PRINT N'Dropping [dbo].[SourceFileType]...';


GO
DROP TABLE [dbo].[SourceFileType];


GO
PRINT N'Dropping [dbo].[SpecialFileResult]...';


GO
DROP TABLE [dbo].[SpecialFileResult];


GO
PRINT N'Dropping [dbo].[TransformationType]...';


GO
DROP TABLE [dbo].[TransformationType];


GO
PRINT N'Dropping [dbo].[UserLogType]...';


GO
DROP TABLE [dbo].[UserLogType];


GO
PRINT N'Starting rebuilding table [dbo].[Client]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Client] (
    [ClientID]                     INT            IDENTITY (1, 1) NOT NULL,
    [ClientName]                   VARCHAR (100)  NOT NULL,
    [DisplayName]                  VARCHAR (100)  NULL,
    [ClientCode]                   VARCHAR (15)   NOT NULL,
    [ClientTestDBConnectionString] VARCHAR (255)  NOT NULL,
    [ClientLiveDBConnectionString] VARCHAR (255)  NOT NULL,
    [IsActive]                     BIT            CONSTRAINT [DF_Client_IsActive] DEFAULT ((1)) NULL,
    [IgnoreUnknownFiles]           BIT            CONSTRAINT [DF_Client_IgnoreUnknownFiles] DEFAULT ((0)) NOT NULL,
    [AccountManagerEmails]         VARCHAR (500)  CONSTRAINT [DF_Client_AccountManagerEmails] DEFAULT ('') NOT NULL,
    [ClientEmails]                 VARCHAR (1000) CONSTRAINT [DF_Client_ClientEmails] DEFAULT ('') NOT NULL,
    [DateCreated]                  DATETIME       NOT NULL,
    [DateUpdated]                  DATETIME       NULL,
    [CreatedByUserID]              INT            NOT NULL,
    [UpdatedByUserID]              INT            NULL,
    [HasPaid]                      BIT            DEFAULT 0 NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Client] PRIMARY KEY CLUSTERED ([ClientID] ASC) WITH (FILLFACTOR = 80)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Client])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Client] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Client] ([ClientID], [ClientName], [ClientCode], [ClientTestDBConnectionString], [ClientLiveDBConnectionString], [IsActive], [IgnoreUnknownFiles], [AccountManagerEmails], [ClientEmails], [DateCreated], [DateUpdated], [CreatedByUserID], [UpdatedByUserID])
        SELECT   [ClientID],
                 [ClientName],
                 [ClientCode],
                 [ClientTestDBConnectionString],
                 [ClientLiveDBConnectionString],
                 [IsActive],
                 [IgnoreUnknownFiles],
                 [AccountManagerEmails],
                 [ClientEmails],
                 [DateCreated],
                 [DateUpdated],
                 [CreatedByUserID],
                 [UpdatedByUserID]
        FROM     [dbo].[Client]
        ORDER BY [ClientID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Client] OFF;
    END

DROP TABLE [dbo].[Client];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Client]', N'Client';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Client]', N'PK_Client', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Altering [dbo].[FileStatus]...';


GO
ALTER TABLE [dbo].[FileStatus] ALTER COLUMN [FileStatusTypeID] INT NOT NULL;


GO
PRINT N'Starting rebuilding table [dbo].[SourceFile]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_SourceFile] (
    [SourceFileID]            INT           IDENTITY (1, 1) NOT NULL,
    [FileRecurrenceTypeId]    INT           DEFAULT (-1) NOT NULL,
    [DatabaseFileTypeId]      INT           DEFAULT (-1) NOT NULL,
    [FileName]                VARCHAR (250) NOT NULL,
    [ClientID]                INT           NOT NULL,
    [PublicationID]           INT           NULL,
    [IsDeleted]               BIT           CONSTRAINT [DF_SourceFile_IsDeleted] DEFAULT ((0)) NOT NULL,
    [IsIgnored]               BIT           CONSTRAINT [DF_SourceFile_IsIgnored] DEFAULT ((0)) NOT NULL,
    [FileSnippetID]           INT           NOT NULL,
    [Extension]               VARCHAR (10)  NULL,
    [IsDQMReady]              BIT           NULL,
    [Delimiter]               VARCHAR (10)  NULL,
    [IsTextQualifier]         BIT           NULL,
    [ServiceID]               INT           CONSTRAINT [DF_SourceFile_ServiceID] DEFAULT ((0)) NOT NULL,
    [ServiceFeatureID]        INT           CONSTRAINT [DF_SourceFile_ServiceFeatureID] DEFAULT ((0)) NOT NULL,
    [MasterGroupID]           INT           CONSTRAINT [DF_SourceFile_MasterGroupID] DEFAULT ((0)) NOT NULL,
    [UseRealTimeGeocoding]    BIT           DEFAULT ((0)) NOT NULL,
    [IsSpecialFile]           BIT           CONSTRAINT [DF_SourceFile_IsSpecialFile] DEFAULT ((0)) NOT NULL,
    [ClientCustomProcedureID] INT           NULL,
    [SpecialFileResultID]     INT           NULL,
    [DateCreated]             DATETIME      NOT NULL,
    [DateUpdated]             DATETIME      NULL,
    [CreatedByUserID]         INT           NOT NULL,
    [UpdatedByUserID]         INT           NULL,
    [QDateFormat]             VARCHAR (20)  DEFAULT ('MMDDYYYY') NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_SourceFile] PRIMARY KEY CLUSTERED ([SourceFileID] ASC) WITH (FILLFACTOR = 90)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[SourceFile])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SourceFile] ON;
        INSERT INTO [dbo].[tmp_ms_xx_SourceFile] ([SourceFileID], [FileName], [ClientID], [IsDeleted], [IsIgnored], [FileSnippetID], [Extension], [IsDQMReady], [Delimiter], [IsTextQualifier], [ServiceID], [ServiceFeatureID], [MasterGroupID], [UseRealTimeGeocoding], [IsSpecialFile], [ClientCustomProcedureID], [SpecialFileResultID], [DateCreated], [DateUpdated], [CreatedByUserID], [UpdatedByUserID], [QDateFormat])
        SELECT   [SourceFileID],
                 [FileName],
                 [ClientID],
                 [IsDeleted],
                 [IsIgnored],
                 [FileSnippetID],
                 [Extension],
                 [IsDQMReady],
                 [Delimiter],
                 [IsTextQualifier],
                 [ServiceID],
                 [ServiceFeatureID],
                 [MasterGroupID],
                 [UseRealTimeGeocoding],
                 [IsSpecialFile],
                 [ClientCustomProcedureID],
                 [SpecialFileResultID],
                 [DateCreated],
                 [DateUpdated],
                 [CreatedByUserID],
                 [UpdatedByUserID],
                 [QDateFormat]
        FROM     [dbo].[SourceFile]
        ORDER BY [SourceFileID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_SourceFile] OFF;
    END

DROP TABLE [dbo].[SourceFile];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_SourceFile]', N'SourceFile';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_SourceFile]', N'PK_SourceFile', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[ApiLog]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_ApiLog] (
    [ApiLogId]         INT              IDENTITY (1, 1) NOT NULL,
    [ClientID]         INT              NOT NULL,
    [AccessKey]        UNIQUEIDENTIFIER NOT NULL,
    [RequestFromIP]    VARCHAR (50)     NOT NULL,
    [ApiId]            INT              NULL,
    [Entity]           VARCHAR (100)    NULL,
    [Method]           VARCHAR (100)    NULL,
    [ErrorMessage]     VARCHAR (MAX)    NULL,
    [RequestData]      VARCHAR (MAX)    NULL,
    [ResponseData]     VARCHAR (MAX)    NULL,
    [RequestStartDate] DATE             NOT NULL,
    [RequestStartTime] TIME (7)         NOT NULL,
    [RequestEndDate]   DATE             NULL,
    [RequestEndTime]   TIME (7)         NULL,
    PRIMARY KEY CLUSTERED ([ApiLogId] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[ApiLog])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_ApiLog] ON;
        INSERT INTO [dbo].[tmp_ms_xx_ApiLog] ([ApiLogId], [ClientID], [AccessKey], [RequestFromIP], [ApiId], [Entity], [Method], [ErrorMessage], [RequestData], [ResponseData], [RequestStartDate], [RequestStartTime], [RequestEndDate], [RequestEndTime])
        SELECT   [ApiLogId],
                 [ClientID],
                 [AccessKey],
                 [RequestFromIP],
                 [ApiId],
                 [Entity],
                 [Method],
                 [ErrorMessage],
                 [RequestData],
                 [ResponseData],
                 [RequestStartDate],
                 [RequestStartTime],
                 [RequestEndDate],
                 [RequestEndTime]
        FROM     [dbo].[ApiLog]
        ORDER BY [ApiLogId] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_ApiLog] OFF;
    END

DROP TABLE [dbo].[ApiLog];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_ApiLog]', N'ApiLog';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[UserAuthorizationLog]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_UserAuthorizationLog] (
    [UserAuthLogID]   INT              IDENTITY (1, 1) NOT NULL,
    [AuthSource]      VARCHAR (50)     NOT NULL,
    [AuthMode]        VARCHAR (50)     NOT NULL,
    [AuthAttemptDate] DATE             NOT NULL,
    [AuthAttemptTime] TIME (7)         NOT NULL,
    [IsAuthenticated] BIT              DEFAULT ('false') NOT NULL,
    [IpAddress]       VARCHAR (15)     NULL,
    [AuthUserName]    VARCHAR (50)     NULL,
    [AuthAccessKey]   UNIQUEIDENTIFIER NULL,
    [ServerVariables] VARCHAR (MAX)    NULL,
    [AppVersion]      VARCHAR (50)     NULL,
    [UserID]          INT              NULL,
    [LogOutDate]      DATE             NULL,
    [LogOutTime]      TIME (7)         NULL,
    PRIMARY KEY CLUSTERED ([UserAuthLogID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[UserAuthorizationLog])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_UserAuthorizationLog] ON;
        INSERT INTO [dbo].[tmp_ms_xx_UserAuthorizationLog] ([UserAuthLogID], [AuthSource], [AuthMode], [AuthAttemptDate], [AuthAttemptTime], [IsAuthenticated], [IpAddress], [AuthUserName], [AuthAccessKey], [ServerVariables], [AppVersion], [UserID], [LogOutDate], [LogOutTime])
        SELECT   [UserAuthLogID],
                 [AuthSource],
                 [AuthMode],
                 [AuthAttemptDate],
                 [AuthAttemptTime],
                 [IsAuthenticated],
                 [IpAddress],
                 [AuthUserName],
                 [AuthAccessKey],
                 [ServerVariables],
                 [AppVersion],
                 [UserID],
                 [LogOutDate],
                 [LogOutTime]
        FROM     [dbo].[UserAuthorizationLog]
        ORDER BY [UserAuthLogID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_UserAuthorizationLog] OFF;
    END

DROP TABLE [dbo].[UserAuthorizationLog];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_UserAuthorizationLog]', N'UserAuthorizationLog';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[FileRule]...';


GO
CREATE TABLE [dbo].[FileRule] (
    [FileRuleId]       INT           IDENTITY (1, 1) NOT NULL,
    [RuleName]         VARCHAR (50)  NOT NULL,
    [DisplayName]      VARCHAR (100) NOT NULL,
    [Description]      VARCHAR (500) NULL,
    [RuleMethod]       VARCHAR (50)  NOT NULL,
    [ProcedureTypeId]  INT           NOT NULL,
    [ExecutionPointId] INT           NOT NULL,
    [IsActive]         BIT           NOT NULL,
    [DateCreated]      DATETIME      NOT NULL,
    [DateUpdated]      DATETIME      NULL,
    [CreatedByUserID]  INT           NOT NULL,
    [UpdatedByUserID]  INT           NULL,
    PRIMARY KEY CLUSTERED ([FileRuleId] ASC)
);


GO
PRINT N'Creating [dbo].[FileRuleMap]...';


GO
CREATE TABLE [dbo].[FileRuleMap] (
    [RulesAssignedID] INT      IDENTITY (1, 1) NOT NULL,
    [RulesID]         INT      NOT NULL,
    [SourceFileID]    INT      NOT NULL,
    [RuleOrder]       INT      NOT NULL,
    [IsActive]        BIT      NOT NULL,
    [DateCreated]     DATETIME NOT NULL,
    [DateUpdated]     DATETIME NULL,
    [CreatedByUserID] INT      NOT NULL,
    [UpdatedByUserID] INT      NULL,
    PRIMARY KEY CLUSTERED ([RulesAssignedID] ASC)
);


GO
PRINT N'Creating FK_Transformation_Client_ClientID...';


GO
ALTER TABLE [dbo].[Transformation] WITH NOCHECK
    ADD CONSTRAINT [FK_Transformation_Client_ClientID] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID]);


GO
PRINT N'Creating FK_FieldMapping_SourceFile...';


GO
ALTER TABLE [dbo].[FieldMapping] WITH NOCHECK
    ADD CONSTRAINT [FK_FieldMapping_SourceFile] FOREIGN KEY ([SourceFileID]) REFERENCES [dbo].[SourceFile] ([SourceFileID]);


GO
PRINT N'Creating FK_TransFieldMap_SourceFile...';


GO
ALTER TABLE [dbo].[TransformationFieldMap] WITH NOCHECK
    ADD CONSTRAINT [FK_TransFieldMap_SourceFile] FOREIGN KEY ([SourceFileID]) REFERENCES [dbo].[SourceFile] ([SourceFileID]);


GO
PRINT N'Creating FK_TransFieldMultiMap_SourceFile...';


GO
ALTER TABLE [dbo].[TransformationFieldMultiMap] WITH NOCHECK
    ADD CONSTRAINT [FK_TransFieldMultiMap_SourceFile] FOREIGN KEY ([SourceFileID]) REFERENCES [dbo].[SourceFile] ([SourceFileID]);


GO

PRINT N'Altering DQMQue - adding SourceFileId...';


GO
ALTER TABLE DQMQue
Add SourceFileId int NULL
go

PRINT N'Altering [e_DQMQue_Save] - adding SourceFileId...';


GO
ALTER PROCEDURE [e_DQMQue_Save]
@ProcessCode   varchar(50),
@ClientID	  int,
@IsDemo bit,
@IsADMS bit = false,
@DateCreated   datetime,
@IsQued	      bit,
@DateQued	  datetime,
@IsCompleted   bit,
@DateCompleted datetime,
@SourceFileId int = 0
AS
	IF EXISTS (Select ProcessCode From DQMQue with(nolock) Where ProcessCode = @ProcessCode and ClientID = @ClientID and IsDemo = @IsDemo)
	BEGIN
		UPDATE DQMQue
		SET IsQued = @IsQued,
			DateQued = @DateQued,
			IsCompleted = @IsCompleted,
			DateCompleted = @DateCompleted
		WHERE ProcessCode = @ProcessCode and ClientID = @ClientID;
		
		SELECT @ProcessCode;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO DQMQue (ProcessCode,ClientID,IsDemo,IsADMS,DateCreated,IsQued,DateQued,IsCompleted,DateCompleted,SourceFileId)
		VALUES(@ProcessCode,@ClientID,@IsDemo,@IsADMS,@DateCreated,@IsQued,@DateQued,@IsCompleted,@DateCompleted,@SourceFileId);SELECT @ProcessCode;
	END
GO




PRINT N'Altering [dbo].[e_Client_Save]...';


GO
ALTER PROCEDURE e_Client_Save
@ClientID int,
@ClientName varchar(100),
@DisplayName varchar(100),
@ClientCode varchar(15),
@ClientTestDBConnectionString varchar(255),
@ClientLiveDBConnectionString varchar(255),
@IsActive bit,
@IgnoreUnknownFiles bit,
@AccountManagerEmails varchar(500),
@ClientEmails varchar(1000),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@HasPaid bit
AS

IF @ClientID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Client
		SET ClientName = @ClientName,
			DisplayName = @DisplayName,
			ClientCode = @ClientCode,
			ClientTestDBConnectionString = @ClientTestDBConnectionString,
			ClientLiveDBConnectionString = @ClientLiveDBConnectionString,
			IsActive = @IsActive,
			IgnoreUnknownFiles = @IgnoreUnknownFiles,
			AccountManagerEmails = @AccountManagerEmails,
			ClientEmails = @ClientEmails,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			HasPaid = @HasPaid
		WHERE ClientID = @ClientID;
		
		SELECT @ClientID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Client (ClientName,DisplayName,ClientCode,ClientTestDBConnectionString,ClientLiveDBConnectionString,IsActive,IgnoreUnknownFiles,AccountManagerEmails,ClientEmails,DateCreated,CreatedByUserID,HasPaid)
		VALUES(@ClientName,@DisplayName,@ClientCode,@ClientTestDBConnectionString,@ClientLiveDBConnectionString,@IsActive,@IgnoreUnknownFiles,@AccountManagerEmails,@ClientEmails,@DateCreated,@CreatedByUserID,@HasPaid);SELECT @@IDENTITY;
	END
GO
PRINT N'Altering [dbo].[o_HasService_ClientID_ServiceName]...';


GO
ALTER PROCEDURE o_HasService_ClientID_ServiceName
@ClientID int,
@ServiceName varchar(100)
AS
	select ISNULL(cgsm.IsEnabled,'false') 
	from Client c with(nolock)
	join ClientGroupClientMap cgcm with(nolock) on c.ClientID = cgcm.ClientID
	join ClientGroup cg with(nolock) on cg.ClientGroupID = cgcm.ClientGroupID
	join ClientGroupServiceMap cgsm with(nolock) on cg.ClientGroupID = cgsm.ClientGroupID
	join Service s with(nolock) on cgsm.ServiceID = s.ServiceID
	where c.ClientID = @ClientID
	and s.ServiceName = @ServiceName
GO
PRINT N'Altering [dbo].[rpt_ClientFileLog_ClientID]...';


GO
ALTER PROCEDURE rpt_ClientFileLog_ClientID
@ClientID int
AS
SELECT  c.ClientID,c.ClientName,
		s.SourceFileID,s.FileName,
		fst.CodeName as 'FileStatusName',fst.CodeValue as 'FileStatusCode',fst.CodeDescription as 'FileStatusDescription',
		f.LogDate,f.LogTime,f.Message
FROM FileLog f With(NoLock)
JOIN SourceFile s With(NoLock) ON f.SourceFileID = f.SourceFileID 
JOIN Code fst With(NoLock) ON f.FileStatusTypeID = fst.CodeId
JOIN Client c With(NoLock) ON s.ClientID = c.ClientID
WHERE c.ClientID = @ClientID 
ORDER BY c.ClientID,s.FileName,f.LogDate,f.LogTime
GO
PRINT N'Altering [dbo].[rpt_ClientFileLog_ClientID_LogDate]...';


GO
ALTER PROCEDURE [rpt_ClientFileLog_ClientID_LogDate]
@ClientID int,
@LogDate date
AS
SELECT  c.ClientID,c.ClientName,
		s.SourceFileID,s.FileName,
		fst.CodeName as 'FileStatusName',fst.CodeValue as 'FileStatusCode',fst.CodeDescription as 'FileStatusDescription',
		f.LogDate,f.LogTime,f.Message
FROM FileLog f With(NoLock)
JOIN SourceFile s With(NoLock) ON f.SourceFileID = f.SourceFileID 
JOIN Code fst With(NoLock) ON f.FileStatusTypeID = fst.CodeId
JOIN Client c With(NoLock) ON s.ClientID = c.ClientID
WHERE c.ClientID = @ClientID 
AND CAST(f.LogDate as date) = @LogDate
ORDER BY c.ClientID,s.FileName,f.LogDate,f.LogTime
GO
PRINT N'Altering [dbo].[rpt_ClientFileLog_ClientID_StartDate_EndDate]...';


GO
ALTER PROCEDURE [rpt_ClientFileLog_ClientID_StartDate_EndDate]
@ClientID int,
@StartDate date,
@EndDate date
AS
SELECT  c.ClientID,c.ClientName,
		s.SourceFileID,s.FileName,
		fst.CodeName as 'FileStatusName',fst.CodeValue as 'FileStatusCode',fst.CodeDescription as 'FileStatusDescription',
		f.LogDate,f.LogTime,f.Message
FROM FileLog f With(NoLock)
JOIN SourceFile s With(NoLock) ON f.SourceFileID = f.SourceFileID 
JOIN Code fst With(NoLock) ON f.FileStatusTypeID = fst.CodeId
JOIN Client c With(NoLock) ON s.ClientID = c.ClientID
WHERE c.ClientID = @ClientID 
AND CAST(f.LogDate as date) BETWEEN @StartDate AND @EndDate
ORDER BY c.ClientID,s.FileName,f.LogDate,f.LogTime
GO
PRINT N'Altering [dbo].[rpt_ClientFileLog_LogDate]...';


GO
ALTER PROCEDURE [rpt_ClientFileLog_LogDate]
@LogDate date
AS
SELECT  c.ClientID,c.ClientName,
		s.SourceFileID,s.FileName,
		fst.CodeName as 'FileStatusName',fst.CodeValue as 'FileStatusCode',fst.CodeDescription as 'FileStatusDescription',
		f.LogDate,f.LogTime,f.Message
FROM FileLog f With(NoLock)
JOIN SourceFile s With(NoLock) ON f.SourceFileID = f.SourceFileID 
JOIN Code fst With(NoLock) ON f.FileStatusTypeID = fst.CodeId
JOIN Client c With(NoLock) ON s.ClientID = c.ClientID
WHERE CAST(f.LogDate as date) = @LogDate
ORDER BY c.ClientID,s.FileName,f.LogDate,f.LogTime
GO
PRINT N'Altering [dbo].[rpt_ClientFileLog_StartDate_EndDate]...';


GO
ALTER PROCEDURE [rpt_ClientFileLog_StartDate_EndDate]
@StartDate date,
@EndDate date
AS
SELECT  c.ClientID,c.ClientName,
		s.SourceFileID,s.FileName,
		fst.CodeName as 'FileStatusName',fst.CodeValue as 'FileStatusCode',fst.CodeDescription as 'FileStatusDescription',
		f.LogDate,f.LogTime,f.Message
FROM FileLog f With(NoLock)
JOIN SourceFile s With(NoLock) ON f.SourceFileID = f.SourceFileID 
JOIN Code fst With(NoLock) ON f.FileStatusTypeID = fst.CodeId
JOIN Client c With(NoLock) ON s.ClientID = c.ClientID
WHERE CAST(f.LogDate as date) BETWEEN @StartDate AND @EndDate
ORDER BY c.ClientID,s.FileName,f.LogDate,f.LogTime
GO
PRINT N'Altering [dbo].[e_FileStatus_Create]...';


GO
ALTER PROCEDURE [e_FileStatus_Create]
@SourceFileID int,
@ClientID int,
@FileStatusName varchar(50),
@DateCreated datetime,
@CreatedByUserID int
AS
BEGIN
	IF @DateCreated IS NULL
		BEGIN
			SET @DateCreated = GETDATE();
		END
	DECLARE @FileStatusTypeID int = (Select c.CodeID 
							     From Code c With(NoLock) 
								 Join CodeType ct with(nolock) on c.CodeTypeId = ct.CodeTypeId	 
							     Where ct.CodeTypeName = 'File Status'
							     and c.CodeName = @FileStatusName)
	IF NOT EXISTS(Select FileStatusID FROM FileStatus With(NoLock) WHERE ClientID = @ClientID AND SourceFileID = @SourceFileID)
		BEGIN
			DECLARE @FileStatusID int
		                           								 
			INSERT INTO FileStatus (SourceFileID,ClientID,FileStatusTypeID,DateCreated,CreatedByUserID)
			VALUES(@SourceFileID,@ClientID,@FileStatusTypeID,@DateCreated,@CreatedByUserID);SET @FileStatusID = @@IDENTITY;
			
			SELECT @FileStatusID as 'FileStatusID',
				   @SourceFileID as 'SourceFileID',
				   @ClientID as 'ClientID',
				   @FileStatusTypeID as 'FileStatusTypeID',
				   @DateCreated as 'DateCreated',
				   null as 'DateUpdated',
				   @CreatedByUserID as 'CreatedByUserID',
				   null as 'UpdatedByUserID'
		END
	ELSE
		BEGIN
			UPDATE FileStatus
			SET 
				FileStatusTypeID = @FileStatusTypeID,
				DateUpdated = GETDATE(),
				UpdatedByUserID = @CreatedByUserID
			WHERE FileStatusID = @FileStatusID;
			
			SELECT *
			FROM FileStatus With(NoLock)
			WHERE SourceFileID = @SourceFileID 
			AND ClientID = @ClientID
		END
	
END
GO

ALTER PROCEDURE [e_FileStatus_Create]
@SourceFileID int,
@ClientID int,
@FileStatusName varchar(50),
@DateCreated datetime,
@CreatedByUserID int
AS
BEGIN
	IF @DateCreated IS NULL
		BEGIN
			SET @DateCreated = GETDATE();
		END
	DECLARE @FileStatusTypeID int = (Select isnull(c.CodeID,0)
									 From Code c With(NoLock) 
									 Join CodeType ct with(nolock) on c.CodeTypeId = ct.CodeTypeId	 
									 Where ct.CodeTypeName = 'File Status'
									 and c.CodeName = @FileStatusName)
    if(@FileStatusTypeID is null)
            begin
                  set @FileStatusTypeID = (Select isnull(c.CodeID,0)
                                                      From Code c With(NoLock) 
                                                       Join CodeType ct with(nolock) on c.CodeTypeId = ct.CodeTypeId  
                                                       Where ct.CodeTypeName = 'File Status'
                                                      and c.CodeName = REPLACE(@FileStatusName,'_',' '))
            end

	if(@FileStatusTypeID = 0)
		begin
			set @FileStatusTypeID = (Select isnull(c.CodeID,0)
									 From Code c With(NoLock) 
									 Join CodeType ct with(nolock) on c.CodeTypeId = ct.CodeTypeId	 
									 Where ct.CodeTypeName = 'File Status'
									 and c.CodeName = REPLACE(@FileStatusName,'_',' '))
		end

	IF NOT EXISTS(Select FileStatusID FROM FileStatus With(NoLock) WHERE ClientID = @ClientID AND SourceFileID = @SourceFileID)
		BEGIN
			DECLARE @FileStatusID int
		                           								 
			INSERT INTO FileStatus (SourceFileID,ClientID,FileStatusTypeID,DateCreated,CreatedByUserID)
			VALUES(@SourceFileID,@ClientID,@FileStatusTypeID,@DateCreated,@CreatedByUserID);SET @FileStatusID = @@IDENTITY;
			
			SELECT @FileStatusID as 'FileStatusID',
				   @SourceFileID as 'SourceFileID',
				   @ClientID as 'ClientID',
				   @FileStatusTypeID as 'FileStatusTypeID',
				   @DateCreated as 'DateCreated',
				   null as 'DateUpdated',
				   @CreatedByUserID as 'CreatedByUserID',
				   null as 'UpdatedByUserID'
		END
	ELSE
		BEGIN
			UPDATE FileStatus
			SET 
				FileStatusTypeID = @FileStatusTypeID,
				DateUpdated = GETDATE(),
				UpdatedByUserID = @CreatedByUserID
			WHERE FileStatusID = @FileStatusID;
			
			SELECT *
			FROM FileStatus With(NoLock)
			WHERE SourceFileID = @SourceFileID 
			AND ClientID = @ClientID
		END
	
END
GO

PRINT N'Altering [dbo].[e_FileStatus_SetStatus]...';


GO

ALTER PROCEDURE [e_FileStatus_SetStatus]
@FileStatusID int,
@FileStatusName varchar(50),
@DateUpdated datetime,
@UpdatedByUserID int
AS
BEGIN
	 IF @DateUpdated IS NULL
		BEGIN
			SET @DateUpdated = GETDATE();
		END
	DECLARE @FileStatusTypeID int = (Select c.CodeID 
									 From Code c With(NoLock) 
									 Join CodeType ct with(nolock) on c.CodeTypeId = ct.CodeTypeId	 
									 Where ct.CodeTypeName = 'File Status'
									 and c.CodeName = @FileStatusName)
                       								 
	UPDATE FileStatus 
	SET 
		FileStatusTypeID = @FileStatusTypeID,
		DateUpdated = @DateUpdated,
		UpdatedByUserID = @UpdatedByUserID
	WHERE FileStatusID = @FileStatusID
	
	SELECT @FileStatusTypeID
END
GO
PRINT N'Altering [dbo].[e_SourceFile_Save]...';


GO

alter PROCEDURE [dbo].[e_SourceFile_Save]
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
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@QDateFormat varchar(20),
@BatchSize int = 2500

AS
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
			  BatchSize = @BatchSize	  		  
        WHERE SourceFileID = @SourceFileID;

        SELECT @SourceFileID;
      END
ELSE
      BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
        INSERT INTO SourceFile (FileName, FileRecurrenceTypeId, DatabaseFileTypeId, ClientID, PublicationID, IsDeleted, IsIgnored, FileSnippetID, Extension, IsDQMReady, DateCreated,CreatedByUserID, Delimiter, IsTextQualifier,IsSpecialFile,ClientCustomProcedureID,SpecialFileResultID,ServiceID,ServiceFeatureID,MasterGroupID,UseRealTimeGeocoding,QDateFormat,BatchSize)
        VALUES(@FileName, @FileRecurrenceTypeId, @DatabaseFileTypeId, @ClientID, @PublicationID, @IsDeleted, @IsIgnored, @FileSnippetID,@Extension,@IsDQMReady,@DateCreated,@CreatedByUserID, @Delimiter, @IsTextQualifier,@IsSpecialFile,@ClientCustomProcedureID,@SpecialFileResultID,@ServiceID,@ServiceFeatureID,@MasterGroupID,@UseRealTimeGeocoding,@QDateFormat,@BatchSize);SELECT @@IDENTITY;
      END
GO

PRINT N'Altering [dbo].[e_DQMQue_Save]...';


GO
ALTER PROCEDURE [dbo].[e_DQMQue_Save]
@ProcessCode   varchar(50),
@ClientID	  int,
@IsDemo bit,
@IsADMS bit = false,
@DateCreated   datetime,
@IsQued	      bit,
@DateQued	  datetime,
@IsCompleted   bit,
@DateCompleted datetime
AS
	IF EXISTS (Select ProcessCode From DQMQue Where ProcessCode = @ProcessCode and ClientID = @ClientID and IsDemo = @IsDemo)
	BEGIN
		UPDATE DQMQue
		SET IsQued = @IsQued,
			DateQued = @DateQued,
			IsCompleted = @IsCompleted,
			DateCompleted = @DateCompleted
		WHERE ProcessCode = @ProcessCode and ClientID = @ClientID;
		
		SELECT @ProcessCode;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO DQMQue (ProcessCode,ClientID,IsDemo,IsADMS,DateCreated,IsQued,DateQued,IsCompleted,DateCompleted)
		VALUES(@ProcessCode,@ClientID,@IsDemo,@IsADMS,@DateCreated,@IsQued,@DateQued,@IsCompleted,@DateCompleted);SELECT @ProcessCode;
	END
GO
PRINT N'Altering [dbo].[e_FieldMapping_Save]...';


GO
ALTER PROCEDURE [e_FieldMapping_Save]
@FieldMappingID int,
@FieldMappingTypeID int,
@IsNonFileColumn bit,
@SourceFileID int,
@IncomingField varchar(50),
@MAFField varchar(50),
@PubNumber int,
@DataType varchar(50),
@PreviewData varchar(1000),
@ColumnOrder int,
@HasMultiMapping bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @FieldMappingID > 0
      BEGIN
            IF @DateUpdated IS NULL
                  BEGIN
                        SET @DateUpdated = GETDATE();
                  END
                  
        UPDATE FieldMapping
        SET 
              SourceFileID = @SourceFileID,
			  FieldMappingTypeID = @FieldMappingTypeID,
			  IsNonFileColumn = @IsNonFileColumn,
			  IncomingField = @IncomingField,
			  MAFField = @MAFField,
			  PubNumber = @PubNumber,
			  DataType = @DataType,
			  PreviewData = @PreviewData,
			  ColumnOrder = @ColumnOrder,
			  HasMultiMapping = @HasMultiMapping,
			  DateUpdated = @DateUpdated,
			  UpdatedByUserID = @UpdatedByUserID
        WHERE FieldMappingID = @FieldMappingID;

        SELECT @FieldMappingID;
      END
ELSE
      BEGIN
            IF @DateCreated IS NULL
                  BEGIN
                        SET @DateCreated = GETDATE();
                  END
        INSERT INTO FieldMapping (FieldMappingTypeID, IsNonFileColumn, SourceFileID, IncomingField, MAFField, PubNumber, DataType, PreviewData, ColumnOrder, HasMultiMapping, DateCreated, CreatedByUserID)
            VALUES(@FieldMappingTypeID, @IsNonFileColumn, @SourceFileID, @IncomingField, @MAFField, @PubNumber, @DataType, @PreviewData, @ColumnOrder, @HasMultiMapping, @DateCreated, @CreatedByUserID);SELECT @@IDENTITY;   
      END
GO
PRINT N'Altering [dbo].[e_SecurityGroup_Save]...';


GO
ALTER PROCEDURE [e_SecurityGroup_Save]
@SecurityGroupID int,
@SecurityGroupName varchar(50),
@MasterClientID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @SecurityGroupID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		IF NOT EXISTS(Select SecurityGroupID From SecurityGroup with(nolock) Where SecurityGroupName = @SecurityGroupName and MasterClientID = @MasterClientID)
			BEGIN
				UPDATE SecurityGroup
				SET SecurityGroupName = @SecurityGroupName,
					IsActive = @IsActive,
					DateUpdated = @DateUpdated,
					UpdatedByUserID = @UpdatedByUserID
				WHERE SecurityGroupID = @SecurityGroupID;
			END

		SELECT @SecurityGroupID;			
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		IF NOT EXISTS(Select SecurityGroupID From SecurityGroup with(nolock) Where SecurityGroupName = @SecurityGroupName and MasterClientID = @MasterClientID)
			BEGIN
				INSERT INTO SecurityGroup (SecurityGroupName,MasterClientID,IsActive,DateCreated,CreatedByUserID)
				VALUES(@SecurityGroupName,@MasterClientID,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
			END
		ELSE
			BEGIN
				SELECT 0;
			END
	END
GO
PRINT N'Altering [dbo].[e_Service_Save]...';


GO
ALTER PROCEDURE e_Service_Save
@ServiceID int,
@ServiceName varchar(100),
@Description varchar(500),
@ServiceCode varchar(5),
@DisplayOrder int,
@IsEnabled bit,
@IsAdditionalCost bit,
@HasFeatures bit,
@DefaultRate decimal(14,2),
@DefaultDurationInMonths int,
@DefaultApplicationID int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @ServiceID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Service
		SET ServiceName = @ServiceName,
			Description = @Description,
			ServiceCode = @ServiceCode,
			DisplayOrder = @DisplayOrder,
			IsEnabled = @IsEnabled,
			IsAdditionalCost = @IsAdditionalCost,
			HasFeatures = @HasFeatures,
			DefaultRate = @DefaultRate,
			DefaultDurationInMonths = @DefaultDurationInMonths,
			DefaultApplicationID = @DefaultApplicationID,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ServiceID = @ServiceID

		SELECT @ServiceID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Service (ServiceID,ServiceName,Description,ServiceCode,DisplayOrder,IsEnabled,IsAdditionalCost,HasFeatures,DefaultRate,DefaultDurationInMonths,DefaultApplicationID,DateCreated,CreatedByUserID)
		VALUES(@ServiceID,@ServiceName,@Description,@ServiceCode,@DisplayOrder,@IsEnabled,@IsAdditionalCost,@HasFeatures,@DefaultRate,@DefaultDurationInMonths,@DefaultApplicationID,@DateCreated,@CreatedByUserID);Select @@IDENTITY;
	END
GO
PRINT N'Altering [dbo].[e_Service_Select_SecurityGroup]...';


GO
ALTER procedure e_Service_Select_SecurityGroup
@SecurityGroupID int
as
	select s.* 
	from service s with(nolock)
	join SecurityGroupServicMap sgsm with(nolock) on s.ServiceID = sgsm.ServiceID 
	where sgsm.SecurityGroupID = @SecurityGroupID
	and sgsm.IsEnabled = 'true'
	and s.IsEnabled = 'true'
GO
PRINT N'Creating [dbo].[e_FileRule_Save]...';


GO
CREATE PROCEDURE [dbo].[e_FileRule_Save]
	@FileRuleId INT,
    @RuleName VARCHAR(50),
	@DisplayName VARCHAR(100),
	@Description VARCHAR(500),
    @RuleMethod VARCHAR(50),
	@ProcedureTypeId INT,
	@ExecutionPointId INT,
	@IsActive BIT,
    @DateCreated DATETIME,
    @DateUpdated DATETIME,
    @CreatedByUserID INT,
    @UpdatedByUserID INT
AS
	
	IF @FileRuleID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE FileRule
		SET RuleName = @RuleName,
			DisplayName = @DisplayName,
			Description = @Description,
			RuleMethod = @RuleMethod,
			ProcedureTypeId = @ProcedureTypeId,
			ExecutionPointId = @ExecutionPointId,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE FileRuleID = @FileRuleID;
		
		SELECT @FileRuleID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO FileRule (RuleName,DisplayName,Description,RuleMethod,ProcedureTypeId,ExecutionPointId,IsActive,DateCreated,CreatedByUserID)
		VALUES(@RuleName,@DisplayName,@Description,@RuleMethod,@ProcedureTypeId,@ExecutionPointId,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_FileRule_Select]...';


GO
CREATE PROCEDURE [dbo].[e_FileRule_Select]
	AS
	SELECT * FROM FileRule With(NoLock)
GO
PRINT N'Creating [dbo].[e_FileRuleMap_Delete_SourceFileByID]...';


GO
CREATE PROCEDURE [dbo].[e_FileRuleMap_Delete_SourceFileByID]
	@SourceFileID int
AS
	DELETE FileRuleMap WHERE SourceFileID = @SourceFileID
GO
PRINT N'Creating [dbo].[e_FileRuleMap_Save]...';


GO
CREATE PROCEDURE [dbo].[e_FileRuleMap_Save]
	@RulesAssignedID int,
	@RulesID int,
    @SourceFileID int,
	@RuleOrder int,
    @IsActive bit,
	@DateCreated datetime,
    @DateUpdated datetime,
    @CreatedByUserID int,
    @UpdatedByUserID int
AS
	
	IF @RulesAssignedID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE FileRuleMap
		SET RulesID = @RulesID,
			SourceFileID = @SourceFileID,
			@RuleOrder = @RuleOrder,
			IsActive = @IsActive,
			DateCreated = @DateCreated,
			DateUpdated = @DateUpdated,
			CreatedByUserID = @CreatedByUserID,
			UpdatedByUserID = @UpdatedByUserID
		WHERE RulesAssignedID = @RulesAssignedID;
		
		SELECT @RulesAssignedID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO FileRuleMap (RulesID,SourceFileID,RuleOrder,IsActive,DateCreated,CreatedByUserID)
		VALUES(@RulesID,@SourceFileID,@RuleOrder,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
PRINT N'Creating [dbo].[e_FileRuleMap_Select]...';


GO
CREATE PROCEDURE [dbo].[e_FileRuleMap_Select]
	AS
	SELECT * FROM FileRuleMap With(NoLock)
GO
PRINT N'Refreshing [dbo].[e_Client_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Client_Select]';


GO
PRINT N'Refreshing [dbo].[e_Client_Select_AccessKey]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Client_Select_AccessKey]';


GO
PRINT N'Refreshing [dbo].[e_Client_Select_ClientGroupID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Client_Select_ClientGroupID]';


GO
PRINT N'Refreshing [dbo].[e_Client_Select_ClientID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Client_Select_ClientID]';


GO
PRINT N'Refreshing [dbo].[e_Client_Select_ClientName]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Client_Select_ClientName]';


GO
PRINT N'Refreshing [dbo].[e_Client_Select_DefaultClient_AccessKey]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Client_Select_DefaultClient_AccessKey]';


GO
PRINT N'Refreshing [dbo].[e_Client_SelectActive_ClientGroupID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Client_SelectActive_ClientGroupID]';


GO
PRINT N'Refreshing [dbo].[e_ClientGroup_Select_ClientID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ClientGroup_Select_ClientID]';


GO
PRINT N'Refreshing [dbo].[e_SourceFile_Select_ClientName]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SourceFile_Select_ClientName]';


GO
PRINT N'Refreshing [dbo].[e_SourceFile_Select_SearchClientFileMapping]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SourceFile_Select_SearchClientFileMapping]';


GO
PRINT N'Refreshing [dbo].[e_Transformation_Select_ByClientAndName]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Transformation_Select_ByClientAndName]';


GO
PRINT N'Refreshing [dbo].[e_Transformation_Select_ClientName]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_Transformation_Select_ClientName]';


GO
PRINT N'Refreshing [dbo].[e_TransformationFieldMap_Delete]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_TransformationFieldMap_Delete]';


GO
PRINT N'Refreshing [dbo].[e_TransformationFieldMap_DeleteFieldMappingID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_TransformationFieldMap_DeleteFieldMappingID]';


GO
PRINT N'Refreshing [dbo].[o_HasServiceFeature_ClientID_ServiceName_FeatureName]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[o_HasServiceFeature_ClientID_ServiceName_FeatureName]';


GO
PRINT N'Refreshing [dbo].[rpt_TransformationCount_clientID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[rpt_TransformationCount_clientID]';


GO
PRINT N'Refreshing [dbo].[e_FileStatus_Save]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_FileStatus_Save]';


GO
PRINT N'Refreshing [dbo].[e_FileStatus_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_FileStatus_Select]';


GO
PRINT N'Refreshing [dbo].[e_FileStatus_Select_ClientID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_FileStatus_Select_ClientID]';


GO
PRINT N'Refreshing [dbo].[e_FileStatus_Select_FileStatusTypeID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_FileStatus_Select_FileStatusTypeID]';


GO
PRINT N'Refreshing [dbo].[e_FileStatus_Select_SourceFileID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_FileStatus_Select_SourceFileID]';


GO
PRINT N'Refreshing [dbo].[e_FileStatus_Select_SourceFileID_FileStatusTypeID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_FileStatus_Select_SourceFileID_FileStatusTypeID]';


GO
PRINT N'Refreshing [dbo].[e_FieldMapping_Select_ByClientSourceFile]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_FieldMapping_Select_ByClientSourceFile]';


GO
PRINT N'Refreshing [dbo].[e_FileLog_Select_ClientID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_FileLog_Select_ClientID]';


GO
PRINT N'Refreshing [dbo].[e_SourceFile_Delete_SourceFileByID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SourceFile_Delete_SourceFileByID]';


GO
PRINT N'Refreshing [dbo].[e_SourceFile_Select]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SourceFile_Select]';


GO
PRINT N'Refreshing [dbo].[e_SourceFile_Select_ClientID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SourceFile_Select_ClientID]';


GO
PRINT N'Refreshing [dbo].[e_SourceFile_Select_ClientID_IsDeleted]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SourceFile_Select_ClientID_IsDeleted]';


GO
PRINT N'Refreshing [dbo].[e_SourceFile_Select_IsDeleted]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SourceFile_Select_IsDeleted]';


GO
PRINT N'Refreshing [dbo].[e_SourceFile_Select_SourceFileID]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_SourceFile_Select_SourceFileID]';


GO
PRINT N'Refreshing [dbo].[e_ApiLog_Save]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_ApiLog_Save]';


GO
PRINT N'Refreshing [dbo].[e_UserAuthorizationLog_Save]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[e_UserAuthorizationLog_Save]';


GO
PRINT N'Checking existing data against newly created constraints';


GO
ALTER TABLE [dbo].[Transformation] WITH CHECK CHECK CONSTRAINT [FK_Transformation_Client_ClientID];

ALTER TABLE [dbo].[FieldMapping] WITH CHECK CHECK CONSTRAINT [FK_FieldMapping_SourceFile];

ALTER TABLE [dbo].[TransformationFieldMap] WITH CHECK CHECK CONSTRAINT [FK_TransFieldMap_SourceFile];

ALTER TABLE [dbo].[TransformationFieldMultiMap] WITH CHECK CHECK CONSTRAINT [FK_TransFieldMultiMap_SourceFile];


GO
PRINT N'Update complete.';


GO


UPDATE Menu 
SET IsActive = 1
WHERE MenuName LIKE 'Open Close'
go

DECLARE @mid int
DECLARE @openClosemid int

INSERT INTO UAS..Menu(ApplicationID, IsServiceFeature, ServiceFeatureID, MenuName, Description, IsParent, ParentMenuID, URL, IsActive, MenuOrder, HasFeatures, ImagePath, DateCreated,CreatedByUserID)
VALUES(2, 0, -1, 'Circulation Explorer', '', 1, -1, 'Circulation_Explorer.Modules.HomeDemo', 1, 7, 0, '', GETDATE(), 1); SET @mid = @@IDENTITY

SET @openClosemid = (SELECT MenuID FROM Menu WHERE MenuName LIKE 'Open Close')

DECLARE @groupID int = 1

WHILE @groupID < 5
BEGIN

	if not exists (Select MenuSecurityGroupMapID FROM MenuSecurityGroupMap WHERE MenuID = @mid AND SecurityGroupID = @groupID)
	BEGIN
		INSERT INTO UAS..MenuSecurityGroupMap(SecurityGroupID, MenuID, HasAccess, IsActive, DateCreated, CreatedByUserID)
		VALUES(@groupID, @mid, 1, 1, GETDATE(), 1)
	END
	
	if not exists (Select MenuSecurityGroupMapID FROM MenuSecurityGroupMap WHERE MenuID = @openClosemid AND SecurityGroupID = @groupID)
	BEGIN
		INSERT INTO UAS..MenuSecurityGroupMap(SecurityGroupID, MenuID, HasAccess, IsActive, DateCreated, CreatedByUserID)
		VALUES(@groupID, @openClosemid, 1, 1, GETDATE(), 1)
	END
	
	SET @groupID = @groupID + 1	
END
go
