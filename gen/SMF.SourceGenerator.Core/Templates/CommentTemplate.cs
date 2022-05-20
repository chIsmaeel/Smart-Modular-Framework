namespace SMF.SourceGenerator.Core.Templates;

using System.Text;

/// <summary>
/// CommentTemplate.
/// </summary>
public static class CommentTemplate
{
    /// <summary>
    /// Creates the simple type comment from text.
    /// </summary>
    /// <param name="typeComment">The type comment.</param>
    /// <returns>A string.</returns>
    public static string CreateCommentFromText(string typeComment)
    {
        return $"/// <summary> {typeComment} </summary>";
    }

    /// <summary>
    /// Creates the comment from identifier name.
    /// </summary>
    /// <param name="identifierName">The identifier name.</param>
    /// <returns>A string.</returns>
    public static string CreateCommentFromIdentifierName(string identifierName)
    {
        return $"/// <summary> {identifierName}. </summary>";
    }

    /// <summary>
    /// Gets the comment from c d s.
    /// </summary>
    /// <param name="comment">The comment.</param>
    /// <returns>A string? .</returns>
    public static string? GetCommentToken(string? comment)
    {
        if (comment is null) return null;
        var chSpan = comment!.Trim().AsSpan();
        var startingSummaryIndex = chSpan.IndexOf("/// <summary>".AsSpan());
        var endingSummaryIndex = chSpan.IndexOf("/// </summary>".AsSpan());
        endingSummaryIndex = endingSummaryIndex > 0 ? endingSummaryIndex : chSpan.IndexOf("</summary>".AsSpan());
        if (startingSummaryIndex >= 0 && endingSummaryIndex > 0)
        {
            var tempSB = new StringBuilder();
            //tempSB.Append("/// <summary>");
            startingSummaryIndex += 13;
            var summary = chSpan.Slice(startingSummaryIndex, endingSummaryIndex - startingSummaryIndex);
            if (summary.Length <= 0) return null;
            //tempSB.Append(' ');
            tempSB.Append(summary.ToString().Replace(" \r\n", " ").Replace("\r\n", " ").Replace("/// ", "").Trim());
            //tempSB.Append(' ');
            //tempSB.Append("/// </summary>");
            return tempSB.ToString();
        }
        return null;
    }

    /// <summary>
    /// Converts the to single line.
    /// </summary>
    /// <param name="comment">The comment.</param>
    /// <returns>A string? .</returns>
    //public static string? ConvertToSingleLine(string? comment)
    //{
    //    //Debug.Assert(false, comment);
    //    string tempComment = comment!.Trim();
    //    //return tempComment;
    //    var startingSummaryIndex = tempComment.IndexOf("/// <summary>");
    //    var endingSummaryIndex = tempComment.IndexOf("/// </summary>");
    //    endingSummaryIndex = endingSummaryIndex > 0 ? endingSummaryIndex : tempComment.IndexOf("</summary>");
    //    if (startingSummaryIndex >= 0 && endingSummaryIndex > 0)
    //    {
    //        var tempSB = new StringBuilder();
    //        //tempSB.Append("/// <summary>");
    //        startingSummaryIndex += 13;
    //        var summary = tempComment.Substring(startingSummaryIndex, endingSummaryIndex - startingSummaryIndex);
    //        if (summary.Length < 1) return null;
    //        return summary.Replace(" \r\n", " ").Replace("\r\n", " ").Replace("/// ", "").Trim();
    //        //tempSB.Append(' ');  
    //        //tempSB.Append();
    //        //tempSB.Append(' ');
    //        //tempSB.Append("</summary>");
    //        //Debug.Assert(false, summary);
    //        //return tempSB.ToString();
    //    }
    //    return null;
    //}
}
