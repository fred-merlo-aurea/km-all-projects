using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ecn.communicator.main.Salesforce.SF_Pages.Converters
{
    public class ActionConverter
    {
        private const string ActionColumn = "Action";
        private const string TotalsColumn = "Totals";
        private const string SortOrderColumn = "sortOrder";

        private Dictionary<string, ActionModel> _actionMapping = new Dictionary<string, ActionModel>();

        public ActionConverter()
        {
            InitMapping();
        }

        public void AddOrOverrideMapping(string key, ActionModel model)
        {
            if (_actionMapping.ContainsKey(key))
            {
                _actionMapping[key] = model;
            }
            else
            {
                _actionMapping.Add(key, model);
            }
        }

        public DataTable ConvertToView(DataTable dtResults)
        {
            var actions = GetActions(dtResults);
            return ConvertCore(actions);
        }

        public DataTable ConvertToView(Hashtable hUpdatedRecords)
        {
            var actions = GetActions(hUpdatedRecords);
            return ConvertCore(actions);
        }

        private DataTable ConvertCore(IDictionary<string, int> actions)
        {
            DataTable table = null;
            if (actions.Count > 0)
            {
                table = SetupDataTable(ActionColumn, TotalsColumn, SortOrderColumn);

                foreach (var action in actions)
                {
                    var row = table.NewRow();
                    var model = GetActionModel(action.Key);
                    if (model != null)
                    {
                        row[ActionColumn] = model.Name;
                        row[SortOrderColumn] = model.SortOrder;
                    }
                    row[TotalsColumn] = action.Value;
                    table.Rows.Add(row);
                }
                AddLastRow(table);
            }

            return table;
        }

        private void AddLastRow(DataTable table)
        {
            const string LastAction = "&nbsp;";
            const string LastTotals = " ";
            const int LastSortOrder = 8;

            var row = table.NewRow();
            row[ActionColumn] = LastAction;
            row[TotalsColumn] = LastTotals;
            row[SortOrderColumn] = LastSortOrder;

            table.Rows.Add(row);
        }

        private IDictionary<string, int> GetActions(DataTable table)
        {
            var result = new Dictionary<string, int>();
            if (table.Rows.Count > 0)
            {
                const string ActionColumn = "Action";
                const string CountsColumn = "Counts";

                foreach (DataRow row in table.Rows)
                {
                    var action = row[ActionColumn].ToString();
                    var counts = Convert.ToInt32(row[CountsColumn]);

                    if (!result.ContainsKey(action))
                    {
                        result.Add(action, counts);
                    }
                    else
                    {
                        result[action] += counts;
                    }
                }
            }
            return result;
        }

        private IDictionary<string, int> GetActions(Hashtable dict)
        {
            var result = new Dictionary<string, int>();
            foreach (string key in dict.Keys)
            {
                var count = Convert.ToInt32(dict[key]);
                result.Add(key, count);
            }

            return result;
        }

        private ActionModel GetActionModel(string actionKey)
        {
            var key = actionKey.ToUpperInvariant();

            if (_actionMapping.ContainsKey(key))
            {
                return _actionMapping[key];
            }
            return null;
        }

        private DataTable SetupDataTable(params string[] columns)
        {
            var dataTable = new DataTable();
            foreach (var column in columns)
            {
                dataTable.Columns.Add(column);
            }

            return dataTable;
        }

        private void InitMapping()
        {
            _actionMapping.Add("T", new ActionModel("Total Records in the Import", 1));
            _actionMapping.Add("I", new ActionModel("New", 2));
            _actionMapping.Add("U", new ActionModel("Changed", 3));
            _actionMapping.Add("D", new ActionModel("Duplicate(s)", 4));
            _actionMapping.Add("S", new ActionModel("Skipped", 5));
            _actionMapping.Add("M", new ActionModel("Skipped (Emails in Master Suppression)", 6));
        }
    }
}