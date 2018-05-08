using System.Collections.Generic;
using System.Collections.Specialized;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;

namespace UAS.UnitTests.AMS_Operations
{
    public partial class FileValidatorTest
    {
        private static readonly object[] GetTransformByPubCodeTestCases =
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
                        {IncomingField, "1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {IncomingField, "1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    }
                }
            },
            new object[]
            {
                "tab", "Equals", "Numbers|1,2\t3", "Numbers|1,2\t3", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1,2	3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {IncomingField, "Numbers|1,2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1,2	3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {IncomingField, "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1,2	3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {IncomingField, "Numbers|1,2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1,2	3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {IncomingField, "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
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
                        {IncomingField, "Numbers|1|2|3|4"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2|3|4"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {IncomingField, "Numbers|1|2|3|4"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
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
                        {IncomingField, "1~2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1~2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {IncomingField, "1~2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
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
                        {IncomingField, "pipe"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "pipe"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {IncomingField, "pipe"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
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
                        {IncomingField, string.Empty},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    }
                }
            },
            new object[]
            {
                string.Empty, "Find_and_Replace", "Numbers|1|2,3,4 ", " 1|2 ", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3,4"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {IncomingField, "Numbers|1|2,3,4"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    }
                }
            }
        };

        private static readonly object[] GetJoinTransformTestCases =
        {
            new object[]
            {
                "Equals", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "Numbers|1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "Numbers|1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    }
                }
            },
            new object[]
            {
                "Not Equals", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    }
                }
            },
            new object[]
            {
                "Like", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    }
                }
            },
            new object[]
            {
                "Has Data", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    }
                }
            },
            new object[]
            {
                "Default", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "1|2"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    }
                }
            },
            new object[]
            {
                "Is_Null_or_Empty", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "Numbers|1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "Numbers|1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    }
                }
            },
            new object[]
            {
                "Find_and_Replace", new List<StringDictionary>
                {
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "Numbers|1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "Numbers|1|2"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    },
                    new StringDictionary
                    {
                        {"pub-code", "Numbers|1|2,3"},
                        {"incoming-field-2", string.Empty},
                        {"originalimportrow", "1"},
                        {"incoming-field", "3"},
                        {"multi-pub-code", string.Empty},
                        {"pubcode", string.Empty}
                    }
                }
            }
        };
    }
}