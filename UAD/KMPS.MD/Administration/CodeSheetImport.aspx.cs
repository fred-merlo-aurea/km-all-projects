using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using KMPS.MD.Objects;
using Microsoft.VisualBasic.FileIO;

namespace KMPS.MD.Administration
{
    public partial class CodesheetImport : System.Web.UI.Page
    {
        private const int MaxCollectionSize = 5000;

        private const string CodesheetImportAndMappingValue = "CODESHEET IMPORT AND MAPPING";
        private const string ProductImportValue = "PRODUCT IMPORT";
        private const string TxtExtension = ".txt";

        private const string CannotUploadFileErrorMessage =
            "ERROR - Cannot Upload File: <br><br>Only files with an extension of .txt are supported.";

        private const string FileCannotBeUploadedMessage = "File cannot be uploaded.</br></br>";
        private const string FileSuccessfullyUploadedMessage = "File successfully uploaded.";
        private const string UploadingErrorMessage = "An error has occured uploading your file.<br /><br />";
        private const string UploadingYourFileErrorMessage = "An error has occured uploading your file.<br /><br />";

        private const string IncorrectPubHeadersMessage =
            "The headers for your txt file are incorrect. The correct format is PubName,PubCode,IsTradeShow,PubType,EnableSearching,Score,YearStartDate,YearEndDate,IssueDate,IsImported,IsActive,AllowDataEntry,KMImportAllowed,ClientImportAllowed,AddRemoveAllowed,IsUAD,IsCirc,IsOpenCloseLocked,HasPaidRecords,UseSubGen,Brand";

        private const string IncorrectResponseHeadersMessage =
            "The headers for your txt file are incorrect. The correct format is PubCode,ResponseGroupName,ResponseGroup_DisplayName,ResponseValue,ResponseDesc,MasterValue,MasterDesc,MasterGroupName";

        private const string IncorrectPersonalHeadersMessage =
            "The headers for your txt file are incorrect. The correct format is Name,DisplayName,IsActive,EnableSubReporting,EnableSearching,EnableAdhocSearch";

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Products";
            Master.SubMenu = "Import and Mapping";
            divError.Visible = false;
            lblErrorMessage.Text = "";
            divMessage.Visible = false;
            lblMessage.Text = "";
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileSelector.PostedFile.FileName.EndsWith(TxtExtension, StringComparison.OrdinalIgnoreCase))
            {
                if (ValidateUploadTxtHeaders(FileSelector.PostedFile.InputStream))
                {
                    switch (drpImport.SelectedValue.ToUpper())
                    {
                        case ProductImportValue:
                            TryToImportProductItem();
                            break;
                        case CodesheetImportAndMappingValue:
                            TryToImportCodeSheetItem();
                            break;
                        default:
                            TryToImportMasterGroupItem();
                            break;
                    }
                }
                else
                {
                    SetLabelErrorMessage();
                    divError.Visible = true;
                }
            }
            else
            {
                lblErrorMessage.Text = CannotUploadFileErrorMessage;
                divError.Visible = true;
            }
        }

        private void TryToImportProductItem()
        {
            var tupData = SerializeTxtFile(FileSelector.PostedFile.InputStream, "productlist", "product", MaxCollectionSize);

            try
            {
                Pubs.Import(Master.clientconnections, tupData.Item1, Master.LoggedInUser);
            }
            // Must catch all exceptions and set label error message
            catch (Exception ex)
            {
                lblErrorMessage.Text = $"{UploadingErrorMessage}{ex.Message}";
                divError.Visible = true;
                return;
            }

            lblMessage.Text = FileSuccessfullyUploadedMessage;
            divMessage.Visible = true;
        }

        private void TryToImportCodeSheetItem()
        {
            try
            {
                var tupData = SerializeTxtFile(FileSelector.PostedFile.InputStream, "codesheetlist", "codesheet", MaxCollectionSize);

                if (tupData.Item2 == string.Empty)
                {
                    var nvc = CodeSheet.Import(Master.clientconnections, tupData.Item1, Master.LoggedInUser);

                    if (nvc.Count > 0)
                    {
                        var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });

                        var errorMsg = items.Aggregate(
                            FileCannotBeUploadedMessage, 
                            (current, item) => current + $"{item.key} {item.value}</br>");

                        lblErrorMessage.Text = errorMsg;
                        divError.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblErrorMessage.Text = tupData.Item2;
                    divError.Visible = true;
                    return;
                }
            }
            // Must catch all exceptions and set label error message
            catch (Exception ex)
            {
                lblErrorMessage.Text = $"{UploadingYourFileErrorMessage}{ex.Message}";
                divError.Visible = true;
                return;
            }

            lblMessage.Text = FileSuccessfullyUploadedMessage;
            divMessage.Visible = true;
        }

        private void TryToImportMasterGroupItem()
        {
            var tupData = SerializeTxtFile(FileSelector.PostedFile.InputStream, "mastergrouplist", "mastergroup", MaxCollectionSize);

            try
            {
                if (tupData.Item2 == string.Empty)
                {
                    var collection = MasterGroup.Import(Master.clientconnections, tupData.Item1, Master.LoggedInUser);

                    if (collection.Count > 0)
                    {
                        var items = collection.AllKeys.SelectMany(collection.GetValues, (k, v) => new { key = k, value = v });

                        var errorMsg = items.Aggregate(
                            FileCannotBeUploadedMessage,
                            (current, item) => current + $"{item.key} {item.value}</br>");

                        lblErrorMessage.Text = errorMsg;
                        divError.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblErrorMessage.Text = tupData.Item2;
                    divError.Visible = true;
                    return;
                }
            }
            // Must catch all exceptions and set label error message
            catch (Exception ex)
            {
                lblErrorMessage.Text = $"{UploadingYourFileErrorMessage}{ex.Message}";
                divError.Visible = true;
                return;
            }

            lblMessage.Text = FileSuccessfullyUploadedMessage;
            divMessage.Visible = true;
        }

        private void SetLabelErrorMessage()
        {
            switch (drpImport.SelectedValue.ToUpper())
            {
                case ProductImportValue:
                    lblErrorMessage.Text = IncorrectPubHeadersMessage;
                    break;
                case CodesheetImportAndMappingValue:
                    lblErrorMessage.Text = IncorrectResponseHeadersMessage;
                    break;
                default:
                    lblErrorMessage.Text = IncorrectPersonalHeadersMessage;
                    break;
            }
        }

        private bool ValidateUploadTxtHeaders(Stream stream)
        {
            bool isValid = false;

            MemoryStream memoryStream = new MemoryStream();

            try
            {
                stream.CopyTo(memoryStream);
                memoryStream.Position = 0;

                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    string headerString = reader.ReadLine();

                    if (drpImport.SelectedValue.ToUpper() == "PRODUCT IMPORT")
                        isValid = string.Compare(headerString, "PubName\tPubCode\tIsTradeShow\tPubType\tEnableSearching\tScore\tYearStartDate\tYearEndDate\tIssueDate\tIsImported\tIsActive\tAllowDataEntry\tKMImportAllowed\tClientImportAllowed\tAddRemoveAllowed\tIsUAD\tIsCirc\tIsOpenCloseLocked\tHasPaidRecords\tUseSubGen\tBrand", true) == 0;
                    else if (drpImport.SelectedValue.ToUpper() == "CODESHEET IMPORT AND MAPPING")
                        isValid = string.Compare(headerString, "PubCode\tResponseGroupName\tResponseGroup_DisplayName\tResponseValue\tResponseDesc\tMasterValue\tMasterDesc\tMasterGroupName", true) == 0;
                    else
                        isValid = string.Compare(headerString, "Name\tDisplayName\tIsActive\tEnableSubReporting\tEnableSearching\tEnableAdhocSearch", true) == 0;
                }
            }
            finally
            {
                stream.Position = 0;

                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }
            }

            return isValid;
        }

        public static Tuple<XDocument, string> SerializeTxtFile(Stream stream, string rootNode, string itemNode, int maxCollectionSize)
        {
            stream.Position = 0;
            XDocument xDoc = new XDocument(new XElement(rootNode));
            string re = @"[(),]";
            string strInValidName = string.Empty;
            string strInValidDisplayName = string.Empty;
            string strError = string.Empty;

            using (TextFieldParser parser = new TextFieldParser(stream))
            {
                parser.HasFieldsEnclosedInQuotes = true;
                string[] delimiters = { ",", "\t" };
                parser.SetDelimiters(delimiters);

                string[] headerValues = parser.ReadFields();

                string[] headerValuesLower = (from str in headerValues
                                              select str.ToLower()).ToArray();

                while (!parser.EndOfData)
                {
                    int collectionItemCount = 0;

                    while ((collectionItemCount < maxCollectionSize || maxCollectionSize <= 0)
                        && !parser.EndOfData)
                    {
                        string[] TxtData = parser.ReadFields();

                        if(itemNode =="mastergroup")
                        {
                            if (Regex.IsMatch(TxtData[0], re))
                            {
                                strInValidName += (strInValidName == "" ? "" : ",") + TxtData[0];
                            }

                            if (Regex.IsMatch(TxtData[1], re))
                            {
                                strInValidDisplayName += (strInValidDisplayName == "" ? "" : ",") + TxtData[1];
                            }
                        }
                        else if (itemNode == "codesheet")
                        {
                            if (Regex.IsMatch(TxtData[1], re))
                            {
                                strInValidName += (strInValidName == "" ? "" : ",") + TxtData[1];
                            }

                            if (Regex.IsMatch(TxtData[2], re))
                            {
                                strInValidDisplayName += (strInValidDisplayName == "" ? "" : ",") + TxtData[2];
                            }
                        }

                        xDoc.Root.Add(BuildElement(headerValuesLower, TxtData, itemNode));
                        collectionItemCount++;
                    }
                }
            }

            if (strInValidName != "" && strInValidDisplayName != "")
                strError = "Name and DisplayName contain invalid character";
            else if (strInValidName != "")
                strError = "Name contain invalid character";
            else if (strInValidDisplayName != "")
                strError = "Display Name contain invalid character";

            return Tuple.Create(xDoc, strError);
        }

        private static XElement BuildElement(string[] headerValues, string[] TxtData, string itemNode)
        {
            XElement newItemElement = new XElement(itemNode);
            string re = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";

            if (TxtData.Length == headerValues.Length)
            {
                for (int i = 0; i < headerValues.Length; i++)
                {
                    newItemElement.Add(new XElement(headerValues[i], Regex.Replace(TxtData[i], re, "")));
                }
            }

            return newItemElement;
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            if (drpImport.SelectedValue.ToUpper() == "PRODUCT IMPORT")
                UploadTxtFileLabel.Text = string.Concat("Product Import: ", "");
            else if (drpImport.SelectedValue.ToUpper() == "CODESHEET IMPORT AND MAPPING")
                UploadTxtFileLabel.Text = string.Concat("Codesheet Import and Mapping: ", "");
            else
                UploadTxtFileLabel.Text = string.Concat("MasterGroup Import: ", "");
            mdlPopupTxt.Show();
        }
    }
}