using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ExCSS;
using KMEntities;
using KMEnums;

namespace KMModels.PostModels
{
    public class FormStylesPostModel : PostModelBase
    {
        private readonly static string[] ControlTypes = Enum.GetNames(typeof(StyledControlType));
        private const string kmPrefix = ".km";
        private const string body = "body";
        private const string kmForm = ".kmForm";
        private const string kmItem = ".km_item";
        private const string kmHint = ".km_hint";
        private const string f_l = "fieldset legend";
        private const string input = "input";
        private const string grid_table = ".kmGrid table";
        private const string color = "color";
        private const string background = "background";
        private const string border = "border";
        private const string border_color = "border-color";
        private const string solid = "solid";
        private const string text = "text";
        private const string font = "font";
        private const string family = "family";
        private const string size = "size";
        private const string weight = "weight";
        private const string bold = "bold";
        private const string normal = "normal";
        private const string s_float = "float";
        private const string text_align = "text-align";
        private const string padding_bottom = "padding-bottom";
        private const string width = "width";
        private const string left = "left";
        private const string right = "right";
        private const string center = "center";
        private const string none = "none";
        private const string margin = "margin";
        private const string bottom = "bottom";
        private const string auto = "auto";
        private const string display = "display";
        private const string block = "block";
        private const string inline_block = "inline-block";
        private const string label = "label";
        private const string category = ".category";
        private const string li_ = "li>";
        private const string ul_li = "ul li";
        private const string ul_with_li = "ul>li";
        private const string li_div_label = "li>div>label";
        private const string option = "option";
        private const string span = "span";
        private const string div = "div";
        private const string table = "table";
        private const string td = "td";
        private const string s_clear = "clear";
        private const string both = "both";

        private const string RgbaPattern = "^rgba\\(([0-9]+),\\s?([0-9]+),\\s?([0-9]+),\\s?([01]\\.?[0-9]*)\\)$";
        private const string RgbaFormat = "rgba({0}, {1}, {2}, {3})";
        private static Regex RgbaRex = new Regex(RgbaPattern);
        private static HtmlColor transparentColor = HtmlColor.FromRgba(0, 0, 0, 0);

        private string margin_bottom
        {
            get
            {
                return margin + '-' + bottom;
            }
        }

        private string background_color
        {
            get
            {
                return background + '-' + color;
            }
        }

        private string font_family
        {
            get
            {
                return font + '-' + family;
            }
        }

        private string font_weight
        {
            get
            {
                return font + '-' + weight;
            }
        }

        private string font_size
        {
            get
            {
                return font + '-' + size;
            }
        }

        [GetFromField("Form_Seq_ID")]
        public int Id { get; set; }

        public StylingType StylingType { get; set; }

        [GetFromField("CssUri")]
        public string ExternalUrl { get; set; }

        public CssFile File { get; set; }

        public CustomStyles CustomStyles { get; set; }

        public override void FillData(object entity)
        {
            var styleSheet = entity as StyleSheet;
            if (styleSheet != null)
            {
                CustomStyles = new CustomStyles();
                CustomStyles.FormStyles = new FormStyles(true);
                CustomStyles.ControlsStyles = new Dictionary<StyledControlType, ControlStyles>();

                foreach (var styleRule in styleSheet.StyleRules)
                {
                    var ruleSelectors = styleRule.Value.Split(',');

                    foreach (var selector in ruleSelectors)
                    {
                        switch (selector)
                        {
                            case body:
                                FillBodyStyles(CustomStyles, styleRule);
                                break;
                            case kmForm:
                                FillKmFormStyles(CustomStyles, styleRule);
                                break;
                            case kmForm + " " + ul_with_li:
                                FillUnorderedListItemsAlignmentStyles(CustomStyles, styleRule);
                                break;
                            case kmForm + " " + ul_li:
                                FillUnorderedListItemsSpacingStyles(CustomStyles, styleRule);
                                break;
                            default:
                                FillControlStyles(CustomStyles, styleSheet, selector, styleRule);
                                break;
                        }
                    }
                }

                FillButtonProperties(CustomStyles);
            }
            else
            {
                base.FillData(entity);
                if (ExternalUrl != null && StylingType != StylingType.External)
                {
                    StylingType = StylingType.External;
                }

                var entityAsForm = entity as Form;
                if (StylingType == StylingType.Upload && entityAsForm != null)
                {
                    File = new CssFile();
                    File.Name = entityAsForm.CssFile.Name;
                    File.UID = entityAsForm.CssFile.CssFileUID;
                }
            }
        }

        private ControlStyles GetDefStyles(StyleSheet sheet)
        {
            var defStyle = default(ControlStyles);
            var defRule = sheet.StyleRules.SingleOrDefault(x => x.Value == kmForm + ' ' + li_div_label);

            if (defRule != null)
            {
                defStyle = new ControlStyles(true);
                foreach (var styleProperty in defRule.Declarations)
                {
                    var propertyName = styleProperty.Name;
                    var propertyValue = styleProperty.Term.ToString();

                    if (propertyName.Equals(color, StringComparison.OrdinalIgnoreCase))
                    {
                        defStyle.LabelTextColor = GetColor(styleProperty.Term);
                    }

                    if (propertyName.Equals(background_color, StringComparison.OrdinalIgnoreCase))
                    {
                        defStyle.LabelBackgroundColor = GetColor(styleProperty.Term);
                    }

                    if (propertyName.IndexOf(font, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        if (propertyName.Equals(font, StringComparison.OrdinalIgnoreCase))
                        {
                            var termList = styleProperty.Term as TermList;
                            if (termList != null)
                            {
                                foreach (var term in termList)
                                {
                                    CheckTermForFont(term, defStyle);
                                }
                            }
                            else
                            {
                                CheckTermForFont(styleProperty.Term, defStyle);
                            }
                        }

                        if (propertyName.IndexOf(family, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            defStyle.LabelFont = propertyValue;
                        }

                        if (propertyName.IndexOf(size, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            var primitiveTerm = styleProperty.Term as PrimitiveTerm;
                            if (primitiveTerm != null)
                            {
                                int labelFontSize;
                                defStyle.LabelFontSize =
                                    int.TryParse(primitiveTerm.Value.ToString(), out labelFontSize)
                                    ? labelFontSize
                                    : defStyle.LabelFontSize;
                            }
                        }

                        if (propertyName.IndexOf(weight, StringComparison.OrdinalIgnoreCase) > -1 &&
                            propertyValue.Equals(bold, StringComparison.OrdinalIgnoreCase))
                        {
                            defStyle.LabelFontBold = true;
                        }
                    }
                }
            }

            return defStyle;
        }

        private void FillBodyStyles(CustomStyles customStyles, StyleRule styleRule)
        {
            var backgroundColorProperty = styleRule.Declarations
                                    .FirstOrDefault(x => x.Name.Equals(background_color, StringComparison.OrdinalIgnoreCase));
            if (backgroundColorProperty != null)
            {
                customStyles.FormStyles.BackgroundColor = GetColor(backgroundColorProperty.Term);
            }
        }

        private void FillKmFormStyles(CustomStyles customStyles, StyleRule styleRule)
        {
            foreach (var styleProperty in styleRule.Declarations)
            {
                var propertyName = styleProperty.Name;
                if (propertyName.IndexOf(color, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    if (propertyName.IndexOf(background, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        customStyles.FormStyles.Color = GetColor(styleProperty.Term);
                    }

                    if (propertyName.IndexOf(border_color, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        customStyles.FormStyles.BorderColor = GetColor(styleProperty.Term);

                        var borderColorValue = FromRgba(customStyles.FormStyles.BorderColor);
                        customStyles.FormStyles.Border = !borderColorValue.ToString()
                            .Equals(transparentColor.ToString(), StringComparison.OrdinalIgnoreCase);
                    }
                }

                if (propertyName.Equals(border, StringComparison.OrdinalIgnoreCase))
                {
                    var termList = styleProperty.Term as TermList;
                    if (termList != null)
                    {
                        foreach (var term in termList)
                        {
                            if (CheckTermForBorderColor(term, customStyles.FormStyles))
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        CheckTermForBorderColor(styleProperty.Term, customStyles.FormStyles);
                    }
                }
            }
        }

        private void FillUnorderedListItemsAlignmentStyles(CustomStyles customStyles, StyleRule styleRule)
        {
            var align = styleRule.Declarations
                .FirstOrDefault(x => x.Name.Equals(text_align, StringComparison.OrdinalIgnoreCase));
            if (align != null)
            {
                var alignValue = align.Term.ToString();

                if (alignValue == left)
                {
                    customStyles.FormStyles.Alignment = Alignment.Left;
                }
                else if (alignValue == right)
                {
                    customStyles.FormStyles.Alignment = Alignment.Right;
                }
            }
        }

        private void FillUnorderedListItemsSpacingStyles(CustomStyles customStyles, StyleRule styleRule)
        {
            var spacing = styleRule.Declarations
                .FirstOrDefault(x => x.Name.Equals(margin_bottom, StringComparison.OrdinalIgnoreCase));
            if (spacing != null)
            {
                var primitiveTerm = spacing.Term as PrimitiveTerm;
                if (primitiveTerm != null)
                {
                    int primitiveTermValue;
                    customStyles.FormStyles.Spacing =
                        int.TryParse(primitiveTerm.Value.ToString(), out primitiveTermValue)
                        ? primitiveTermValue
                        : customStyles.FormStyles.Spacing;
                }
            }
        }

        private void FillControlStyles(CustomStyles customStyles, StyleSheet styleSheet, string selector, StyleRule styleRule)
        {
            var styledControlType = GetControlType(selector);
            if (styledControlType.HasValue)
            {
                bool allowSetColor;
                bool allowSetOther;
                bool allowSetBorder;

                if (!DecideAllowings(styledControlType.Value, selector,
                    out allowSetColor, out allowSetOther, out allowSetBorder))
                {
                    return;
                }

                var controlStyles = default(ControlStyles);
                if (customStyles.ControlsStyles.ContainsKey(styledControlType.Value))
                {
                    controlStyles = customStyles.ControlsStyles[styledControlType.Value];
                }
                if (controlStyles == null)
                {
                    var defStyle = GetDefStyles(styleSheet);
                    controlStyles = new ControlStyles(defStyle);
                    customStyles.ControlsStyles.Add(styledControlType.Value, controlStyles);
                }
                foreach (var styleProperty in styleRule.Declarations)
                {
                    if (selector.EndsWith(label, StringComparison.OrdinalIgnoreCase) ||
                        styledControlType == StyledControlType.Button)
                    {
                        FillLabelControlStyles(controlStyles, styleProperty);
                    }
                    if (selector.EndsWith(category, StringComparison.OrdinalIgnoreCase))
                    {
                        FillCategoryControlStyles(controlStyles, styleProperty);
                    }
                    if (!selector.EndsWith(label, StringComparison.OrdinalIgnoreCase) &&
                        styledControlType != StyledControlType.Button &&
                        allowSetColor)
                    {
                        SetFont(styleProperty.Name, controlStyles, styleProperty.Term);
                        SetFontSize(styleProperty.Name, controlStyles, styleProperty.Term);
                        SetFontBold(styleProperty.Name, controlStyles, styleProperty.Term);
                    }
                    if (!selector.EndsWith(label, StringComparison.OrdinalIgnoreCase) ||
                        styledControlType == StyledControlType.Button)
                    {
                        SetColor(styleProperty.Name, allowSetColor, controlStyles, styleProperty.Term);
                        if (allowSetOther &&
                            styleProperty.Name.IndexOf(color, StringComparison.OrdinalIgnoreCase) > -1 &&
                            styleProperty.Name.IndexOf(background, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            controlStyles.BackgroundColor = GetColor(styleProperty.Term);
                        }
                        if (allowSetBorder &&
                            styleProperty.Name.Equals(border, StringComparison.OrdinalIgnoreCase))
                        {
                            var termList = styleProperty.Term as TermList;
                            if (termList != null)
                            {
                                foreach (var term in termList)
                                {
                                    if (CheckTermForBorderColor(term, controlStyles))
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                CheckTermForBorderColor(styleProperty.Term, controlStyles);
                            }
                        }
                    }
                }
            }
        }

        private bool DecideAllowings(StyledControlType styledControlType, string selector, out bool allowSetColor, out bool allowSetOther, out bool allowSetBorder)
        {
            allowSetColor = true;
            allowSetOther = true;
            allowSetBorder = false;
            switch (styledControlType)
            {
                case StyledControlType.DropDown:
                case StyledControlType.ListBox:
                    allowSetColor = selector.EndsWith(option, StringComparison.OrdinalIgnoreCase);
                    allowSetOther = !allowSetColor;
                    break;
                case StyledControlType.Grid:
                    allowSetColor = selector.EndsWith(td, StringComparison.OrdinalIgnoreCase);
                    allowSetOther = selector.EndsWith(table, StringComparison.OrdinalIgnoreCase);
                    break;
                case StyledControlType.RadioButton:
                case StyledControlType.CheckBox:
                case StyledControlType.NewsLetter:
                    allowSetColor = selector.EndsWith(span, StringComparison.OrdinalIgnoreCase);
                    allowSetOther = selector.EndsWith(div, StringComparison.OrdinalIgnoreCase);
                    break;
                case StyledControlType.Button:
                    if (selector.EndsWith(GetCssClassByControlType(StyledControlType.Button), StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    break;
            }
            allowSetBorder = allowSetOther;
            if (IsGroupControl(styledControlType))
            {
                if (styledControlType == StyledControlType.CheckBox ||
                    styledControlType == StyledControlType.RadioButton ||
                    styledControlType == StyledControlType.NewsLetter)
                {
                    allowSetBorder = selector.EndsWith(">" + div, StringComparison.OrdinalIgnoreCase);
                }
                else if (styledControlType == StyledControlType.Grid)
                {
                    allowSetBorder = selector.EndsWith(">" + table, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    allowSetBorder = selector.EndsWith(GetCssClassByControlType(styledControlType), StringComparison.OrdinalIgnoreCase);
                }

                if (allowSetBorder)
                {
                    allowSetColor = false;
                    allowSetOther = false;
                }
            }

            return true;
        }

        private void FillButtonProperties(CustomStyles customStyles)
        {
            var buttonStyles = customStyles.ControlsStyles[StyledControlType.Button];
            customStyles.ButtonStyles = new ButtonStyles();
            customStyles.ButtonStyles.BackgroundColor = buttonStyles.BackgroundColor;
            customStyles.ButtonStyles.BorderColor = buttonStyles.BorderColor;
            customStyles.ButtonStyles.Color = buttonStyles.TextColor;
            customStyles.ButtonStyles.Font = buttonStyles.LabelFont;
            customStyles.ButtonStyles.FontBold = buttonStyles.LabelFontBold;
            customStyles.ButtonStyles.FontSize = buttonStyles.LabelFontSize;
        }

        private void FillLabelControlStyles(ControlStyles controlStyles, Property styleProperty)
        {
            var propertyName = styleProperty.Name;
            var propertyValue = styleProperty.Term.ToString();

            if (propertyName.Equals(color, StringComparison.OrdinalIgnoreCase))
            {
                controlStyles.LabelTextColor = GetColor(styleProperty.Term);
            }

            if (propertyName.Equals(background_color, StringComparison.OrdinalIgnoreCase))
            {
                controlStyles.LabelBackgroundColor = GetColor(styleProperty.Term);
            }

            if (propertyName.IndexOf(font, StringComparison.OrdinalIgnoreCase) > -1)
            {
                if (propertyName.Equals(font, StringComparison.OrdinalIgnoreCase))
                {
                    var termList = styleProperty.Term as TermList;
                    if (termList != null)
                    {
                        foreach (var term in termList)
                        {
                            CheckTermForFont(term, controlStyles);
                        }
                    }
                    else
                    {
                        CheckTermForFont(styleProperty.Term, controlStyles);
                    }
                }
                if (propertyName.IndexOf(family, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    controlStyles.LabelFont = propertyValue;
                }
                if (propertyName.IndexOf(size, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    var primitiveTerm = styleProperty.Term as PrimitiveTerm;
                    if (primitiveTerm != null)
                    {
                        int labelFontSize;
                        controlStyles.LabelFontSize =
                            int.TryParse(primitiveTerm.Value.ToString(), out labelFontSize)
                            ? labelFontSize
                            : controlStyles.LabelFontSize;
                    }
                }
                if (propertyName.IndexOf(weight, StringComparison.OrdinalIgnoreCase) > -1 &&
                    propertyValue.Equals(bold, StringComparison.OrdinalIgnoreCase))
                {
                    controlStyles.LabelFontBold = true;
                }
            }
        }

        private void FillCategoryControlStyles(ControlStyles controlStyles, Property styleProperty)
        {
            var propertyName = styleProperty.Name;
            var propertyValue = styleProperty.Term.ToString();

            if (propertyName.Equals(color, StringComparison.OrdinalIgnoreCase))
            {
                controlStyles.CategoryTextColor = GetColor(styleProperty.Term);
            }

            if (propertyName.Equals(background_color, StringComparison.OrdinalIgnoreCase))
            {
                controlStyles.CategoryBackgroundColor = GetColor(styleProperty.Term);
            }

            if (propertyName.IndexOf(font, StringComparison.OrdinalIgnoreCase) > -1)
            {
                if (propertyName.Equals(font, StringComparison.OrdinalIgnoreCase))
                {
                    if (styleProperty.Term is TermList)
                    {
                        var termList = styleProperty.Term as TermList;
                        foreach (var term in termList)
                        {
                            var primitiveTerm = term as PrimitiveTerm;
                            FillCategoryControlFontStyles(controlStyles, primitiveTerm);
                        }
                    }
                    else if (styleProperty.Term is PrimitiveTerm)
                    {
                        var primitiveTerm = styleProperty.Term as PrimitiveTerm;
                        FillCategoryControlFontStyles(controlStyles, primitiveTerm);
                    }
                }

                if (propertyName.IndexOf(family, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    controlStyles.CategoryFont = propertyValue;
                }

                if (propertyName.IndexOf(size, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    var primitiveTerm = styleProperty.Term as PrimitiveTerm;
                    if (primitiveTerm != null)
                    {
                        int primitiveTermValue;
                        controlStyles.CategoryFontSize =
                            int.TryParse(primitiveTerm.Value.ToString(), out primitiveTermValue)
                            ? primitiveTermValue
                            : controlStyles.CategoryFontSize;

                    }
                }

                if (propertyName.IndexOf(weight, StringComparison.OrdinalIgnoreCase) > -1 &&
                    propertyValue.Equals(bold, StringComparison.OrdinalIgnoreCase))
                {
                    controlStyles.CategoryFontBold = true;
                }
            }
        }

        private void FillCategoryControlFontStyles(ControlStyles controlStyles, PrimitiveTerm primitiveTerm)
        {
            if (primitiveTerm != null)
            {
                var primitiveTermValue = primitiveTerm.Value.ToString();
                if (primitiveTerm.PrimitiveType == UnitType.Pixel ||
                    primitiveTerm.PrimitiveType == UnitType.Number)
                {
                    int categoryFontSize;
                    controlStyles.CategoryFontSize =
                        int.TryParse(primitiveTermValue, out categoryFontSize)
                        ? categoryFontSize
                        : controlStyles.CategoryFontSize;
                }
                else
                {
                    if (primitiveTermValue.Equals(bold, StringComparison.OrdinalIgnoreCase) ||
                        primitiveTermValue.Equals(normal, StringComparison.OrdinalIgnoreCase))
                    {
                        controlStyles.CategoryFontBold = primitiveTermValue.Equals(bold, StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        controlStyles.CategoryFont = primitiveTermValue;
                    }
                }
            }
        }

        public string RewriteCss(string css)
        {
            var cssParser = new Parser();
            var sheet = cssParser.Parse(css);

            RewriteTextAlign(sheet);
            RewriteFormRules(sheet);
            RewriteBodyRules(sheet);
            RewriteKmFormListItemRules(sheet);

            CopyButtonRulesToControlStyles();
            
            foreach (var item in CustomStyles.ControlsStyles)
            {
                RewriteControlLabelRule(sheet, item.Key, item.Value);
                RewriteControlCategoryRule(sheet, item.Key, item.Value);
                RewriteControlColorRule(sheet, item.Key, item.Value);
                RewriteControlRule(sheet, item.Key, item.Value);
                RewriteControlBorderRule(sheet, item.Key, item.Value);
            }

            return sheet.ToString();
        }

        private void RewriteTextAlign(StyleSheet sheet)
        {
            RewriteTextAlignForFormUnorderedList(sheet);
            RewriteTextAlignForGridTable(sheet);
            RewriteTextAlignForKmItemRule(sheet);
            RewriteTextAlignForToggleButtons(sheet);
            RewriteTextAlignForHintRule(sheet);
            RewriteTextAlignForFieldset(sheet);
            RewriteTextAlignForButton(sheet);
        }

        private void RewriteTextAlignForFormUnorderedList(StyleSheet sheet)
        {
            var form_ul_with_li_Rule = sheet.StyleRules.Single(x => x.Value == string.Format("{0} {1}", kmForm, ul_with_li));
            SetProperty(form_ul_with_li_Rule, text_align, new PrimitiveTerm(UnitType.Ident, CustomStyles.FormStyles.Alignment.ToString().ToLower()));
        }

        private void RewriteTextAlignForGridTable(StyleSheet sheet)
        {
            const int marginZero = 0;

            var grid_table_Rule = sheet.StyleRules.Single(x => x.Value == string.Format("{0} {1}", kmForm, grid_table));
            var marginProperty = grid_table_Rule.Declarations.FirstOrDefault(x => x.Name.Equals(margin, StringComparison.OrdinalIgnoreCase));
            var gt_displayProperty = grid_table_Rule.Declarations.FirstOrDefault(x => x.Name.Equals(display, StringComparison.OrdinalIgnoreCase));

            RemoveProperty(grid_table_Rule, marginProperty);
            RemoveProperty(grid_table_Rule, gt_displayProperty);

            switch (CustomStyles.FormStyles.Alignment)
            {
                case Alignment.Left:
                case Alignment.Right:
                    gt_displayProperty = new Property(display);
                    gt_displayProperty.Term = new PrimitiveTerm(UnitType.Ident, inline_block);
                    grid_table_Rule.Declarations.Add(gt_displayProperty);
                    break;
                case Alignment.Center:
                    marginProperty = new Property(margin);
                    marginProperty.Term = SetMarginTerm(marginZero, true);
                    grid_table_Rule.Declarations.Add(marginProperty);
                    break;
            }
        }

        private void RewriteTextAlignForKmItemRule(StyleSheet sheet)
        {
            var kmItem_Rule = sheet.StyleRules.Single(x => x.Value == string.Format("{0} {1}", kmForm, kmItem));

            var displayProperty = kmItem_Rule.Declarations.FirstOrDefault(x => x.Name.Equals(display, StringComparison.OrdinalIgnoreCase));
            var floatProperty = kmItem_Rule.Declarations.FirstOrDefault(x => x.Name.Equals(s_float, StringComparison.OrdinalIgnoreCase));

            RemoveProperty(kmItem_Rule, displayProperty);
            RemoveProperty(kmItem_Rule, floatProperty);

            displayProperty = new Property(display);
            displayProperty.Term = new PrimitiveTerm(UnitType.Ident, block);

            floatProperty = new Property(s_float);
            floatProperty.Term = CustomStyles.FormStyles.Alignment == Alignment.Right
                ? new PrimitiveTerm(UnitType.Ident, right)
                : new PrimitiveTerm(UnitType.Ident, left);

            kmItem_Rule.Declarations.Add(displayProperty);
            kmItem_Rule.Declarations.Add(floatProperty);
        }

        private void RewriteTextAlignForToggleButtons(StyleSheet sheet)
        {
            var checkBoxCssClass = GetCssClassByControlType(StyledControlType.CheckBox);
            var radioButtonCssClass = GetCssClassByControlType(StyledControlType.RadioButton);
            var newsLetterCssClass = GetCssClassByControlType(StyledControlType.NewsLetter);

            var cbRule = sheet.StyleRules
                .Single(x => x.Value.StartsWith(string.Format("{0} {1}", kmForm, checkBoxCssClass)) && x.Value.IndexOf(input) > -1);
            var rbRule = sheet.StyleRules
                .Single(x => x.Value.StartsWith(string.Format("{0} {1}", kmForm, radioButtonCssClass)) && x.Value.IndexOf(input) > -1);
            var newsRule = sheet.StyleRules
                .SingleOrDefault(x => x.Value.StartsWith(string.Format("{0} {1}", kmForm, newsLetterCssClass)) && x.Value.IndexOf(input) > -1);

            var cbfProperty = cbRule.Declarations.FirstOrDefault(x => x.Name.Equals(s_float, StringComparison.OrdinalIgnoreCase));
            var rbfProperty = rbRule.Declarations.FirstOrDefault(x => x.Name.Equals(s_float, StringComparison.OrdinalIgnoreCase));
            var newsfProperty = newsRule.Declarations.FirstOrDefault(x => x.Name.Equals(s_float, StringComparison.OrdinalIgnoreCase));

            var toggleButtonTerm = CustomStyles.FormStyles.Alignment == Alignment.Left
                ? new PrimitiveTerm(UnitType.Ident, left)
                : new PrimitiveTerm(UnitType.Ident, none);

            cbfProperty.Term = toggleButtonTerm;
            rbfProperty.Term = toggleButtonTerm;
            newsfProperty.Term = toggleButtonTerm;
        }

        private void RewriteTextAlignForHintRule(StyleSheet sheet)
        {
            var hintRule = sheet.StyleRules.SingleOrDefault(x => x.Value == string.Format("{0} {1}", kmForm, kmHint));

            if (CustomStyles.FormStyles.Alignment == Alignment.Right && hintRule == null)
            {
                var clear = new Property(s_clear);
                clear.Term = new PrimitiveTerm(UnitType.Ident, both);
                hintRule = new StyleRule();
                hintRule.Value = string.Format("{0} {1}", kmForm, kmHint);
                hintRule.Declarations.Add(clear);
                sheet.Rules.Add(hintRule);
            }
            else
            {
                RemoveRule(sheet, hintRule);
            }
        }

        private void RewriteTextAlignForFieldset(StyleSheet sheet)
        {
            const int oneHunderedPercentage = 100;
            const int tenPixels = 10;

            var fieldsetRule = sheet.StyleRules.SingleOrDefault(x => x.Value == string.Format("{0} {1}", kmForm, f_l));
            RemoveRule(sheet, fieldsetRule);

            if (CustomStyles.FormStyles.Alignment == Alignment.Center ||
                CustomStyles.FormStyles.Alignment == Alignment.Right)
            {
                fieldsetRule = new StyleRule();
                fieldsetRule.Value = string.Format("{0} {1}", kmForm, f_l);

                if (CustomStyles.FormStyles.Alignment == Alignment.Center)
                {
                    var displayProp = new Property(display);
                    var textAlignProp = new Property(text_align);
                    var widthProp = new Property(width);
                    displayProp.Term = new PrimitiveTerm(UnitType.Ident, block);
                    textAlignProp.Term = new PrimitiveTerm(UnitType.Ident, center);
                    widthProp.Term = new PrimitiveTerm(UnitType.Percentage, oneHunderedPercentage);
                    fieldsetRule.Declarations.Add(displayProp);
                    fieldsetRule.Declarations.Add(textAlignProp);
                    fieldsetRule.Declarations.Add(widthProp);
                }
                else
                {
                    var floatProp = new Property(s_float);
                    var paddingBottomProp = new Property(padding_bottom);
                    floatProp.Term = new PrimitiveTerm(UnitType.Ident, right);
                    paddingBottomProp.Term = new PrimitiveTerm(UnitType.Pixel, tenPixels);
                    fieldsetRule.Declarations.Add(floatProp);
                    fieldsetRule.Declarations.Add(paddingBottomProp);
                }

                sheet.Rules.Add(fieldsetRule);
            }
        }

        private void RewriteTextAlignForButton(StyleSheet sheet)
        {
            var buttonCssClass = GetCssClassByControlType(StyledControlType.Button);
            var buttonRule = sheet.StyleRules.Single(x => x.Value == string.Format("{0} {1}", kmForm, buttonCssClass));

            var buttonProperty = buttonRule.Declarations.First(x => x.Name.Equals(text_align, StringComparison.OrdinalIgnoreCase));
            switch (CustomStyles.FormStyles.Alignment)
            {
                case Alignment.Left:
                    buttonProperty.Term = new PrimitiveTerm(UnitType.Ident, left);
                    break;
                case Alignment.Center:
                    buttonProperty.Term = new PrimitiveTerm(UnitType.Ident, center);
                    break;
                case Alignment.Right:
                    buttonProperty.Term = new PrimitiveTerm(UnitType.Ident, right);
                    break;
            }
        }

        private void RewriteFormRules(StyleSheet sheet)
        {
            var formRule = sheet.StyleRules.Single(x => x.Value == kmForm);
            var borderColorProperty = formRule.Declarations
                .FirstOrDefault(x => x.Name.IndexOf(color, StringComparison.OrdinalIgnoreCase) > -1 && x.Name.IndexOf(border, StringComparison.OrdinalIgnoreCase) > -1);

            borderColorProperty.Term = CustomStyles.FormStyles.Border
                ? FromRgba(CustomStyles.FormStyles.BorderColor)
                : transparentColor;

            SetProperty(formRule, background_color, FromRgba(CustomStyles.FormStyles.Color));
        }

        private void RewriteBodyRules(StyleSheet sheet)
        {
            var bodyRule = sheet.StyleRules.Single(x => x.Value == body);
            SetProperty(bodyRule, background_color, FromRgba(CustomStyles.FormStyles.BackgroundColor));
        }

        private void RewriteKmFormListItemRules(StyleSheet sheet)
        {
            var form_ul_li_Rule = sheet.StyleRules.Single(x => x.Value == string.Format("{0} {1}", kmForm, ul_li));
            SetProperty(form_ul_li_Rule, margin_bottom, new PrimitiveTerm(UnitType.Pixel, CustomStyles.FormStyles.Spacing));
        }

        private void CopyButtonRulesToControlStyles()
        {
            ControlStyles button = new ControlStyles(true);
            button.BackgroundColor = CustomStyles.ButtonStyles.BackgroundColor;
            button.BorderColor = CustomStyles.ButtonStyles.BorderColor;
            button.TextColor = CustomStyles.ButtonStyles.Color;
            button.Font = CustomStyles.ButtonStyles.Font;
            button.FontBold = CustomStyles.ButtonStyles.FontBold;
            button.FontSize = CustomStyles.ButtonStyles.FontSize;
            CustomStyles.ControlsStyles[StyledControlType.Button] = button;
        }

        private StyleRule GetControlRule(StyleSheet sheet, StyledControlType controlType)
        {
            StyleRule cRule;

            var cssClass = GetCssClassByControlType(controlType);
            var cRules = sheet.StyleRules.Where(x => x.Value.IndexOf(kmForm) > -1 && x.Value.IndexOf(cssClass) > -1 && x.Value.IndexOf(label) == -1);

            switch (controlType)
            {
                case StyledControlType.RadioButton:
                case StyledControlType.CheckBox:
                case StyledControlType.NewsLetter:
                    cRule = cRules.First(x => x.Value.EndsWith(string.Format("{0} {1}", cssClass, div)));
                    break;
                case StyledControlType.DropDown:
                case StyledControlType.ListBox:
                    cRule = cRules.First(x => !x.Value.EndsWith(option));
                    break;
                case StyledControlType.Grid:
                    cRule = cRules.First(x => x.Value.EndsWith(string.Format("{0} {1}", cssClass, table)));
                    break;
                case StyledControlType.Button:
                    cRule = cRules.Single(x => !x.Value.EndsWith(cssClass));
                    break;
                default:
                    cRule = cRules.First();
                    break;
            }

            return cRule;
        }

        private StyleRule GetControlColorRule(StyleSheet sheet, StyledControlType controlType)
        {
            StyleRule cColor;
            var cssClass = GetCssClassByControlType(controlType);
            var cRules = sheet.StyleRules.Where(x => x.Value.IndexOf(kmForm) > -1 && x.Value.IndexOf(cssClass) > -1 && x.Value.IndexOf(label) == -1);
            
            switch (controlType)
            {
                case StyledControlType.RadioButton:
                case StyledControlType.CheckBox:
                case StyledControlType.NewsLetter:
                    cColor = cRules.First(x => x.Value.EndsWith(span));
                    break;
                case StyledControlType.DropDown:
                case StyledControlType.ListBox:
                    cColor = cRules.First(x => x.Value.EndsWith(option));
                    break;
                case StyledControlType.Grid:
                    cColor = cRules.First(x => x.Value.EndsWith(td) || x.Value.IndexOf(string.Format("{0},", td)) > -1);
                    break;
                case StyledControlType.Button:
                    cColor = cRules.Single(x => !x.Value.EndsWith(cssClass));
                    break;
                default:
                    cColor = cRules.First();
                    break;
            }

            return cColor;
        }

        private StyleRule GetControlBorderRule(StyleSheet sheet, StyledControlType controlType)
        {
            StyleRule cBorder;

            var cssClass = GetCssClassByControlType(controlType);
            var cRules = sheet.StyleRules.Where(x => x.Value.IndexOf(kmForm) > -1 && x.Value.IndexOf(cssClass) > -1 && x.Value.IndexOf(label) == -1);

            switch (controlType)
            {
                case StyledControlType.RadioButton:
                case StyledControlType.CheckBox:
                case StyledControlType.NewsLetter:
                    cBorder = cRules.FirstOrDefault(x => x.Value.EndsWith(string.Format("{0}>{1}", cssClass, div)));
                    break;
                case StyledControlType.DropDown:
                case StyledControlType.ListBox:
                    cBorder = cRules.First(x => !x.Value.EndsWith(option));
                    break;
                case StyledControlType.Grid:
                    cBorder = cRules.FirstOrDefault(x => x.Value.EndsWith(string.Format("{0}>{1}", cssClass, table)));
                    break;
                case StyledControlType.Button:
                    cBorder = cRules.Single(x => !x.Value.EndsWith(cssClass));
                    break;
                default:
                    cBorder = cRules.First();
                    break;
            }

            return cBorder;
        }

        private StyleRule GetControlCategoryRule(StyleSheet sheet, StyledControlType controlType)
        {
            const string categoryClass = ".category";

            StyleRule cCategory;
            var cssClass = GetCssClassByControlType(controlType);
            var cRules = sheet.StyleRules.Where(x => x.Value.IndexOf(kmForm) > -1 && x.Value.IndexOf(cssClass) > -1 && x.Value.IndexOf(label) == -1);

            switch (controlType)
            {
                case StyledControlType.RadioButton:
                case StyledControlType.CheckBox:
                case StyledControlType.NewsLetter:
                case StyledControlType.DropDown:
                case StyledControlType.ListBox:
                    cCategory = cRules.First(x => x.Value.EndsWith(string.Format("{0} {1}", cssClass, categoryClass)));
                    break;
                default:
                    cCategory = cRules.First();
                    break;
            }

            return cCategory;
        }

        private StyleRule GetControlLabelRule(StyleSheet sheet, StyledControlType controlType)
        {
            var cssClass = GetCssClassByControlType(controlType);
            var cRules = sheet.StyleRules.Where(x => x.Value.IndexOf(kmForm) > -1 && x.Value.IndexOf(cssClass) > -1 && x.Value.IndexOf(label) == -1);

            var lRule = controlType == StyledControlType.Button
                ? cRules.Single(x => !x.Value.EndsWith(cssClass))
                : sheet.StyleRules.FirstOrDefault(x => x.Value.IndexOf(kmForm) > -1 && x.Value.IndexOf(cssClass) > -1 && x.Value.EndsWith(label));

            if (lRule == null)
            {
                lRule = new StyleRule();
                lRule.Value = string.Format("{0} {1} {2}", kmForm, cssClass, label);

                var cRule = GetControlRule(sheet, controlType);
                var labelRuleIndex = sheet.StyleRules.IndexOf(cRule) + 1;
                sheet.Rules.Insert(labelRuleIndex, lRule);
            }

            return lRule;
        }

        private void RewriteControlLabelRule(StyleSheet sheet, StyledControlType controlType, ControlStyles controlStyles)
        {
            var lRule = GetControlLabelRule(sheet, controlType);

            if (controlType != StyledControlType.Button)
            {
                SetProperty(lRule, color, FromRgba(controlStyles.LabelTextColor));
                SetProperty(lRule, background_color, FromRgba(controlStyles.LabelBackgroundColor));
            }
            Property lFont = lRule.Declarations.FirstOrDefault(x => x.Name.Equals(font, StringComparison.OrdinalIgnoreCase));
            RemoveProperty(lRule, lFont);
            SetProperty(lRule, font_family, new PrimitiveTerm(UnitType.Ident, controlStyles.LabelFont));
            SetProperty(lRule, font_weight, new PrimitiveTerm(UnitType.Ident, controlStyles.LabelFontBold ? bold : normal));
            SetProperty(lRule, font_size, new PrimitiveTerm(UnitType.Pixel, controlStyles.LabelFontSize));
        }

        private void RewriteControlCategoryRule(StyleSheet sheet, StyledControlType controlType, ControlStyles controlStyles)
        {
            var cCategory = GetControlCategoryRule(sheet, controlType);

            SetProperty(cCategory, color, FromRgba(controlStyles.CategoryTextColor));
            SetProperty(cCategory, background_color, FromRgba(controlStyles.CategoryBackgroundColor));
            SetProperty(cCategory, font_family, new PrimitiveTerm(UnitType.Ident, controlStyles.CategoryFont));
            SetProperty(cCategory, font_weight, new PrimitiveTerm(UnitType.Ident, controlStyles.CategoryFontBold ? bold : normal));
            SetProperty(cCategory, font_size, new PrimitiveTerm(UnitType.Pixel, controlStyles.CategoryFontSize));
        }

        private void RewriteControlColorRule(StyleSheet sheet, StyledControlType controlType, ControlStyles controlStyles)
        {
            var cColor = GetControlColorRule(sheet, controlType);

            SetProperty(cColor, color, FromRgba(controlStyles.TextColor));
            SetProperty(cColor, font_family, new PrimitiveTerm(UnitType.Ident, controlStyles.Font));
            SetProperty(cColor, font_size, new PrimitiveTerm(UnitType.Pixel, controlStyles.FontSize));
            SetProperty(cColor, font_weight, new PrimitiveTerm(UnitType.Ident, controlStyles.FontBold ? bold : normal));
        }

        private void RewriteControlRule(StyleSheet sheet, StyledControlType controlType, ControlStyles controlStyles)
        {
            var cRule = GetControlRule(sheet, controlType);

            SetProperty(cRule, background_color, FromRgba(controlStyles.BackgroundColor));
            if (controlType == StyledControlType.DropDown || controlType == StyledControlType.ListBox)
            {
                SetProperty(cRule, color, FromRgba(controlStyles.TextColor));
                SetProperty(cRule, font_family, new PrimitiveTerm(UnitType.Ident, controlStyles.Font));
                SetProperty(cRule, font_size, new PrimitiveTerm(UnitType.Pixel, controlStyles.FontSize));
                SetProperty(cRule, font_weight, new PrimitiveTerm(UnitType.Ident, controlStyles.FontBold ? bold : normal));
            }
        }

        private void RewriteControlBorderRule(StyleSheet sheet, StyledControlType controlType, ControlStyles controlStyles)
        {
            var cBorder = GetControlBorderRule(sheet, controlType);
            var cRule = GetControlRule(sheet, controlType);

            var isButton = controlType == StyledControlType.Button;
            var isGroupControl = IsGroupControl(controlType);
            var cssClass = GetCssClassByControlType(controlType);

            if (isGroupControl)
            {
                RemoveRule(sheet, cBorder);
            }

            var bColor = controlStyles.Border || isButton
                ? FromRgba(controlStyles.BorderColor) as HtmlColor
                : FromRgba(controlStyles.BackgroundColor) as HtmlColor;

            if (isGroupControl)
            {
                cBorder = new StyleRule();
                var cBorderValueBuilder = new StringBuilder();

                cBorderValueBuilder.AppendFormat("{0} ", kmForm);
                if (controlType == StyledControlType.CheckBox ||
                    controlType == StyledControlType.RadioButton ||
                    controlType == StyledControlType.NewsLetter)
                {
                    cBorderValueBuilder.Append(li_);
                }
                
                cBorderValueBuilder.Append(cssClass);

                if (controlType == StyledControlType.CheckBox ||
                    controlType == StyledControlType.RadioButton ||
                    controlType == StyledControlType.NewsLetter)
                {
                    cBorderValueBuilder.AppendFormat(">{0}", div);
                    var tempFloat = new Property(s_float);
                    switch (CustomStyles.FormStyles.Alignment)
                    {
                        case Alignment.Left:
                            tempFloat.Term = new PrimitiveTerm(UnitType.Ident, left);
                            break;
                        case Alignment.Center:
                            tempFloat.Term = new PrimitiveTerm(UnitType.Ident, left);
                            break;
                        case Alignment.Right:
                            tempFloat.Term = new PrimitiveTerm(UnitType.Ident, right);
                            break;
                    }
                    cBorder.Declarations.Add(tempFloat);
                }
                else if (controlType == StyledControlType.Grid)
                {
                    cBorderValueBuilder.AppendFormat(">{0}", table);
                }

                cBorder.Value = cBorderValueBuilder.ToString();
                SetProperty(cBorder, border, CreateBorderTerm(bColor));
                sheet.Rules.Insert(sheet.StyleRules.IndexOf(cRule) + 1, cBorder);
            }
            else
            {
                var bcProperties = cBorder != null
                    ? cBorder.Declarations.FirstOrDefault(x => x.Name.Equals(border, StringComparison.OrdinalIgnoreCase))
                    : default(Property);

                if (bcProperties == null)
                {
                    SetProperty(cBorder, bcProperties, border, CreateBorderTerm(bColor));
                }
                else
                {
                    SetProperty(cBorder, bcProperties, border, RewriteTermWithColor(bcProperties.Term, bColor));
                }
            }
        }

        #region Private methods
        private void RemoveRule(StyleSheet sheet, StyleRule rule)
        {
            if (rule != null)
            {
                sheet.Rules.Remove(rule);
            }
        }

        private void SetProperty(StyleRule r, string name, Term t)
        {
            SetProperty(r, r.Declarations.FirstOrDefault(x => x.Name.ToLower() == name), name, t);
        }

        private void SetProperty(StyleRule r, Property p, string name, Term t)
        {
            if (p == null)
            {
                p = new Property(name);
                r.Declarations.Add(p);
            }
            p.Term = t;
        }

        private void RemoveProperty(StyleRule rule, Property property)
        {
            if (property != null)
            {
                rule.Declarations.Remove(property);
            }
        }

        private void SetColor(string name, bool allowSetColor, ControlStyles cStyles, Term t)
        {
            if (name == color && allowSetColor)
            {
                cStyles.TextColor = GetColor(t);
            }
        }

        private void SetFont(string name, ControlStyles cStyles, Term t)
        {
            if (name == font_family)
            {
                cStyles.Font = t.ToString();
                int end = cStyles.Font.IndexOf(',');
                if (end > -1)
                {
                    cStyles.Font = cStyles.Font.Substring(0, end);
                }
            }
        }

        private void SetFontSize(string name, ControlStyles cStyles, Term t)
        {
            if (name == font_size)
            {
                cStyles.FontSize = int.Parse(((PrimitiveTerm)t).Value.ToString());
            }
        }

        private void SetFontBold(string name, ControlStyles cStyles, Term t)
        {
            if (name == font_weight)
            {
                cStyles.FontBold = t.ToString().ToLower() == bold;
            }
        }

        private Term RewriteTermWithColor(Term term, string color)
        {
            return RewriteTermWithColor(term, (HtmlColor)FromRgba(color));
        }

        private Term RewriteTermWithColor(Term term, HtmlColor color)
        {
            TermList lst = new TermList();
            if (term is TermList)
            {
                foreach (var t in (TermList)term)
                {
                    AddNotColor(t, lst);
                }
            }
            else
            {
                AddNotColor(term, lst);
            }
            lst.AddTerm(color);

            return lst;
        }

        private void AddNotColor(Term term, TermList lst)
        {
            if (!(term is HtmlColor))
            {
                lst.AddTerm(term);
                lst.AddSeparator(TermList.TermSeparator.Space);
            }
        }

        private Term FromRgba(string color)
        {
            HtmlColor res = null;
            if (color.StartsWith("#"))
            {
                res = HtmlColor.FromHex(color.TrimStart('#'));
            }
            else
            {
                Match match = RgbaRex.Match(color.ToLower());
                res = HtmlColor.FromRgba(byte.Parse(match.Groups[1].Value), byte.Parse(match.Groups[2].Value),
                                            byte.Parse(match.Groups[3].Value), double.Parse(match.Groups[4].Value));
            }

            return res;
        }

        private string GetColor(Term term)
        {
            string res = term.ToString();
            if (term is HtmlColor)
            {
                HtmlColor color = term as HtmlColor;
                res = string.Format(RgbaFormat, color.R, color.G, color.B, Math.Round(color.Alpha, 2));
            }

            return res;
        }

        //private Term RewriteMargin(Term term, bool isAuto)
        //{
        //    int marginValue = 0;
        //    if (term != null)
        //    {
        //        if (term is TermList)
        //        {
        //            foreach (var t in (TermList)term)
        //            {
        //                if (TrySetMargin(t, ref marginValue))
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            TrySetMargin(term, ref marginValue);
        //        }
        //    }

        //    TermList lst = new TermList();
        //    lst.AddTerm(new PrimitiveTerm(UnitType.Pixel, marginValue));
        //    if (isAuto)
        //    {
        //        lst.AddSeparator(TermList.TermSeparator.Space);
        //        lst.AddTerm(new PrimitiveTerm(UnitType.Ident, auto));
        //    }

        //    return lst;
        //}

        //private bool TrySetMargin(Term term, ref int marginValue)
        //{
        //    bool res = false;
        //    if (term is PrimitiveTerm)
        //    {
        //        PrimitiveTerm pt = (PrimitiveTerm)term;
        //        if (pt.PrimitiveType == UnitType.Pixel || pt.PrimitiveType == UnitType.Number)
        //        {
        //            try
        //            {
        //                marginValue = int.Parse(pt.Value.ToString());
        //                res = true;
        //            }
        //            catch
        //            { }
        //        }
        //    }

        //    return res;
        //}

        private Term SetMarginTerm(int margin, bool isAuto)
        {
            TermList lst = new TermList();
            lst.AddTerm(new PrimitiveTerm(UnitType.Pixel, margin));
            if (isAuto)
            {
                lst.AddSeparator(TermList.TermSeparator.Space);
                lst.AddTerm(new PrimitiveTerm(UnitType.Ident, auto));
            }

            return lst;
        }

        private void CheckTermForFont(Term t, ControlStyles cStyles)
        {
            if (t is PrimitiveTerm)
            {
                PrimitiveTerm pt = (PrimitiveTerm)t;
                string value = pt.Value.ToString();
                if (pt.PrimitiveType == UnitType.Pixel || pt.PrimitiveType == UnitType.Number)
                {
                    cStyles.LabelFontSize = int.Parse(value);
                }
                else
                {
                    if (value == bold || value == normal)
                    {
                        cStyles.LabelFontBold = value == bold;
                    }
                    else
                    {
                        cStyles.LabelFont = value;
                    }
                }
            }
        }

        private bool CheckTermForBorderColor(Term t, ControlStyles cStyles)
        {
            bool res = false;
            if (t is HtmlColor)
            {
                cStyles.BorderColor = GetColor(t);
                cStyles.Border = true;
                res = true;
            }

            return res;
        }

        private bool CheckTermForBorderColor(Term t, FormStyles fStyles)
        {
            bool res = false;
            if (t is HtmlColor)
            {
                fStyles.BorderColor = GetColor(t);
                fStyles.Border = true;
                res = true;
            }

            return res;
        }

        private Term CreateBorderTerm(HtmlColor bColor)
        {
            TermList term = new TermList();
            term.AddTerm(new PrimitiveTerm(UnitType.Pixel, 1));
            term.AddSeparator(TermList.TermSeparator.Space);
            term.AddTerm(new PrimitiveTerm(UnitType.Ident, solid));
            term.AddSeparator(TermList.TermSeparator.Space);
            term.AddTerm(bColor);

            return term;
        }

        private StyledControlType? GetControlType(string rule)
        {
            StyledControlType? type = null;
            foreach (var c in rule.Split(new char[] { ' ', '>' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (c.StartsWith(kmPrefix))
                {
                    foreach (var strType in ControlTypes)
                    {
                        if (c.Substring(kmPrefix.Length).ToLower() == strType.ToLower())
                        {
                            type = (StyledControlType)Enum.Parse(typeof(StyledControlType), strType);
                            break;
                        }
                    }
                }
                if (type.HasValue)
                {
                    break;
                }
            }

            return type;
        }

        private string GetCssClassByControlType(StyledControlType cType)
        {
            string type = cType.ToString();

            return kmPrefix + type.Substring(0, 1) + type.Substring(1).ToLower();
        }

        private bool IsGroupControl(StyledControlType type)
        {
            return type == StyledControlType.CheckBox || type == StyledControlType.RadioButton || 
                    type == StyledControlType.NewsLetter || type == StyledControlType.Grid;
        }
        #endregion
    }

    public class CustomStyles
    {
        public FormStyles FormStyles { get; set; }

        public ButtonStyles ButtonStyles { get; set; }

        public IDictionary<StyledControlType, ControlStyles> ControlsStyles { get; set; }

        //public IDictionary<StyledControlType, ControlStyles> ControlsStylesWithoutButton { get; set; }
        //{
        //    get
        //    {
        //        return ControlsStyles.Where(x => x.Key != StyledControlType.Button).ToDictionary(x => x.Key, x => x.Value);
        //    }
        //}
    }

    public class FormStyles
    {
        public FormStyles()
        { }

        public FormStyles(bool setDefaultValues = true)
        {
            if (setDefaultValues)
            {
                Color = "rgba(255, 255, 255, 1)";
                BackgroundColor = "rgba(255, 255, 255, 1)";
                Border = false;
                BorderColor = "rgba(0, 0, 0, 0)";
                Alignment = Alignment.Center;
                Spacing = 12;
            }
        }

        public string Color { get; set; }

        public string BackgroundColor { get; set; }

        public bool Border { get; set; }

        public string BorderColor { get; set; }

        public Alignment Alignment { get; set; }

        public int Spacing { get; set; }
    }

    public class ControlStyles
    {
        public ControlStyles()
        { }

        public ControlStyles(ControlStyles defStyle) : this(true)
        {
            if (defStyle != null)
            {
                // TODO: Complete member initialization
                LabelFont = defStyle.LabelFont;
                LabelFontBold = defStyle.LabelFontBold;
                LabelFontSize = defStyle.LabelFontSize;
                LabelTextColor = defStyle.LabelTextColor;
                LabelBackgroundColor = defStyle.LabelBackgroundColor;
            }
        }

        public ControlStyles(bool setDefaultValues = true) 
        {
            if (setDefaultValues) 
            {
                LabelTextColor = "rgba(0, 0, 0, 1)";
                LabelFont = "Arial";
                LabelFontSize = 12;
                LabelFontBold = false;
                LabelBackgroundColor = "rgba(255, 255, 255, 1)";
                CategoryTextColor = "rgba(0, 0, 0, 1)";
                CategoryFont = "Arial";
                CategoryFontSize = 12;
                CategoryFontBold = false;
                CategoryBackgroundColor = "rgba(255, 255, 255, 1)";
                Font = "Arial";
                FontSize = 12;
                FontBold = false;
                TextColor = "rgba(0, 0, 0, 1)";
                Border = false;
                BorderColor = "rgba(0, 0, 0, 1)";
                BackgroundColor = "rgba(255, 255, 255, 1)";
            }
        }

        public string LabelTextColor { get; set; }

        public string LabelFont { get; set; }

        public int LabelFontSize { get; set; }

        public bool LabelFontBold { get; set; }

        public string LabelBackgroundColor { get; set; }

        public string CategoryTextColor { get; set; }

        public string CategoryFont { get; set; }

        public int CategoryFontSize { get; set; }

        public bool CategoryFontBold { get; set; }

        public string CategoryBackgroundColor { get; set; }

        public string TextColor { get; set; }

        public string Font { get; set; }

        public int FontSize { get; set; }

        public bool FontBold { get; set; }

        public bool Border { get; set; }

        public string BorderColor { get; set; }

        public string BackgroundColor { get; set; }
    }

    public class ButtonStyles 
    {
        public ButtonStyles() 
        {
        }

        public ButtonStyles(bool setDefaultValues = false) 
        {
            if (setDefaultValues) 
            {
                Color = "rgba(0, 0, 0, 1)";
                Font = "Arial";
                FontSize = 12;
                FontBold = false;
                BorderColor = "rgba(0, 0, 0, 1)";
                BackgroundColor = "rgba(255, 255, 255, 1)";
            }
        }

        public string Color { get; set; }

        public string Font { get; set; }

        public int FontSize { get; set; }

        public bool FontBold { get; set; }

        public string BorderColor { get; set; }

        public string BackgroundColor { get; set; }
    }
}