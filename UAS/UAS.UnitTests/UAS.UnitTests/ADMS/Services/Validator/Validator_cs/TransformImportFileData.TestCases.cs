using System.Collections.Generic;
using System.Collections.Specialized;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    public partial class TransformImportFileDataTest
    {
        private static readonly object[] GetSetupTransformationDataTableTestCases =
        {
            new object[]
            {
                "pubcode", IncomingField, "multi-field-pubcode", new StringDictionary
                {
                    {"incoming-field-2", string.Empty},
                    {"pubcodeid", "1"},
                    {"originalimportrow", "1"},
                    {IncomingField, IncomingField},
                    {"multi-pubcode", string.Empty},
                    {"pubcode", "pubcode"}
                },
                new StringDictionary
                {
                    {"originalimportrow", "4"},
                    {"pubcode", "1"},
                    {"incoming-field-2", "3"},
                    {"pubcodeid", "2"}
                },
                new StringDictionary
                {
                    {"pubcodeid", "4"},
                    {"pubcode", "1"},
                    {"incoming-field-2", "2"},
                    {"originalimportrow", "3"}
                }
            },
            new object[]
            {
                "pubcode", string.Empty, "originalimportrow", new StringDictionary
                {
                    {"-2", string.Empty},
                    {"pubcodeid", "2"},
                    {"originalimportrow", "1"},
                    {"multi-pubcode", string.Empty},
                    {"pubcode", "pubcode"}
                },
                new StringDictionary
                {
                    {"-2", "3"},
                    {"pubcode", "1"},
                    {"originalimportrow", "4"},
                    {"pubcodeid", "2"}
                },
                new StringDictionary
                {
                    {"-2", "1"},
                    {"pubcode", "2"},
                    {"pubcodeid", "4"},
                    {"originalimportrow", "3"}
                }
            },
            new object[]
            {
                "non-pub-code", "non-pub-code-incoming-field", "multi-field-non-pub-code",
                new StringDictionary
                {
                    {"non-pub-code-incoming-field", "non-pub-code-incoming-field"},
                    {"pubcodeid", "0"},
                    {"non-pub-code-incoming-field-2", string.Empty},
                    {"multi-non-pub-code", string.Empty},
                    {"originalimportrow", "1"},
                    {"non-pub-code", "non-pub-code"}
                },
                new StringDictionary
                {
                    {"originalimportrow", "4"},
                    {"non-pub-code-incoming-field", "1"},
                    {"non-pub-code-incoming-field-2", "3"},
                    {"pubcodeid", "2"}
                },
                new StringDictionary
                {
                    {"originalimportrow", "3"},
                    {"non-pub-code-incoming-field-2", "2"},
                    {"pubcodeid", "4"},
                    {"non-pub-code-incoming-field", "1"}
                }
            },
            new object[]
            {
                "non-pub-code", string.Empty, "originalimportrow",
                new StringDictionary
                {
                    {"-2", string.Empty},
                    {"pubcodeid", "0"},
                    {"multi-non-pub-code", string.Empty},
                    {"originalimportrow", "1"},
                    {"non-pub-code", "non-pub-code"},
                    {string.Empty, string.Empty}
                },
                new StringDictionary
                {
                    {"-2", "3"},
                    {string.Empty, "1"},
                    {"originalimportrow", "4"},
                    {"pubcodeid", "2"}
                },
                new StringDictionary
                {
                    {"-2", "1"},
                    {string.Empty, "2"},
                    {"pubcodeid", "4"},
                    {"originalimportrow", "3"}
                }
            },
            new object[]
            {
                string.Empty, string.Empty, string.Empty,
                new StringDictionary
                {
                    {"-2", string.Empty},
                    {string.Empty, string.Empty},
                    {"originalimportrow", "1"},
                    {"pubcodeid", "0"}
                },
                new StringDictionary
                {
                    {"-2", "3"},
                    {string.Empty, "1"},
                    {"originalimportrow", "4"},
                    {"pubcodeid", "2"}
                },
                new StringDictionary
                {
                    {"-2", "1"},
                    {string.Empty, "2"},
                    {"pubcodeid", "4"},
                    {"originalimportrow", "3"}
                }
            }
        };

        private static readonly object[] GetSplitTransformTestCases =
        {
            new object[]
            {
                "comma", "Any Character", "Numbers|1|2,3", " 1|2 ", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "1|2"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "3"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    }
                }
            },
            new object[]
            {
                "tab", "Equals", "Numbers|1,2\t3", " Numbers|1,2 ", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "Numbers|1,2"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "3"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    }
                }
            },
            new object[]
            {
                "semicolon", "Not Equals", "Numbers|1|2;3", " Numbers|1|2|3|4 ", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "Numbers|1|2|3|4"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "Numbers|1|2|3|4"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    }
                }
            },
            new object[]
            {
                "colon", "Like", "Numbers|1~2:3", "1~2", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "1~2"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "3"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    }
                }
            },
            new object[]
            {
                "tild", "Has Data", "Numbers|1,2,3~4", " 4,5 ", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "4,5"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "4,5"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    }
                }
            },
            new object[]
            {
                "pipe", "Default", "Numbers~1_2:3|4", " pipe ", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "pipe"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "pipe"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    }
                }
            },
            new object[]
            {
                string.Empty, "Is Null_or_Empty", string.Empty, "fill_data", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"pubcodeid", "0"},
                        {"pub-code", string.Empty},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    }
                }
            },
            new object[]
            {
                string.Empty, "Find_and_Replace", "Numbers|1|2,3,4 ", " 1|2 ", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "Numbers|1|2"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "3"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"incoming-field-2", string.Empty},
                        {"pubcodeid", "0"},
                        {"pub-code", "4"},
                        {"originalimportrow", "1"},
                        {IncomingField, IncomingField},
                        {"multi-pub-code", string.Empty}
                    }
                }
            }
        };

        private static readonly object[] GetDataMapTransformTestCases =
        {
            new object[] {"Any_Character", "Numbers|1|2,3", " 1|2 ", " 1|2 "},
            new object[] {"Equals", "numbers|1,2\t3", "numbers|1,2\t3", "numbers|1,2\t3"},
            new object[] {"Not_Equals", "Numbers|1|2;3", " Numbers|1|2|3|4 ", " Numbers|1|2|3|4 "},
            new object[] {"Like", "numbers|1~2:3", "numbers|1~2:3", "numbers|1~2:3"},
            new object[] {"Find_and_Replace", "Numbers|1|2,3,4", " 1|2 ", "Numbers|1|2,3,4"}
        };
    }
}