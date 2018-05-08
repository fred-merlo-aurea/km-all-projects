using System.Collections.Generic;
using System.Collections.Specialized;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;

namespace UAS.UnitTests.AMS_Operations
{
    public partial class FileValidatorTest
    {
        private static readonly object[] GetSetupTransformationDataTableTestCases =
        {
            new object[]
            {
                "pubcode", IncomingField, "multi-field-pubcode",
                new StringDictionary
                {
                    {"incoming-field-2", string.Empty},
                    {"originalimportrow", "1"},
                    {"incoming-field", "incoming-field"},
                    {"multi-pubcode", string.Empty},
                    {"pubcode", "pubcode"}
                },
                new StringDictionary
                {
                    {"incoming-field-2", string.Empty},
                    {"originalimportrow", "1"},
                    {"incoming-field", "incoming-field"},
                    {"multi-pubcode", string.Empty},
                    {"pubcode", "pubcode"}
                },
                new StringDictionary
                {
                    {"pubcode", "1"},
                    {"incoming-field-2", "2"},
                    {"originalimportrow", "3"}
                },
                new StringDictionary
                {
                    {"pubcode", "1"},
                    {"incoming-field-2", "2"},
                    {"originalimportrow", "3"}
                }
            },
            new object[]
            {
                "pubcode", string.Empty, "originalimportrow",
                new StringDictionary
                {
                    {"-2", string.Empty},
                    {"pubcode", "pubcode"},
                    {"multi-pubcode", string.Empty},
                    {"originalimportrow", "1"}
                },
                new StringDictionary
                {
                    {"-2", string.Empty},
                    {"pubcode", "pubcode"},
                    {"multi-pubcode", string.Empty},
                    {"originalimportrow", "1"}
                },
                new StringDictionary
                {
                    {"-2", "2"},
                    {"pubcode", "1"},
                    {"originalimportrow", "3"}
                },
                new StringDictionary
                {
                    {"-2", "1"},
                    {"pubcode", "2"},
                    {"originalimportrow", "3"}
                }
            },
            new object[]
            {
                "non-pub-code", "non-pub-code-incoming-field", "multi-field-non-pub-code",
                new StringDictionary
                {
                    {"non-pub-code-incoming-field", "non-pub-code-incoming-field"},
                    {"non-pub-code-incoming-field-2", string.Empty},
                    {"multi-non-pub-code", string.Empty},
                    {"originalimportrow", "1"},
                    {"non-pub-code", "non-pub-code"}
                },
                new StringDictionary
                {
                    {"non-pub-code-incoming-field", "non-pub-code-incoming-field"},
                    {"non-pub-code-incoming-field-2", string.Empty},
                    {"multi-non-pub-code", string.Empty},
                    {"originalimportrow", "1"},
                    {"non-pub-code", "non-pub-code"}
                },
                new StringDictionary
                {
                    {"originalimportrow", "3"},
                    {"non-pub-code-incoming-field-2", "2"},
                    {"non-pub-code-incoming-field", "1"}
                },
                new StringDictionary
                {
                    {"originalimportrow", "3"},
                    {"non-pub-code-incoming-field-2", "1"},
                    {"non-pub-code-incoming-field", "2"}
                }
            },
            new object[]
            {
                "non-pub-code", string.Empty, "originalimportrow",
                new StringDictionary
                {
                    {"-2", string.Empty},
                    {"multi-non-pub-code", string.Empty},
                    {"originalimportrow", "1"},
                    {"non-pub-code", "non-pub-code"},
                    {string.Empty, string.Empty}
                },
                new StringDictionary
                {
                    {"-2", string.Empty},
                    {"multi-non-pub-code", string.Empty},
                    {"originalimportrow", "1"},
                    {"non-pub-code", "non-pub-code"},
                    {string.Empty, string.Empty}
                },
                new StringDictionary
                {
                    {"-2", "2"},
                    {string.Empty, "1"},
                    {"originalimportrow", "3"}
                },
                new StringDictionary
                {
                    {"-2", "1"},
                    {string.Empty, "2"},
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
                    {"originalimportrow", "1"}
                },
                new StringDictionary
                {
                    {"-2", string.Empty},
                    {string.Empty, string.Empty},
                    {"originalimportrow", "1"}
                },
                new StringDictionary
                {
                    {"-2", "2"},
                    {string.Empty, "1"},
                    {"originalimportrow", "3"}
                },
                new StringDictionary
                {
                    {"-2", "1"},
                    {string.Empty, "2"},
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
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {IncomingField, "incoming-field"},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
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
                        {"pub-code", "Numbers|1,2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
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
                        {"pub-code", "Numbers|1|2|3|4"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2|3|4"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
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
                        {"pub-code", "1~2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
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
                        {"pub-code", "4,5"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "4,5"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
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
                        {"pub-code", "pipe"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "pipe"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
                        {"multi-pub-code", string.Empty}
                    }
                }
            },
            new object[]
            {
                string.Empty, "Is_Null_or_Empty", string.Empty, "fill_data", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"pub-code", string.Empty},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
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
                        {"pub-code", "Numbers|1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
                        {"multi-pub-code", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "4"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "incoming-field"},
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