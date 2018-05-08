namespace ActiveUp.WebControls
{
    internal enum ParseStatus
    {
        ReadText = 0,
        ReadEndTag = 1,
        ReadStartTag = 2,
        ReadAttributeName = 3,
        ReadAttributeValue = 4
    };
}